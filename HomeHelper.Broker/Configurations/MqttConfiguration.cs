using HomeHelper.Broker.Services.Connection;
using HomeHelper.Broker.Services.Publishing;
using HomeHelper.Broker.Services.Server;
using HomeHelper.Broker.Services.Subscription;
using HomeHelper.Common.Broker;
using HomeHelper.Common.Zigbee;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.Extensions;
using System.Reflection;

namespace HomeHelper.Broker.Configurations
{
    public static class MqttConfiguration
    {
        public static void ConfigureMqttService(this IServiceCollection services)
        {
            services.AddSingleton<BrokerCommands>();
            services.AddSingleton<BrokerEvents>();
            services.AddSingleton<ZigbeeEvents>();
            services.AddSingleton<ZigbeeCommands>();
            services.AddSingleton<IMqttConnectionService, MqttConnectionService>();
            services.AddSingleton<IMqttSubscriptionService, MqttSubscriptionService>();
            services.AddSingleton<IMqttPublishingService, MqttPublishingService>();
            services.AddSingleton<MqttServerService>();

            services.AddHostedMqttServerWithServices(options =>
            {
                var mqttService = options.ServiceProvider.GetRequiredService<MqttServerService>();
                mqttService.ConfigureMqttServerOptions(options);
            });
            services.AddMqttConnectionHandler();
            services.AddMqttWebSocketServerAdapter();
            services.AddConnections();
        }

        public static void UseConfiguredMqttServer(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapMqtt("/mqtt");
            });
            app.UseMqttServer(mqttServer =>
            {
                app.ApplicationServices.GetRequiredService<MqttServerService>().ConfigureMqttServer(mqttServer);
            });
        }
    }
}
