using HomeHelper.DTO.Account;
using HomeHelper.DTO.Authentication;
using HomeHelper.Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Services.Account
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<Response<string>> RegisterAsync(RegisterRequest request);
        Task LogoutAsync();

        Task<bool> IsEmailTakenAsync(string email);
    }
}
