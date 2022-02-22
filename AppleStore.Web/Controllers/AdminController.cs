using System;
using System.Web.Mvc;

using AppleStore.InputModels;
using AppleStore.Services.Contracts;
using Microsoft.AspNet.Identity;

using static AppleStore.Common.GlobalConstants.UserRolesConstants;

namespace AppleStore.Web.Controllers
{
    [Authorize(Roles = AdminRole)]
    public class AdminController : BaseController
    {
        private readonly IAppleService appleService;

        public AdminController(IAppleService appleService)
        {
            this.appleService = appleService;
        }

        public ActionResult Index()
        {
            if (!User.IsInRole(AdminRole))
                return Redirect("/Home/");
            try
            {
                var model = this.appleService.GetPurchased();
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }

        public ActionResult Store()
        {
            if (!User.IsInRole(AdminRole))
                return Redirect("/Home/");

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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppleInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                this.appleService.Add(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            try
            {
                var userId = this.User.Identity.GetUserId();
                var model = (AppleInputModel)this.appleService.GetAsForm(id, userId);
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppleInputModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                this.appleService.Edit(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}