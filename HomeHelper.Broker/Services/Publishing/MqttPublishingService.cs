using HomeHelper.Common.Settings;
using HomeHelper.CQRS.MqttDevices.Queries.GetMqttDeviceById;
using MediatR;
using Microsoft.Extensions.Configuration;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using System;
using System.Threading.Tasks;

namespace HomeHelper.Broker.Services.Publishing
{
    public class MqttPublishingService : IMqttPublishingService
    {
        private readonly IMediator _mediator;
        private readonly MqttClientSettings _clientSettings;
        private readonly Zigbee2mqttSettings _zigbeeSettings;
        private IMqttServer _mqttServer;
        public MqttPublishingService(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _clientSettings = new MqttClientSettings();
            _zigbeeSettings = new Zigbee2mqttSettings();
            config.GetSection(nameof(MqttClientSettings)).Bind(_clientSettings);
            config.GetSection(nameof(Zigbee2mqttSettings)).Bind(_zigbeeSettings);
        }
        public void ConfigureMqttServer(IMqttServer mqttServer)
        {
            _mqttServer = mqttServer;
        }

        public void ConfigureMqttServerOptions(AspNetMqttServerOptionsBuilder options)
        {
            options.WithApplicationMessageInterceptor(this);
        }

        public async Task InterceptApplicationMessagePublishAsync(MqttApplicationMessageInterceptorContext context)
        {

            if (context.ClientId == _clientSettings.Id || context.ClientId == _zigbeeSettings.Id)
            {
                context.AcceptPublish = true;
            }
            else
            {
                var mqttDevice = await _mediator.Send(new GetMqttDeviceByIdQuery() { Id = context.ClientId });

                if (!mqttDevice.IsConfirmed)
                {
                    context.AcceptPublish = false;
                }
                else
                {
                    context.AcceptPublish = true;
                }
            }
        }

        public Task InterceptClientMessageQueueEnqueueAsync(MqttClientMessageQueueInterceptorContext context)
        {
            throw new NotImplementedException();
        }
    }
}
