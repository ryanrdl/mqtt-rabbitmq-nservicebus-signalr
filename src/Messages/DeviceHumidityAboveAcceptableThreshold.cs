using NServiceBus;

namespace Messages
{
    public class DeviceHumidityAboveAcceptableThreshold : IEvent
    {
        public string DeviceId { get; set; }
        public int Value { get; set; }
    }
}