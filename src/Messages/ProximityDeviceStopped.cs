using NServiceBus;

namespace Messages
{
    public class ProximityDeviceStopped : IEvent
    {
        public string DeviceId { get; set; }
    }
}