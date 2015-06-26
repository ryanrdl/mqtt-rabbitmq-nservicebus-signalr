using System;
using NServiceBus;

namespace Messages
{
    public class HumidityDeviceAdded : IEvent
    {
        public string DeviceId { get; set; }
    }
}