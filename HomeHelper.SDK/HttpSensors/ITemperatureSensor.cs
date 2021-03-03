using System.Threading.Tasks;

namespace HomeHelper.SDK.HttpSensors
{
    public interface ITemperatureSensor : IDevice
    {
        public Task<float> GetTemperatureValue(string ipAddress, int port);
        public Task<float> GetBatteryValue(string ipAddress, int port);
    }
}
