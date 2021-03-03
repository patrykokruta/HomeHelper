using HomeHelper.Common.Enums;

namespace HomeHelper.Common.Mqtt
{
    public interface IMqttPublishMessage
    {
        public PublishType PublishType { get; }
        public string GetTopic(string id);
        public string GetPayload(params object[] args);
    }
}
