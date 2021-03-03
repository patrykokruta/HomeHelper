using HomeHelper.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeHelper.DB
{
    public static class DbServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedAccount = false;
                config.SignIn.RequireConfirmedEmail = false;
                config.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
            });
        }
    }
}
