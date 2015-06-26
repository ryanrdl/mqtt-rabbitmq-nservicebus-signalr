using System;
using System.Text;
using Messages;
using Newtonsoft.Json;
using NServiceBus;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace TemperatureClient
{
    class Program
    {
        private static IBus _bus;

        private static void Main(string[] args)
        {
            using (_bus = BusFactory.GetBus())
            { 
                MqttClient client = new MqttClient("localhost");

                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId, "administrator", "password");

                client.Subscribe(new string[] {"/sensor/temperature"}, new byte[] {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE});
                client.MqttMsgPublishReceived += ClientOnMqttMsgPublishReceived; 

                Console.ReadKey();
            }
        }

        private static void ClientOnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs mqttMsgPublishEventArgs)
        {
            if (!mqttMsgPublishEventArgs.DupFlag)
            {
                SensorData data =
                    JsonConvert.DeserializeObject<SensorData>(Encoding.UTF8.GetString(mqttMsgPublishEventArgs.Message));

                _bus.Publish(new TemperatureSensorDataReceived(data));
            }
        }
    }
}
