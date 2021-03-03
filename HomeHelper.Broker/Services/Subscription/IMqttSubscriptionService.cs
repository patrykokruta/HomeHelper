using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Broker.Services.Subscription
{
    public interface IMqttSubscriptionService : IMqttServerClientSubscribedTopicHandler,
                                                IMqttServerSubscriptionInterceptor,
                                                IMqttServerClientUnsubscribedTopicHandler,
                                                IMqttServerUnsubscriptionInterceptor,
                                                IMqttConfigurationService
    {
    }
}
