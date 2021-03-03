using HomeHelper.Client.Options;
using HomeHelper.Client.Services;
using HomeHelper.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;

namespace HomeHelper.Client.Configurations
{
    public static class MqttClientConfiguration
    {
        public static void AddHostedMqttClient(this IServiceCollection services, IConfiguration configuration)
        {
            var clientSettings = new MqttClientSettings();
            var brokerSettings = new MqttBrokerSettings();
            configuration.GetSection(nameof(MqttClientSettings)).Bind(clientSettings);
            configuration.GetSection(nameof(MqttBrokerSettings)).Bind(brokerSettings);
            services.AddConfiguredMqttClientService(optionBuilder =>
            {
                optionBuilder
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(20))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId(clientSettings.Id)
                    .WithCredentials(clientSettings.UserName, clientSettings.Password)
                    .WithTcpServer(brokerSettings.Host, brokerSettings.Port)
                    .Build());
            });

            //services.AddTransient<BrokerCommands>();
        }

        private static IServiceCollection AddConfiguredMqttClientService(this IServiceCollection services,
                    Action<AspCoreManagedMqttClientOptionBuilder> configuration)
        {

            services.AddSingleton<IManagedMqttClientOptions>(serviceProvider =>
            {
                var optionsBuilder = new AspCoreManagedMqttClientOptionBuilder(serviceProvider);
                configuration(optionsBuilder);
                return optionsBuilder.Build();
            });
            services.AddSingleton<MqttClientService>();



            services.AddSingleton<IHostedService>(serviceProvider =>
            {
                return serviceProvider.GetService<MqttClientService>();
            });
            services.AddTransient<MqttClientServiceProvider>(serviceProvider =>
            {
                var mqttClientService = serviceProvider.GetService<MqttClientService>();
                var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
                return mqttClientServiceProvider;
            });
            return services;
        }
    }
}
