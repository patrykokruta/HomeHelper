using HomeHelper.Common.Enums;
using HomeHelper.Common.Mqtt;
using HomeHelper.SDK;
using HomeHelper.Sonoff.Messages.Subscribe;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Sonoff
{
    public class SonoffSNZB04 : IMqtt
    {
        public IEnumerable<IMqttSubscribeMessage> SubscribeMessages => new List<IMqttSubscribeMessage>() { new SonoffSNZB04_Base() };

        public IEnumerable<IMqttPublishMessage> PublishMessages => null;

        public string Name => "SonoffSNZB04";

        public ProtocolType Protocol => ProtocolType.Zigbee;

        public DeviceType Type => DeviceType.ContactSensor;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDevice, SonoffSNZB04>();
            services.AddScoped<IMqtt, SonoffSNZB04>();
        }
    }
}
