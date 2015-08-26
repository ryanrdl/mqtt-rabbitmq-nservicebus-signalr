using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoAP;
using Messages;
using Messages.Helpers;
using Newtonsoft.Json;
using RabbitMQ.Client;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Simulator
{
    public class Program
    { 
        private static readonly Random Random = new Random();
        private static readonly MqttClient Client = new MqttClient("localhost");
        private static int _count; 
        private static CancellationToken _cancellationToken;
        private static readonly ConnectionFactory Factory = new ConnectionFactory {HostName = "localhost"};

        private static string PromptForSelection()
        {
            using (var c = NewColor.Yellow())
            {
                Console.WriteLine("(M)QTT");
                Console.WriteLine("(A)MQP");
                Console.WriteLine("(C)OaP");

                using (c.ChangeTo(ConsoleColor.White))
                    Console.Write("Enter selection: ");
                return Console.ReadKey().KeyChar.ToString().ToLower();
            }
        }

        public static void Main(string[] args)
        {
            string clientId = Guid.NewGuid().ToString();
            Client.Connect(clientId, "administrator", "password"); 
             
            var tokenSource = new CancellationTokenSource();
            _cancellationToken = tokenSource.Token;
        
            using (var color = NewColor.Green())
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("-- Sensor Simulator                 --");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Select type of sensor to start:");

                string selection = PromptForSelection();
                while (!new[] {"m", "a", "c"}.Contains(selection))
                {
                    using (color.ChangeTo(ConsoleColor.Red))
                        Console.WriteLine("{0} is not a valid selection ");
                    selection = PromptForSelection();
                }

                Console.WriteLine(Environment.NewLine);
                string id = Guid.NewGuid().ToString();
                switch (selection)
                {
                    case "m":
                        Console.WriteLine("Starting temperature sensor (mqtt)"); 
                        Task.Factory.StartNew(StartTemperatureSensors, _cancellationToken);
                        break;
                    case "a":
                        Console.WriteLine("Starting humidity sensor (amqp)"); 
                        Task.Factory.StartNew(StartHumiditySensors, _cancellationToken);
                        break;
                    case "c":
                        Console.WriteLine("Starting heartbeat sensor (coap)");
                        Task.Factory.StartNew(StartCoapSensor, _cancellationToken);
                        break;
                }

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("--------------------------------------");
            } 

            Console.WriteLine("Press any key to quit...");
            Console.ReadKey();

            tokenSource.Cancel();
            Environment.Exit(0);
        }

        private static void ReportDeviceReading(SensorType type, string topic, string value)
        {
            Console.Write("{0} sensor sending value ", Enum.GetName(typeof (SensorType), type));
            using (NewColor.Green())
                Console.Write(value);
            Console.Write(" to topic {0}", topic);
            Console.WriteLine(Environment.NewLine);
        }

        private static void StartTemperatureSensors()
        {
            SensorType type = SensorType.Temperature;
            string deviceId = GetDeviceId();
            string topic = string.Format("{0}.{1}", Topics.Temperature, deviceId);
            bool running = true;

            _cancellationToken.Register(() => running = false);

            while (running)
            {
                string value = GetRandomValue(); 

                var payload =
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                        new SensorData
                        {
                            Value = value,
                            DeviceId = deviceId,
                            Type = type
                        }));

                Client.Publish(topic, payload,
                    MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
                    true);
                
                ReportDeviceReading(type, topic, value);

                Thread.Sleep(250);
                _count++;
            }
        }

        private static void StartHumiditySensors()
        {
            SensorType type = SensorType.Humidity; 
            string deviceId = GetDeviceId();
            string topic = string.Format("{0}.{1}", Topics.Humidity, deviceId);
            bool running = true;

            _cancellationToken.Register(() => running = false);
            
            while (running)
            {
                string value = GetRandomValue();
                SendAmqpData(type, topic, value, deviceId);
                Thread.Sleep(250);
                _count++;
            }
        }

        private static void StartCoapSensor()
        {
            SensorType type = SensorType.Proximity; 
            string deviceId = GetDeviceId();
            string topic = string.Format("{0}.{1}", Topics.Proximity, deviceId);

            CoapClient client = new CoapClient(new Uri("coap://californium.eclipse.org:5683/obs"));
            CoapObserveRelation obs = client.Observe(response =>
            {
                string value = Encoding.UTF8.GetString(response.Payload);
                SendAmqpData(type, topic, value, deviceId);
            });

            _cancellationToken.Register(() =>
            {
                obs.ProactiveCancel();
                Console.WriteLine("Cancelling coap observe");
            });
        }

        private static void SendAmqpData(SensorType type, string topic, string value, string deviceId)
        {
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var message = JsonConvert.SerializeObject(new SensorData
                {
                    Value = value,
                    DeviceId = deviceId,
                    Type = SensorType.Proximity
                });
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                properties.CorrelationId = Guid.NewGuid().ToString().Replace("-", string.Empty);

                channel.BasicPublish("amq.topic", topic, properties, body);

                ReportDeviceReading(type, topic, value);
            }
        }

        private static string GetRandomValue()
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

            return value.ToString();
        }

        private static string GetDeviceId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}