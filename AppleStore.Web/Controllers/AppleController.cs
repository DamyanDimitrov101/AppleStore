using System;
using System.Web.Mvc;

using AppleStore.Services;
using AppleStore.Services.Contracts;
using Microsoft.AspNet.Identity;

using static AppleStore.Common.GlobalConstants.UserRolesConstants;

namespace AppleStore.Web.Controllers
{
    public class AppleController : BaseController
    {
        private ApplicationUserManager userManager;
        private readonly IAppleService appleService;

        public AppleController(ApplicationUserManager userManager, IAppleService appleService)
        {
            this.userManager = userManager;
            this.appleService = appleService;
        }

        [OutputCache(Duration = 60)]
        public ActionResult Index()
        {
            try
            {
                var model = this.appleService.GetAll();
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }

        public ActionResult Details(string id)
        {
            try
            {
                var userId = this.User.Identity.GetUserId();
                var model = this.appleService.Get(id, userId);
                
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }
    }
}