using NServiceBus;

namespace Messages.DeviceGateway
{
    public class RemoveHumidityDevice : ICommand
    {
        public string DeviceId { get; set; }
        
    }
}