using HomeHelper.Client.Services;
using HomeHelper.Common.Broker;
using HomeHelper.Common.Zigbee;
using HomeHelper.CQRS.MqttMessages.SubscribeMessages.Queries;
using HomeHelper.Repositories;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeHelper.Client.Hubs
{
    public class BrokerHub : Hub
    {
        private readonly IMqttClientService _mqttClientService;
        private readonly BrokerCommands _brokerCommands;
        private readonly ZigbeeCommands _zigbeeCommands;
        private readonly IMediator _mediator;

        public BrokerHub(MqttClientServiceProvider provider, BrokerCommands brokerCommands, ZigbeeCommands zigbeeCommands, IMediator mediator)
        {
            _mqttClientService = provider.MqttClientService;
            _brokerCommands = brokerCommands;
            _zigbeeCommands = zigbeeCommands;
            _mediator = mediator;
        }

        public async Task PublishAsync(string topic, string payload)
        {
            await _mqttClientService.PublishAsync(topic, payload);
        }

        public async Task Subscribe(string topic)
        {
            await _mqttClientService.SubscribeAsync(topic);
        }

        public async Task SubscribeClientTopics(string clientId)
        {
            var messages = (await _mediator.Send(new GetAllMqttSubscriptionMessagesQuery())).Where(m => m.ClientId == clientId);
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    await _mqttClientService.SubscribeAsync(message.Topic);
                }
            }
        }
        public async Task GetAllZigbeeDevices()
        {
            await _mqttClientService.PublishAsync(_zigbeeCommands.GetAllConnected, "");
        }

        public async Task PermitJoinZigbeeDevices()
        {
            //var payload = JsonSerializer.Serialize(new ZigbeePayload() { PermitJoin = true },
            //    new JsonSerializerOptions() { IgnoreNullValues = true });
            await _mqttClientService.PublishAsync(_zigbeeCommands.PermitJoin, "true");
        }
        public async Task ForbidJoinZigbeeDevices()
        {
            //var payload = JsonSerializer.Serialize(new ZigbeePayload() { PermitJoin = false },
            //    new JsonSerializerOptions() { IgnoreNullValues = true });
            await _mqttClientService.PublishAsync(_zigbeeCommands.PermitJoin, "false");
        }
        public async Task RemoveZigbeeDevice(string id)
        {
            var payload = JsonSerializer.Serialize(new ZigbeePayload() { FriendlyName = id },
                new JsonSerializerOptions() { IgnoreNullValues = true });
            await _mqttClientService.PublishAsync(_zigbeeCommands.RemoveDevice, payload);
        }

    }

    internal class ZigbeePayload
    {
        [JsonPropertyName("payload_on")]
        public bool PermitJoin { get; set; }

        [JsonPropertyName("friendly_name")]
        public string FriendlyName { get; set; }
    }
}
