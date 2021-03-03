using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Common.Mqtt
{
    public class MqttResponse
    {
        public double? Battery { get; set; }
        public double? Humidity { get; set; }
        public double? Temperature { get; set; }
        public bool? Motion { get; set; }
        public bool? Contact { get; set; }
        public string State { get; set; }
        public int? Linkquality { get; set; }
    }
}
