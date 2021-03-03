using HomeHelper.Common.Enums;
using HomeHelper.Domain.Base;

namespace HomeHelper.Domain
{
    public class Device : BaseEntityWithLog
    {
        public string Name { get; set; }
        public ProtocolType Protocol { get; set; }
        public DeviceType Type { get; set; }
        public string ModelName { get; set; }
        public string ConnectedDeviceId { get; set; }
        public string RoomId { get; set; }
    }
}
