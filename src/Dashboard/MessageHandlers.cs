using Messages;
using Microsoft.AspNet.SignalR;
using NServiceBus;

namespace Dashboard
{
    public class MessageHandlers :
        IHandleMessages<ProximitySensorDataReceived>,
        IHandleMessages<TemperatureSensorDataReceived>,
        IHandleMessages<HumiditySensorDataReceived>
    {
        private readonly IHubContext _context;

        public MessageHandlers()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<DeviceHub>();
        } 

        public void Handle(ProximitySensorDataReceived message)
        {
            _context.Clients.All.proximitySensorDataReceived(message);
        }

        public void Handle(TemperatureSensorDataReceived message)
        {
            _context.Clients.All.temperatureSensorDataReceived(message);
        }

        public void Handle(HumiditySensorDataReceived message)
        {
            _context.Clients.All.humiditySensorDataReceived(message);
        }
    }
}