using HomeHelper.Domain.Base;

namespace HomeHelper.Domain
{
    public class MqttDevice : BaseEntityWithLog
    {
        public bool IsConfirmed { get; set; }
        public bool IsAvailable { get; set; }
    }
}
