using HomeHelper.Domain.Base;

namespace HomeHelper.Domain
{
    public class HttpDevice : BaseEntityWithLog
    {
        public string IpAddress { get; set; }
        public int PortNumber { get; set; }

    }
}
