using System.Web.Mvc;

namespace MVCWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Edit","Order");
        }

        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult { Data = "Success" };
        }

    }
}
