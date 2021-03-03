using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.MqttPublishingMessageRepo
{
    public interface IMqttPublishingMessageRepository : IBaseRepository<MqttPublishingMessage>
    {
        public void Update(MqttPublishingMessage mqttPublishMessage);
    }
}
