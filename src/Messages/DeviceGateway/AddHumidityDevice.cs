using NServiceBus;

namespace Messages.DeviceGateway
{
    public class AddHumidityDevice : ICommand
    {
        public string DeviceId { get; set; }
    }
}