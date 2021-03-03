using HomeHelper.Services.Account;
using HomeHelper.Services.MqttMessage;
using Microsoft.Extensions.DependencyInjection;

namespace HomeHelper.Services
{
    public static class ServiceRegistration
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IMqttMessageService, MqttMessageService>();
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}
