using System;
using System.Linq;
using Messages;
using NServiceBus.Saga;

namespace DeviceSvc
{
    public class HumiditySaga : Saga<HumiditySagaData>,
        IAmStartedByMessages<HumiditySensorDataReceived>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<HumiditySagaData> mapper)
        {
            mapper.ConfigureMapping<HumiditySensorDataReceived>(received => received.DeviceId).ToSaga(o => o.DeviceId);
        }

        public void Handle(HumiditySensorDataReceived message)
        {
            Data.DeviceId = message.DeviceId;
             
            Data.DataPoints.Add(int.Parse(message.Value));

            if (Data.DataPoints.Count%10 == 0)
            {
                Bus.Publish(new AveragedHumidityUpdated {DeviceId = Data.DeviceId, Average = Data.DataPoints.TakeLast(100).Average()});
                Console.WriteLine("[{0}] Average humidity is {1}, {2} datapoints",
                    Data.DeviceId,
                    Data.DataPoints.Average(),
                    Data.DataPoints.Count);
            }
        }
    }
}