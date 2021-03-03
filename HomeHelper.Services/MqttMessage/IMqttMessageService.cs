using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Services.MqttMessage
{
    public interface IMqttMessageService
    {
        public Task RegisterPublishMessages(string clientId, string modelName);
        public Task RegisterSubscribeMessages(string clientId, string modelName);
    }
}
