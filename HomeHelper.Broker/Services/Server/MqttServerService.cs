using HomeHelper.Broker.Services.Connection;
using HomeHelper.Broker.Services.Publishing;
using HomeHelper.Broker.Services.Subscription;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Broker.Services.Server
{
    public class MqttServerService : IMqttServerService
    {
        private readonly IMqttConnectionService _mqttConnectionService;
        private readonly IMqttSubscriptionService _mqttSubscriptionService;
        private readonly IMqttPublishingService _mqttPublishingService;

        public MqttServerService(
            IMqttConnectionService mqttConnectionService,
            IMqttSubscriptionService mqttSubscriptionService,
            IMqttPublishingService mqttPublishingService)
        {
            _mqttConnectionService = mqttConnectionService;
            _mqttSubscriptionService = mqttSubscriptionService;
            _mqttPublishingService = mqttPublishingService;
        }
        public void ConfigureMqttServer(IMqttServer mqttServer)
        {
            _mqttConnectionService.ConfigureMqttServer(mqttServer);
            _mqttSubscriptionService.ConfigureMqttServer(mqttServer);
            _mqttPublishingService.ConfigureMqttServer(mqttServer);
        }

        public void ConfigureMqttServerOptions(AspNetMqttServerOptionsBuilder options)
        {
            _mqttConnectionService.ConfigureMqttServerOptions(options);
            _mqttSubscriptionService.ConfigureMqttServerOptions(options);
            _mqttPublishingService.ConfigureMqttServerOptions(options);
            options.WithoutDefaultEndpoint();
        }
    }
}
