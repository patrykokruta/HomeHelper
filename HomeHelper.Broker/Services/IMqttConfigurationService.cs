using MQTTnet.AspNetCore;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Broker.Services
{
    public interface IMqttConfigurationService
    {
        void ConfigureMqttServerOptions(AspNetMqttServerOptionsBuilder options);
        void ConfigureMqttServer(IMqttServer mqttServer);
    }
}
