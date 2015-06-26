using System;
using Messages;
using NServiceBus;

namespace Simulator
{
    public class DeviceMessageHandler :
        IHandleMessages<TemperatureDeviceAdded>,
        IHandleMessages<TemperatureDeviceRemoved>,
        IHandleMessages<HumidityDeviceAdded>,
        IHandleMessages<HumidityDeviceRemoved>
    {
        private string _;

        public void Handle(TemperatureDeviceAdded message)
        {
            if (!Program.TempSensorDeviceIds.ContainsKey(message.DeviceId))
            {
                Console.WriteLine("Adding temperature device with id {0}", message.DeviceId);
                Program.TempSensorDeviceIds.TryAdd(message.DeviceId, message.DeviceId);
            }
        }

        public void Handle(TemperatureDeviceRemoved message)
        {
            if (Program.TempSensorDeviceIds.ContainsKey(message.DeviceId))
            {
                Console.WriteLine("Removing temperature device with id {0}", message.DeviceId);
                Program.TempSensorDeviceIds.TryRemove(message.DeviceId, out _);
            }
        }

        public void Handle(HumidityDeviceAdded message)
        {
            if (!Program.HumiditySensorDeviceIds.ContainsKey(message.DeviceId))
            {
                Console.WriteLine("Adding humidity device with id {0}", message.DeviceId);
                Program.HumiditySensorDeviceIds.TryAdd(message.DeviceId, message.DeviceId);
            }
        }

        public void Handle(HumidityDeviceRemoved message)
        {
            if (Program.HumiditySensorDeviceIds.ContainsKey(message.DeviceId))
            {
                Console.WriteLine("Removing humidity device with id {0}", message.DeviceId);
                Program.HumiditySensorDeviceIds.TryRemove(message.DeviceId, out _);
            }
        }
    }
}