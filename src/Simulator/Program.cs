using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using Messages;
using Newtonsoft.Json;
using NServiceBus;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Simulator
{
    public class Program
    {
        public static object SyncLock = new object();
        
        public static ConcurrentDictionary<string, string> TempSensorDeviceIds =
            new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, string> HumiditySensorDeviceIds =
            new ConcurrentDictionary<string, string>(); 

        private static readonly Random Random = new Random();
        private static readonly MqttClient Client = new MqttClient("localhost");
        private static int _count;

        public static void Main(string[] args)
        {
            string clientId = Guid.NewGuid().ToString();
            Client.Connect(clientId, "administrator", "password");

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.UseTransport<RabbitMQTransport>();
            busConfiguration.AutoSubscribe();

            var bus = Bus.Create(busConfiguration);

            bus.Start();

            while (true)
            {
                _count++;

                foreach (string deviceId in TempSensorDeviceIds.Keys)
                {
                    SimulateDeviceDataTransmission(SensorType.Temperature, Topics.Temperature, deviceId);
                }

                foreach (string deviceId in HumiditySensorDeviceIds.Keys)
                {
                    SimulateDeviceDataTransmission(SensorType.Humidity, Topics.Humidity, deviceId);
                }

                Thread.Sleep(250);
            }
        }

        private static void SimulateDeviceDataTransmission(SensorType type, string topic, string deviceId)
        {
            int value = Random.Next(1, Math.Min(120, 100 + (_count/25)));

            if (value%13 == 0)
            {
                value = value/2;
            }
            else if (value%17 == 0)
            {
                value = value*2;
            }

            var payload =
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                    new SensorData
                    {
                        Value = value.ToString(),
                        DeviceId = deviceId,
                        Type = type
                    }));

            Client.Publish(topic, payload,
                MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                true);
        }
    }
}