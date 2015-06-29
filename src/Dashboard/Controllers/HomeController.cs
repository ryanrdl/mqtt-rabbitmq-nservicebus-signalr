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
    }
}
