using System.Web.Mvc;
using AppleStore.Contracts.Services;
using static AppleStore.Common.GlobalConstants.UserRolesConstants;

namespace AppleStore.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAppleData data;

        public HomeController(IAppleData data)
        {
            this.data = data;
        }

        public ActionResult Index()
        {
            if (User.IsInRole(AdminRole))
                return RedirectToAction("Index", "Admin");

            return View(this.data.GetCount());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}