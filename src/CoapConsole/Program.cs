using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoAP;

namespace CoapConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Request request = new Request(Method.GET).MarkObserve();
            request.URI = new Uri("coap://californium.eclipse.org:5683/obs");

            request.Respond += (Object sender, ResponseEventArgs e) =>
            {
                Response response = e.Response;
                Console.WriteLine(Encoding.UTF8.GetString(response.Payload));
            };

            request.Send();

            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }
}
