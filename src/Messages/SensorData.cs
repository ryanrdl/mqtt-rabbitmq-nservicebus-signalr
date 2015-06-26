using System;

namespace Messages
{
    public enum SensorType
    {
        Temperature = 100,
        Humidity = 200
    }

    public class SensorData
    {
        public SensorData()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTimeOffset.Now;
        }

        public Guid Id { get; set; }
        public string Value { get; set; }
        public string DeviceId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public SensorType Type { get; set; }
    }
}
