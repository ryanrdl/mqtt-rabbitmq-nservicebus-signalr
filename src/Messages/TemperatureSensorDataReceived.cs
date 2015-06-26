using System;
using NServiceBus;

namespace Messages
{
    public class TemperatureSensorDataReceived : IEvent
    {  
        public TemperatureSensorDataReceived()
        { 
        }

        public TemperatureSensorDataReceived(SensorData data)
        {
            this.Id = data.Id;
            this.Value = data.Value;
            this.Timestamp = data.Timestamp;
            this.DeviceId = data.DeviceId;
        }
        public Guid Id { get; set; }
        public string DeviceId { get; set; }
        
        public string Value { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
