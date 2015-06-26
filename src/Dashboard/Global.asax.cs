using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;

namespace Dashboard
{
    public class _
    {
        public static IBus Bus { get; set; }
    }

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            BusConfiguration busConfiguration = new BusConfiguration();
             
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.UseTransport<RabbitMQTransport>();
            busConfiguration.AutoSubscribe();    

            var bus = Bus.Create(busConfiguration);

            _.Bus = bus.Start();
        }
    }
}