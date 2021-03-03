using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Common.Mqtt
{
    public class MqttResponseWithLog : MqttResponse
    {
        public DateTime CreatedTime { get; set; }
    }
}
