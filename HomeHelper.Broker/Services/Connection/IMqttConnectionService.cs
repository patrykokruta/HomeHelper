using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Broker.Services.Connection
{
    public interface IMqttConnectionService : IMqttServerConnectionValidator,
                                              IMqttServerClientConnectedHandler,
                                              IMqttServerClientDisconnectedHandler,
                                              IMqttConfigurationService
    {
    }
}
