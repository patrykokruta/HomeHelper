using HomeHelper.Client.Hubs;
using HomeHelper.Common.Broker;
using HomeHelper.Common.Settings;
using HomeHelper.Common.Zigbee;
using HomeHelper.CQRS.MqttMessages.SubscribeMessages.Queries;
using HomeHelper.CQRS.Readings.Commands;
using HomeHelper.CQRS.ZigbeeDevices.Commands.CreateZigbeeDevice;
using HomeHelper.CQRS.ZigbeeDevices.Commands.UpdateZigbeeDevice;
using HomeHelper.CQRS.ZigbeeDevices.Queries.GetZigbeeDeviceById;
using HomeHelper.SDK;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace HomeHelper.Client.Services
{
    public class MqttClientService : IMqttClientService
    {
        private readonly IManagedMqttClient _mqttClient;
        private readonly IManagedMqttClientOptions _options;
        private readonly IHubContext<BrokerHub> _hubContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly BrokerEvents _brokerEvents;
        private readonly ZigbeeEvents _zigbeeEvents;
        private readonly MqttClientSettings _clientSettings;
        private readonly Zigbee2mqttSettings _zigbeeSettings;

        public MqttClientService(IManagedMqttClientOptions options, IHubContext<BrokerHub> hubContext,
            BrokerEvents brokerEvents, ZigbeeEvents zigbeeEvents, IServiceScopeFactory serviceScopeFactory,
            IConfiguration config)
        {
            _options = options;
            _hubContext = hubContext;
            _brokerEvents = brokerEvents;
            _zigbeeEvents = zigbeeEvents;
            _serviceScopeFactory = serviceScopeFactory;
            _clientSettings = new MqttClientSettings();
            _zigbeeSettings = new Zigbee2mqttSettings();
            config.GetSection(nameof(MqttClientSettings)).Bind(_clientSettings);
            config.GetSection(nameof(Zigbee2mqttSettings)).Bind(_zigbeeSettings);
            _mqttClient = new MqttFactory().CreateManagedMqttClient();
            ConfigureMqttClient();
        }
        private void ConfigureMqttClient()
        {
            _mqttClient.ConnectedHandler = this;
            _mqttClient.DisconnectedHandler = this;
            _mqttClient.ApplicationMessageReceivedHandler = this;
        }
        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var payload = eventArgs.ApplicationMessage.ConvertPayloadToString();

            if (eventArgs.ApplicationMessage.Topic == _zigbeeEvents.BridgeState)
            {
                if (payload.Contains("online"))
                {
                    await _hubContext.Clients.All.SendAsync("BridgeState", "online");
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("BridgeState", "offline");
                }
            }

            else if (eventArgs.ApplicationMessage.Topic == _zigbeeEvents.BridgeLog)
            {
                try
                {
                    var message = (BridgeLogBody)JsonSerializer.Deserialize(payload, typeof(BridgeLogBody),
                                        new JsonSerializerOptions() { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    if (message.Type == "pairing" && message.Message == "interview_successful")
                    {
                        var id = message.Meta.FriendlyName;
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetService<IMediator>();
                            var device = await mediator.Send(new GetZigbeeDeviceByIdQuery() { Id = id });
                            if (device != null)
                            {
                                await mediator.Send(
                                    new UpdateZigbeeDeviceCommand()
                                    { Id = id, IsAvailable = true, IsConfirmed = device.IsConfirmed });
                            }
                            else
                            {
                                await mediator.Send(new CreateZigbeeDeviceCommand() { Id = id, IsAvailable = true, IsConfirmed = false });
                            }
                            await _hubContext.Clients.All.SendAsync("DeviceConnected", id);
                        }
                    }
                    else if (message.Type == "device_removed" && message.Message == "left_network")
                    {
                        var id = message.Meta.FriendlyName;
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetService<IMediator>();
                            var device = await mediator.Send(new GetZigbeeDeviceByIdQuery() { Id = id });

                            if (device != null)
                            {
                                await mediator.Send(
                                   new UpdateZigbeeDeviceCommand()
                                   { Id = id, IsAvailable = false, IsConfirmed = device.IsConfirmed });

                                await mediator.Send(
                                        new UpdateZigbeeDeviceCommand()
                                        { Id = id, IsAvailable = false, IsConfirmed = device.IsConfirmed });
                                await _hubContext.Clients.All.SendAsync("DeviceDisconnected", id);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception inside bridgeLog message: {e.Message}");
                }

            }

            else if (eventArgs.ApplicationMessage.Topic == _zigbeeEvents.ConnectedDevices)
            {
                await _hubContext.Clients.All.SendAsync("AllZigbeeDevices", payload);
            }
            else if (eventArgs.ApplicationMessage.Topic == _brokerEvents.NewClientConnected)
            {
                await _hubContext.Clients.All.SendAsync("MqttConnected", payload);
            }
            else if (eventArgs.ApplicationMessage.Topic == _brokerEvents.NewClientDisconnected)
            {
                await _hubContext.Clients.All.SendAsync("MqttDisconnected", payload);
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetService<IMediator>();
                    var mqttDevices = scope.ServiceProvider.GetService<IEnumerable<IMqtt>>();

                    var messages = (await mediator.Send(new GetAllMqttSubscriptionMessagesQuery()));
                    foreach (var message in messages)
                    {
                        if (eventArgs.ApplicationMessage.Topic == message.Topic)
                        {
                            var mqttDevice = mqttDevices.FirstOrDefault(d => d.Name == message.ModelName);
                            var subscribeMessage = mqttDevice.SubscribeMessages.FirstOrDefault(m => m.GetTopic(message.ClientId) == message.Topic);
                            foreach (var subscribeType in subscribeMessage.SubscribeTypes)
                            {
                                var value = subscribeMessage.GetValueFromPayload(payload, subscribeType);
                                if (value != null)
                                {
                                    await _hubContext.Clients.All.SendAsync(subscribeType.ToString(), message.ClientId, value);
                                    await mediator.Send(new AddReadingCommand() { Data = value, Type = subscribeType, ConnectedDeviceId = message.ClientId });
                                }
                            }
                        }
                    }
                }
            }
        }

        public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine("Connected");
            return Task.FromResult("");

        }

        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            Console.WriteLine("Disconnected");
            return Task.FromResult("");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _mqttClient.StartAsync(_options);

            await _mqttClient.SubscribeAsync(_zigbeeEvents.BridgeState);
            await _mqttClient.SubscribeAsync(_zigbeeEvents.BridgeLog);
            await _mqttClient.SubscribeAsync(_zigbeeEvents.ConnectedDevices);
            await _mqttClient.SubscribeAsync(_brokerEvents.NewClientConnected);
            await _mqttClient.SubscribeAsync(_brokerEvents.NewClientDisconnected);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                var messages = await mediator.Send(new GetAllMqttSubscriptionMessagesQuery());
                if (messages != null)
                {
                    foreach (var message in messages)
                    {
                        await _mqttClient.SubscribeAsync(message.Topic);
                    }
                }

            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _mqttClient.StopAsync();
        }

        public async Task SubscribeAsync(string topic)
        {
            await _mqttClient.SubscribeAsync(topic);
        }

        public async Task PublishAsync(string topic, string payload)
        {
            var applicationMessage = new MqttApplicationMessageBuilder().WithTopic(topic)
                                                                        .WithPayload(payload)
                                                                        .Build();
            var manegedApplicationMessage = new ManagedMqttApplicationMessageBuilder().WithApplicationMessage(applicationMessage)
                                                                                      .Build();
            await _mqttClient.PublishAsync(manegedApplicationMessage);
        }
    }
    public class BridgeLogBody
    {
        public string Message { get; set; }
        public Meta Meta { get; set; }
        public string Type { get; set; }
    }
    public class Meta
    {
        public string Description { get; set; }

        [JsonPropertyName("friendly_name")]
        public string FriendlyName { get; set; }
        public string Model { get; set; }
        public bool Supported { get; set; }
        public string Vendor { get; set; }
    }
}
