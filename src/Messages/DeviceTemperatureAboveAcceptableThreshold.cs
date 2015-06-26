using NServiceBus;

namespace Messages
{
    public class DeviceTemperatureAboveAcceptableThreshold : IEvent
    {
        public string DeviceId { get; set; }
        public int Value { get; set; }
    }
}