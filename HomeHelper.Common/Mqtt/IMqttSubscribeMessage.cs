using HomeHelper.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Common.Mqtt
{
    public interface IMqttSubscribeMessage
    {
        public IEnumerable<SubscribeType> SubscribeTypes { get; }
        public string GetTopic(string id);
        public MqttResponse GetValueFromPayload(string payload, SubscribeType subscribeType);
    }
}
