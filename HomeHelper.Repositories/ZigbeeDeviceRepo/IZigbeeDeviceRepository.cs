using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.ZigbeeDeviceRepo
{
    public interface IZigbeeDeviceRepository : IBaseRepository<ZigbeeDevice>
    {
        void Update(ZigbeeDevice zigbeeDevice);
    }
}
