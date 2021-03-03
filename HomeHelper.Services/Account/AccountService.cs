using HomeHelper.Common.Wrappers;
using HomeHelper.Domain;
using HomeHelper.DTO.Account;
using HomeHelper.DTO.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeHelper.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }
        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new Response<AuthenticationResponse>("Email does not exists.");
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, request.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return new Response<AuthenticationResponse>("Failed login attempt.");
            }

            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.Name = user.Name;

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.Name}");
        }
        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            var stringBuilder = new MySqlConnectionStringBuilder();
            stringBuilder.ConnectionString = _config.GetConnectionString("DefaultConnection");

            if (!stringBuilder.Password.Equals(request.DbPassword))
            {
                return new Response<string>("Database password is incorrect.");
            }

            var user = new ApplicationUser()
            {
                Email = request.Email,
                Name = request.Name,
                UserName = request.UserName

            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                return new Response<string>($"Email {request.Email} is already registered.");
            }

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {

                return new Response<string>("Something went wrong while creating new user.")
                {
                    Errors = CreateIdentityErrorsList(result.Errors)
                };
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                user, request.Password, isPersistent: false, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                return new Response<string>("Something went wrong while logging new user in.");
            }

            return new Response<string>(user.Id, "Registered new user.");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                return true;
            }
            return false;
        }

        private List<string> CreateIdentityErrorsList(IEnumerable<IdentityError> identityErrors)
        {
            List<string> errors = new List<string>();
            foreach (var err in identityErrors)
            {
                errors.Add(err.Description);
            }
            return errors;
        }
    }
}
