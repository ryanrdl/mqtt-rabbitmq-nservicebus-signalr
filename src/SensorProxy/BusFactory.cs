using NServiceBus;

namespace SensorProxy
{
    public static class BusFactory
    {
        public static IBus GetBus()
        {
            var configuration = new BusConfiguration();
            configuration.UseTransport<RabbitMQTransport>();
            configuration.UsePersistence<InMemoryPersistence>();  

            var bus = Bus.Create(configuration).Start(); 
            return bus;
        }
    }
}