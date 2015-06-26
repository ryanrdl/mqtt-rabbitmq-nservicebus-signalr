using System;
using System.Linq;
using Messages; 
using NServiceBus.Saga;

namespace DeviceSvc
{
    public class TemperatureSaga : Saga<TemperatureSagaData>,
        IAmStartedByMessages<TemperatureSensorDataReceived>
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TemperatureSagaData> mapper)
        {
            mapper.ConfigureMapping<TemperatureSensorDataReceived>(received => received.DeviceId).ToSaga(o => o.DeviceId);
        }

        public void Handle(TemperatureSensorDataReceived message)
        {
            Data.DeviceId = message.DeviceId;
             
            Data.DataPoints.Add(int.Parse(message.Value));

            if (Data.DataPoints.Count%10 == 0)
            {
                Bus.Publish(new AveragedTemperatureUpdated {DeviceId = Data.DeviceId, Average = Data.DataPoints.TakeLast(100).Average()});
                Console.WriteLine("[{0}] Average temperature is {1}, {2} datapoints",
                    Data.DeviceId,
                    Data.DataPoints.Average(),
                    Data.DataPoints.Count);
            }
        }
    }
}
