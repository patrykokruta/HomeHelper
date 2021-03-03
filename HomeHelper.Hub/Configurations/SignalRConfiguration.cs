using HomeHelper.Client.Hubs;
using Microsoft.AspNetCore.Builder;

namespace HomeHelper.Client.Configurations
{
    public static class SignalRConfiguration
    {
        public static void UseConfiguredSignalR(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<BrokerHub>("/brokerhub");
            });
        }
    }
}
