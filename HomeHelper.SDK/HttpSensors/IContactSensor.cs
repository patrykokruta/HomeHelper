using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.SDK.HttpSensors
{
    public interface IContactSensor : IDevice
    {
        public Task<bool> IsContact(string ipAddress, int port);
        public Task<float> GetBatteryValue(string ipAddress, int port);
    }
}
