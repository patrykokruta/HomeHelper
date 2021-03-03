using HomeHelper.Common.Enums;
using HomeHelper.Common.Mqtt;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace HomeHelper.Sonoff.Messages.Subscribe
{
    public class SonoffBasicZBR3_Base : IMqttSubscribeMessage
    {
        public IEnumerable<SubscribeType> SubscribeTypes => new List<SubscribeType>() { SubscribeType.Linkquality, SubscribeType.State };

        public string GetTopic(string id) => $"zigbee2mqtt/{id}";

        public MqttResponse GetValueFromPayload(string payload, SubscribeType subscribeType)
        {
            try
            {
                var body = (Body)JsonSerializer.Deserialize(payload, typeof(Body));

                if (subscribeType == SubscribeType.State)
                {
                    return new MqttResponse() { State = body.state };
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
        internal class Body
        {
            public string state { get; set; }
            public int? linkquality { get; set; }
        }
    }

}
