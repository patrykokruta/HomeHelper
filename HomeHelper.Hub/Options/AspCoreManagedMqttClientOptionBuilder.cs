using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Client.Options
{
    public class AspCoreManagedMqttClientOptionBuilder : ManagedMqttClientOptionsBuilder
    {
        public IServiceProvider ServiceProvider { get; }

        public AspCoreManagedMqttClientOptionBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
