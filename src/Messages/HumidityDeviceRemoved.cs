using System;
using NServiceBus;

namespace Messages
{
    public class HumidityDeviceRemoved : IEvent
    {
        public string DeviceId { get; set; }
        
    }
}