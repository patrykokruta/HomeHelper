using HomeHelper.Common.Enums;
using System.Threading.Tasks;

namespace HomeHelper.SDK.HttpControls
{
    public interface ISwitch : IDevice
    {
        Task<bool> TurnOn(string ip, int port);
        Task<bool> TurnOff(string ip, int port);
        Task<int?> SignalStrength(string ip, int port);
        Task<bool?> IsOn(string ip, int port);
    }
}
