using HomeHelper.Common.Broker;
using HomeHelper.Common.Settings;
using HomeHelper.CQRS.MqttDevices.Commands.CreateMqttDevice;
using HomeHelper.CQRS.MqttDevices.Commands.UpdateMqttDevice;
using HomeHelper.CQRS.MqttDevices.Queries.GetMqttDeviceById;
using HomeHelper.DTO.Authentication;
using HomeHelper.Services.Account;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using System.Threading.Tasks;

namespace HomeHelper.Broker.Services.Connection
{
    public class MqttConnectionService : IMqttConnectionService
    {

        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly BrokerEvents _brokerEvents;
        private readonly MqttClientSettings _clientSettings;
        private readonly Zigbee2mqttSettings _zigbeeSettings;
        private IMqttServer _mqttServer;
        public MqttConnectionService(IConfiguration config, IServiceScopeFactory serviceScopeFactory, BrokerEvents brokerEvents)
        {

            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
            _brokerEvents = brokerEvents;
            _clientSettings = new MqttClientSettings();
            _zigbeeSettings = new Zigbee2mqttSettings();
            config.GetSection(nameof(MqttClientSettings)).Bind(_clientSettings);
            config.GetSection(nameof(Zigbee2mqttSettings)).Bind(_zigbeeSettings);
        }
        public void ConfigureMqttServer(IMqttServer mqttServer)
        {
            _mqttServer = mqttServer;
            mqttServer.ClientConnectedHandler = this;
            mqttServer.ClientDisconnectedHandler = this;
        }

        public void ConfigureMqttServerOptions(AspNetMqttServerOptionsBuilder options)
        {
            options.WithConnectionValidator(this);
        }

        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            System.Console.WriteLine($"Connected: {eventArgs.ClientId}");
            var zigbeeClientId = _zigbeeSettings.Id;
            var hubClientId = _clientSettings.Id;

            if (eventArgs.ClientId != zigbeeClientId && eventArgs.ClientId != hubClientId)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetService<IMediator>();
                    var device = await mediator.Send(new GetMqttDeviceByIdQuery() { Id = eventArgs.ClientId });
                    if (device != null)
                    {
                        var result = await mediator.Send(
                            new UpdateMqttDeviceCommand() { Id = device.Id, isAvailable = true, isConfirmed = device.IsConfirmed });
                    }

                    else
                    {
                        var result = await mediator.Send(
                            new CreateMqttDeviceCommand() { Id = eventArgs.ClientId, isAvailable = true, isConfirmed = false });
                        await _mqttServer.PublishAsync(_brokerEvents.NewClientConnected, eventArgs.ClientId);
                    }
                }
            }
        }

        public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            System.Console.WriteLine($"Disconnected: {eventArgs.ClientId}");
            var zigbeeClientId = _zigbeeSettings.Id;
            var hubClientId = _clientSettings.Id;

            if (eventArgs.ClientId != zigbeeClientId && eventArgs.ClientId != hubClientId)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetService<IMediator>();
                    var device = await mediator.Send(new GetMqttDeviceByIdQuery() { Id = eventArgs.ClientId });

                    await mediator.Send(
                        new UpdateMqttDeviceCommand() { Id = device.Id, isAvailable = false, isConfirmed = device.IsConfirmed });

                    await _mqttServer.PublishAsync(_brokerEvents.NewClientDisconnected, eventArgs.ClientId);
                }
            }
        }

        public Task ValidateConnectionAsync(MqttConnectionValidatorContext context)
        {
            if (context.Username != _clientSettings.UserName || context.Password != _clientSettings.Password)
            {
                return Task.FromResult(context.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword);
            }
            return Task.FromResult(context.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.Success);
        }
    }
}
