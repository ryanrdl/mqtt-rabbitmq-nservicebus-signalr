using NServiceBus;

namespace Messages
{
    public class TemperatureDeviceAdded : IEvent
    {
        public string DeviceId { get; set; }
    }
}