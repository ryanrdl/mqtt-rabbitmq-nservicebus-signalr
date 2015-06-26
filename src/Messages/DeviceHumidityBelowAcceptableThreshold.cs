using NServiceBus;

namespace Messages
{
    public class DeviceHumidityBelowAcceptableThreshold : IEvent
    {
        public string DeviceId { get; set; }
        public int Value { get; set; }
        
    }
}