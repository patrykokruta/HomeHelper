using HomeHelper.Domain.Base;

namespace HomeHelper.Domain
{
    public class MqttSubscriptionMessage : BaseEntity
    {
        public string Topic { get; set; }
        public string ClientId { get; set; }
        public string ModelName { get; set; }
    }
}
