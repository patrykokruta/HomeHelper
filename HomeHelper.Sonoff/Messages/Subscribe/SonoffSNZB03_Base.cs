using HomeHelper.Common.Enums;
using HomeHelper.Common.Mqtt;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace HomeHelper.Sonoff.Messages.Subscribe
{
    public class SonoffSNZB03_Base : IMqttSubscribeMessage
    {
        public IEnumerable<SubscribeType> SubscribeTypes =>
            new List<SubscribeType>() { SubscribeType.Motion, SubscribeType.Battery, SubscribeType.Linkquality };

        public string GetTopic(string id) => $"zigbee2mqtt/{id}";

        public MqttResponse GetValueFromPayload(string payload, SubscribeType subscribeType)
        {
            try
            {
                var body = (Body)JsonSerializer.Deserialize(payload, typeof(Body));

                if (subscribeType == SubscribeType.Motion)
                {
                    return new MqttResponse() { Motion = body.occupancy };
                }
                else if (subscribeType == SubscribeType.Battery)
                {
                    return new MqttResponse() { Battery = body.battery };
                }
                else if (subscribeType == SubscribeType.Linkquality)
                {
                    return new MqttResponse() { Linkquality = body.linkquality };
                }
                else
                {
                    throw new Exception("Not known subscribeType for this payload.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception inside {GetType()} : { e.Message }");
                return new MqttResponse();
            }

        }
        public class Body
        {
            public bool? occupancy { get; set; }
            public double? battery { get; set; }
            public int? linkquality { get; set; }
        }
    }
}
