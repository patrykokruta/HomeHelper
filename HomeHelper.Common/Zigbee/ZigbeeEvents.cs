using System;
using System.Collections.Generic;
using System.Text;

namespace HomeHelper.Common.Zigbee
{
    public class ZigbeeEvents
    {
        public static string Base { get; } = "zigbee2mqtt";
        public static string BridgeBase { get; } = $"{Base}/bridge";
        public string BridgeState { get; } = $"{BridgeBase}/state";
        public string BridgeLog { get; } = $"{BridgeBase}/log";
        public string ConnectedDevices { get; } = $"{BridgeBase}/config/devices";
    }
}
