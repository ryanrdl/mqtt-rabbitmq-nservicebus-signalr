using System.Web.Mvc;
using Messages;
using Microsoft.AspNet.SignalR;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            ViewBag.Title = "Dashboard";

            return View();
        }

        public ActionResult Test()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<DeviceHub>();
            context.Clients.All.temperatureChanged(new AveragedHumidityUpdated
            {
                DeviceId = "dsf", 
                Average = 213
            });
             return Content("done");
        }
    }
}
