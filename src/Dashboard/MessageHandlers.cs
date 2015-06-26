using System;
using System.Linq;
using Messages;
using Microsoft.AspNet.SignalR;
using NServiceBus;

namespace Dashboard
{
    public class MessageHandlers :
        IHandleMessages<AveragedTemperatureUpdated>,
        IHandleMessages<DeviceTemperatureAboveAcceptableThreshold>,
        IHandleMessages<DeviceTemperatureBelowAcceptableThreshold>,
        IHandleMessages<AveragedHumidityUpdated>,
        IHandleMessages<DeviceHumidityAboveAcceptableThreshold>,
        IHandleMessages<DeviceHumidityBelowAcceptableThreshold>,
        
        IHandleMessages<TemperatureDeviceAdded>,
        IHandleMessages<HumidityDeviceAdded>,
        IHandleMessages<TemperatureDeviceRemoved>,
        IHandleMessages<HumidityDeviceRemoved>
    {
        private readonly IHubContext _context;

        public MessageHandlers()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<DeviceHub>();
        }

        public void Handle(AveragedTemperatureUpdated message)
        {
            _context.Clients.All.temperatureChanged(
                new
                {
                    message.Average,
                    DeviceId = (ShortDeviceId(message.DeviceId))
                });
            Console.WriteLine("AveragedTemperatureUpdated for device id {0}", message.DeviceId);
        }

        public void Handle(AveragedHumidityUpdated message)
        {
            _context.Clients.All.humidityChanged(
                new
                {
                    message.Average,
                    DeviceId = (ShortDeviceId(message.DeviceId))
                });
            Console.WriteLine("AveragedHumidityUpdated for device id {0}", message.DeviceId);
        }

        public void Handle(DeviceTemperatureAboveAcceptableThreshold message)
        {
            _context.Clients.All.temperatureAboveThreshold(ShortDeviceId(message.DeviceId));
            Console.WriteLine("DeviceTemperatureAboveAcceptableThreshold for device id {0}", message.DeviceId);
        }

        public void Handle(DeviceTemperatureBelowAcceptableThreshold message)
        {
            _context.Clients.All.temperatureBelowThreshold(ShortDeviceId(message.DeviceId));
            Console.WriteLine("DeviceTemperatureBelowAcceptableThreshold for device id {0}", message.DeviceId);
        }

        public void Handle(DeviceHumidityAboveAcceptableThreshold message)
        {
            _context.Clients.All.humidityAboveThreshold(ShortDeviceId(message.DeviceId));
            Console.WriteLine("DeviceHumidityAboveAcceptableThreshold for device id {0}", message.DeviceId);
        }

        public void Handle(DeviceHumidityBelowAcceptableThreshold message)
        {
            _context.Clients.All.humidityBelowThreshold(ShortDeviceId(message.DeviceId));
            Console.WriteLine("DeviceHumidityBelowAcceptableThreshold for device id {0}", message.DeviceId);
        }

        public void Handle(TemperatureDeviceAdded message)
        {
            _context.Clients.All.addTemperatureDevice(ShortDeviceId(message.DeviceId));
            Console.WriteLine("TemperatureDeviceAdded for device id {0}", message.DeviceId);
        }

        public void Handle(HumidityDeviceAdded message)
        {
            _context.Clients.All.addHumidityDevice(ShortDeviceId(message.DeviceId));
            Console.WriteLine("HumidityDeviceAdded for device id {0}", message.DeviceId);
        }

        public void Handle(TemperatureDeviceRemoved message)
        {
            _context.Clients.All.removeTemperatureDevice(ShortDeviceId(message.DeviceId));
            Console.WriteLine("TemperatureDeviceRemoved for device id {0}", message.DeviceId);
        }

        public void Handle(HumidityDeviceRemoved message)
        {
            _context.Clients.All.removeHumidityDevice(ShortDeviceId(message.DeviceId));
            Console.WriteLine("HumidityDeviceRemoved for device id {0}", message.DeviceId);
        }

        private string ShortDeviceId(string id)
        {
            return id.Split('-').First();
        }
    }
}