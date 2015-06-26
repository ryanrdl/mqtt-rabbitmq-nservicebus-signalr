using NServiceBus;

namespace Messages
{
    public class AveragedTemperatureUpdated : IEvent
    {
        public string DeviceId { get; set; }
        
        public double Average { get; set; }
    }
}