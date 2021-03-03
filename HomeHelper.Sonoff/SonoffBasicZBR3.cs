using HomeHelper.Common.Enums;
using HomeHelper.Common.Mqtt;
using HomeHelper.SDK;
using HomeHelper.Sonoff.Messages.Publish;
using HomeHelper.Sonoff.Messages.Subscribe;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace HomeHelper.Sonoff
{
    public class SonoffBasicZBR3 : IMqtt
    {
        public string Name => "SonoffBasicZBR3";

        public ProtocolType Protocol => ProtocolType.Zigbee;

        public DeviceType Type => DeviceType.Switch;


        public IEnumerable<IMqttPublishMessage> PublishMessages =>
            new List<IMqttPublishMessage>() { new SonoffBasicZBR3_TurnOn(), new SonoffBasicZBR3_TurnOff(), new SonoffBasicZBR3_CheckState() };

        public IEnumerable<IMqttSubscribeMessage> SubscribeMessages => new List<IMqttSubscribeMessage>() { new SonoffBasicZBR3_Base() };

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDevice, SonoffBasicZBR3>();
            services.AddScoped<IMqtt, SonoffBasicZBR3>();
        }
    }
}
