using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.SDK.HttpSensors
{
    public interface IMotionSensor : IDevice
    {
        public Task<bool> IsMotion(string ipAddress, int port);
        public Task<float> GetBatteryValue(string ipAddress, int port);
    }
}
