using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.DeviceRepo
{
    public interface IDeviceRepository : IBaseRepository<Device>
    {
        void Update(Device device);
    }
}
