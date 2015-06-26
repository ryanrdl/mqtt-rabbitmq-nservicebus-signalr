using NServiceBus;

namespace Messages
{
    public class DeviceTemperatureBelowAcceptableThreshold : IEvent
    {
        public string DeviceId { get; set; }
        public int Value { get; set; }
        
    }
}