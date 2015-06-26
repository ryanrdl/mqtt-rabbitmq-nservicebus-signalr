using System;
using NServiceBus;

namespace Messages
{
    public class TemperatureDeviceRemoved : IEvent
    {
        public string DeviceId { get; set; }
        
    }
}