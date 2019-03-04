using System.Web.Mvc;

namespace MVCWeb.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}