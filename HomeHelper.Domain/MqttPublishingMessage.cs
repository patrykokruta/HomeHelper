using HomeHelper.Common.Enums;
using HomeHelper.Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Domain
{
    public class MqttPublishingMessage : BaseEntity
    {
        public string Topic { get; set; }
        public string ClientId { get; set; }
        public string ModelName { get; set; }
        public PublishType PublishType { get; set; }
    }
}
