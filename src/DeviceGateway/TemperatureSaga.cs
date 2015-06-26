using System;
using System.Linq;
using Messages;
using Messages.DeviceGateway;
using NServiceBus;
using NServiceBus.Saga;

namespace DeviceGateway
{
    public class TemperatureSaga : Saga<TemperatureSagaData>,
        IAmStartedByMessages<AddTemperatureDevice>,
        IHandleMessages<TemperatureSensorDataReceived>,
        IHandleMessages<RemoveTemperatureDevice>

    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TemperatureSagaData> mapper)
        {
            mapper.ConfigureMapping<AddTemperatureDevice>(received => received.DeviceId)
                .ToSaga(o => o.DeviceId);
            mapper.ConfigureMapping<TemperatureSensorDataReceived>(received => received.DeviceId)
                .ToSaga(o => o.DeviceId);
            mapper.ConfigureMapping<RemoveTemperatureDevice>(received => received.DeviceId)
                .ToSaga(o => o.DeviceId);
        }

        public void Handle(AddTemperatureDevice message)
        {
            Data.DeviceId = message.DeviceId;
            this.Bus.Publish(new TemperatureDeviceAdded {DeviceId = message.DeviceId});
            this.Bus.Return(ResultCode.OK);
            Console.WriteLine("AddTemperatureDevice for device id {0}", message.DeviceId);
        }

        public void Handle(TemperatureSensorDataReceived message)
        {
            int value = int.Parse(message.Value);
            Data.DataPoints.Add(value);

            if (value > 95)
            {
                Bus.Publish(new DeviceTemperatureAboveAcceptableThreshold
                {
                    DeviceId = message.DeviceId,
                    Value = value
                });
            }

            if (value < 10)
            {
                Bus.Publish(new DeviceTemperatureBelowAcceptableThreshold
                {
                    DeviceId = message.DeviceId,
                    Value = value
                });
            }

            if (Data.DataPoints.Count%10 == 0)
            {
                Bus.Publish(new AveragedTemperatureUpdated
                {
                    DeviceId = Data.DeviceId,
                    Average = Data.DataPoints.TakeLast(100).Average()
                }); 
            }
            Console.WriteLine("TemperatureSensorDataReceived for device id {0}", message.DeviceId);
        }

        public void Handle(RemoveTemperatureDevice message)
        {
            this.MarkAsComplete();
            this.Bus.Publish(new TemperatureDeviceRemoved {DeviceId = message.DeviceId});
            Console.WriteLine("RemoveTemperatureDevice for device id {0}", message.DeviceId);
        }
    }
}