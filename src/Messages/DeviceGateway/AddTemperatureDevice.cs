using NServiceBus;

namespace Messages.DeviceGateway
{
    public class AddTemperatureDevice : ICommand
    {
        public string DeviceId { get; set; }
        
    }
}
