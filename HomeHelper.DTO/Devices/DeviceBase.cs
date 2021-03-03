using HomeHelper.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.DTO.Devices
{
    public class DeviceBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RoomName { get; set; }
        public ProtocolType Protocol { get; set; }
    }
}
