using HomeHelper.Common.Enums;
using HomeHelper.Common.Mqtt;
using HomeHelper.SDK;
using HomeHelper.Sonoff.Messages.Subscribe;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace HomeHelper.Sonoff
{
    public class SonoffSNZB02 : IMqtt
    {
        public string Name => "SonoffSNZB02";

        public ProtocolType Protocol => ProtocolType.Zigbee;

        public DeviceType Type => DeviceType.TemperatureAndHumiditySensor;

        public IEnumerable<IMqttSubscribeMessage> SubscribeMessages => new List<IMqttSubscribeMessage>() { new SonoffSNZB02_Base() };

        public IEnumerable<IMqttPublishMessage> PublishMessages => null;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDevice, SonoffSNZB02>();
            services.AddScoped<IMqtt, SonoffSNZB02>();
        }
    }
}
