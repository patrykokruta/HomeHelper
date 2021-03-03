using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Client.Services
{
    public class MqttClientServiceProvider
    {
        public IMqttClientService MqttClientService { get; set; }

        public MqttClientServiceProvider(IMqttClientService mqttClientService)
        {
            MqttClientService = mqttClientService;
        }
    }
}
