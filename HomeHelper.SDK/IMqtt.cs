using HomeHelper.Common.Mqtt;
using System.Collections.Generic;

namespace HomeHelper.SDK
{
    public interface IMqtt : IDevice
    {
        public IEnumerable<IMqttSubscribeMessage> SubscribeMessages { get; }
        public IEnumerable<IMqttPublishMessage> PublishMessages { get; }
    }
}
