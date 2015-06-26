using NServiceBus;

namespace Messages
{
    public class AveragedHumidityUpdated : IEvent
    {
        public string DeviceId { get; set; }

        public double Average { get; set; }
    }
}