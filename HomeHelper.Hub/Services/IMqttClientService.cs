using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Client.Services
{
    public interface IMqttClientService : IHostedService,
                                          IMqttClientConnectedHandler,
                                          IMqttClientDisconnectedHandler,
                                          IMqttApplicationMessageReceivedHandler
    {
        public Task SubscribeAsync(string topic);
        Task PublishAsync(string topic, string payload);
    }
}
