using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Common.Zigbee
{
    public class ZigbeeCommands
    {
        public static string Base { get; } = "zigbee2mqtt";
        public static string BridgeBase { get; } = $"{Base}/bridge";
        public string GetAllConnected { get; } = $"{BridgeBase}/config/devices/get";
        public string PermitJoin { get; } = $"{BridgeBase}/config/permit_join";
        public string RemoveDevice { get; } = $"{BridgeBase}/config/remove";

    }
}
