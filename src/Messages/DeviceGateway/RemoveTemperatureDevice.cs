using NServiceBus;

namespace Messages.DeviceGateway
{
    public class RemoveTemperatureDevice : ICommand
    {
        public string DeviceId { get; set; }
        
    }
}