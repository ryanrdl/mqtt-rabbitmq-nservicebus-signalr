using System;
using System.Text;
using Messages;
using Newtonsoft.Json;
using NServiceBus;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SensorProxy
{
    class Program
    {
        private static IBus _bus; 
        static void Main(string[] args)
        {
            using (_bus = BusFactory.GetBus())
            {
                MqttClient client = new MqttClient("localhost");

                string clientId = Guid.NewGuid().ToString();
                client.Connect(clientId, "administrator", "password");

                client.Subscribe(new[]
                {
                    Topics.Humidity, 
                    Topics.Temperature
                }, new[]
                {
                    MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                    MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE
                }); 
                client.MqttMsgPublishReceived += ClientOnMqttMsgPublishReceived;

                Console.ReadKey();
            }
        }

        private static void ClientOnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        { 
            if (!e.DupFlag)
            {
                SensorData data =
                    JsonConvert.DeserializeObject<SensorData>(Encoding.UTF8.GetString(e.Message));

                Console.WriteLine("Received message on topic {0} with device id {1}", e.Topic, data.DeviceId);
                
                if (e.Topic == Topics.Humidity)
                    _bus.Publish(new HumiditySensorDataReceived(data));

                if (e.Topic == Topics.Temperature)
                    _bus.Publish(new TemperatureSensorDataReceived(data));

                if (e.Topic == Topics.Proximity)
                    _bus.Publish(new ProximitySensorDataReceived(data));
            }
        } 
    }
}
