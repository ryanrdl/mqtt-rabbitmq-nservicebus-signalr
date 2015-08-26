using System;
using Messages;
using NServiceBus;

namespace DeviceGateway
{
    public class SensorHandler : 
        IHandleMessages<ProximitySensorDataReceived>,
        IHandleMessages<TemperatureSensorDataReceived>,
        IHandleMessages<HumiditySensorDataReceived>
    {
        public void Handle(ProximitySensorDataReceived message)
        {
            throw new NotImplementedException();
        }

        public void Handle(TemperatureSensorDataReceived message)
        {
            throw new NotImplementedException();
        }

        public void Handle(HumiditySensorDataReceived message)
        {
            throw new NotImplementedException();
        }
    }
}
