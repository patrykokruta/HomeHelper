using HomeHelper.Domain.Base;

namespace HomeHelper.Domain
{
    public class ZigbeeDevice : BaseEntityWithLog
    {
        public bool IsConfirmed { get; set; }
        public bool IsAvailable { get; set; }
    }
}
