using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Broker.Services.Publishing
{
    public interface IMqttPublishingService : IMqttServerApplicationMessageInterceptor,
                                              IMqttServerClientMessageQueueInterceptor,
                                              IMqttConfigurationService
    {
    }
}
