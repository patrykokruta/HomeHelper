using HomeHelper.Domain;
using HomeHelper.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Repositories.MqttSubscriptionMessageRepo
{
    public interface IMqttSubscriptionMessageRepository : IBaseRepository<MqttSubscriptionMessage>
    {
        public void Update(MqttSubscriptionMessage mqttSubscribeMessage);
    }
}
