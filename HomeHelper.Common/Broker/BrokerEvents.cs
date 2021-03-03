using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Common.Broker
{
    public class BrokerEvents
    {
        private static string ClientBase { get; } = "clients";
        public string NewClientConnected { get; } = $"{ClientBase}/connected/new";
        public string NewClientDisconnected { get; } = $"{ClientBase}/disconnected/new";
    }
}
