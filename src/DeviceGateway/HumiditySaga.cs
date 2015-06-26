using System;
using System.Linq;
using Messages;
using Messages.DeviceGateway;
using NServiceBus;
using NServiceBus.Saga;

namespace DeviceGateway
{
    public class HumiditySaga : Saga<HumiditySagaData>,
        IAmStartedByMessages<AddHumidityDevice>,
        IHandleMessages<HumiditySensorDataReceived>,
        IHandleMessages<RemoveHumidityDevice>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<HumiditySagaData> mapper)
        {
            mapper.ConfigureMapping<AddHumidityDevice>(received => received.DeviceId)
                .ToSaga(o => o.DeviceId);
            mapper.ConfigureMapping<HumiditySensorDataReceived>(received => received.DeviceId)
                .ToSaga(o => o.DeviceId);
            mapper.ConfigureMapping<RemoveHumidityDevice>(received => received.DeviceId)
                .ToSaga(o => o.DeviceId);
        }

        public void Handle(AddHumidityDevice message)
        {
            Data.DeviceId = message.DeviceId;
            this.Bus.Publish(new HumidityDeviceAdded {DeviceId = message.DeviceId});
            this.Bus.Return(ResultCode.OK);
            Console.WriteLine("AddHumidityDevice for device id {0}", message.DeviceId);
        }

        public void Handle(HumiditySensorDataReceived message)
        {
            Data.DeviceId = message.DeviceId;

            int value = int.Parse(message.Value);
            Data.DataPoints.Add(value);

            if (value > 95)
            {
                Bus.Publish(new DeviceHumidityAboveAcceptableThreshold
                {
                    DeviceId = message.DeviceId,
                    Value = value
                });
            }

            if (value < 10)
            {
                Bus.Publish(new DeviceHumidityBelowAcceptableThreshold
                {
                    DeviceId = message.DeviceId,
                    Value = value
                });
            }

            if (Data.DataPoints.Count%10 == 0)
            {
                Bus.Publish(new AveragedHumidityUpdated
                {
                    DeviceId = Data.DeviceId,
                    Average = Data.DataPoints.TakeLast(100).Average()
                });
            }

            Console.WriteLine("HumiditySensorDataReceived for device id {0}", message.DeviceId);
        }

        public void Handle(RemoveHumidityDevice message)
        {
            this.MarkAsComplete();
            this.Bus.Publish(new HumidityDeviceRemoved {DeviceId = message.DeviceId});
            Console.WriteLine("RemoveHumidityDevice for device id {0}", message.DeviceId);
        }
    }
}