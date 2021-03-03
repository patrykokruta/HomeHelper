using HomeHelper.Common.Enums;
using Microsoft.Extensions.DependencyInjection;


namespace HomeHelper.SDK
{
    public interface IDevice
    {
        string Name { get; }
        ProtocolType Protocol { get; }
        DeviceType Type { get; }
        void ConfigureServices(IServiceCollection services);
    }
}
