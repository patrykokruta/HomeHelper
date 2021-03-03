using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.MqttDeviceRepo
{
    public interface IMqttDeviceRepository : IBaseRepository<MqttDevice>
    {
        void Update(MqttDevice mqttDevice);
    }
}
