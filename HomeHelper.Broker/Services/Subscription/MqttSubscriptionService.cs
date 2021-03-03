using HomeHelper.Common.Settings;
using HomeHelper.CQRS.MqttDevices.Queries.GetMqttDeviceById;
using MediatR;
using Microsoft.Extensions.Configuration;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Broker.Services.Subscription
{
    public class MqttSubscriptionService : IMqttSubscriptionService
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly MqttClientSettings _clientSettings;
        private readonly Zigbee2mqttSettings _zigbeeSettings;
        private IMqttServer _mqttServer;
        public MqttSubscriptionService(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config = config;
            _clientSettings = new MqttClientSettings();
            _zigbeeSettings = new Zigbee2mqttSettings();
            config.GetSection(nameof(MqttClientSettings)).Bind(_clientSettings);
            config.GetSection(nameof(Zigbee2mqttSettings)).Bind(_zigbeeSettings);
        }
        public void ConfigureMqttServer(IMqttServer mqttServer)
        {
            _mqttServer = mqttServer;
            mqttServer.ClientSubscribedTopicHandler = this;
            mqttServer.ClientUnsubscribedTopicHandler = this;
        }

        public void ConfigureMqttServerOptions(AspNetMqttServerOptionsBuilder options)
        {
            options.WithSubscriptionInterceptor(this);
            options.WithUnsubscriptionInterceptor(this);
        }

        public Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
        {
            return Task.FromResult("");
        }

        public Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
        {
            return Task.FromResult("");
        }

        public async Task InterceptSubscriptionAsync(MqttSubscriptionInterceptorContext context)
        {
            if (context.ClientId == _clientSettings.Id || context.ClientId == _zigbeeSettings.Id)
            {
                context.AcceptSubscription = true;
            }
            else
            {
                var mqttDevice = await _mediator.Send(new GetMqttDeviceByIdQuery() { Id = context.ClientId });

                if (!mqttDevice.IsConfirmed)
                {
                    context.AcceptSubscription = false;
                }
                else
                {
                    context.AcceptSubscription = true;
                }
            }
        }

        public Task InterceptUnsubscriptionAsync(MqttUnsubscriptionInterceptorContext context)
        {
            return Task.FromResult(context.AcceptUnsubscription = true);
        }
    }
}
