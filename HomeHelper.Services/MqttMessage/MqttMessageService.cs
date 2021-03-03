using HomeHelper.CQRS.MqttMessages.PublishingMessages.Commands;
using HomeHelper.CQRS.MqttMessages.SubscriptionMessages.Commands;
using HomeHelper.SDK;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHelper.Services.MqttMessage
{
    public class MqttMessageService : IMqttMessageService
    {
        private readonly IMediator _mediator;
        private readonly IEnumerable<IMqtt> _mqttDevices;

        public MqttMessageService(IMediator mediator, IEnumerable<IMqtt> mqttDevices)
        {
            _mediator = mediator;
            _mqttDevices = mqttDevices;
        }
        public async Task RegisterPublishMessages(string clientId, string modelName)
        {
            var mqttDevice = _mqttDevices.FirstOrDefault(d => d.Name == modelName);
            if (mqttDevice.PublishMessages != null)
            {
                foreach (var message in mqttDevice.PublishMessages)
                {
                    await _mediator.Send(
                        new CreateMqttPublishingMessageCommand()
                        { ClientId = clientId, ModelName = modelName, Topic = message.GetTopic(clientId), PublishType = message.PublishType });
                }

            }
        }

        public async Task RegisterSubscribeMessages(string clientId, string modelName)
        {
            var mqttDevice = _mqttDevices.FirstOrDefault(d => d.Name == modelName);
            if (mqttDevice.SubscribeMessages != null)
            {
                foreach (var message in mqttDevice.SubscribeMessages)
                {
                    await _mediator.Send(
                        new CreateMqttSubscriptionMessageCommand() { ClientId = clientId, ModelName = modelName, Topic = message.GetTopic(clientId) });
                }
            }

        }
    }
}
