using System.Web.Mvc;

namespace AppleStore.Web.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}