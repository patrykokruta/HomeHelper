using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.SDK.HttpSensors
{
    public interface IHumiditySensor : IDevice
    {
        public Task<float> GetHumidityValue(string ipAddress, int port);
        public Task<float> GetBatteryValue(string ipAddress, int port);
    }
}
