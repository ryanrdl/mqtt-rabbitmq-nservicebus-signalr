using NServiceBus;

namespace Messages
{
    public class ProximityDeviceStarted : IEvent
    {
        public string DeviceId { get; set; }
    }
}