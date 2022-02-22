using System;
using System.Web.Mvc;

using AppleStore.InputModels;
using AppleStore.Services.Contracts;
using Microsoft.AspNet.Identity;

namespace AppleStore.Web.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService cartService;
        public CartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        public ActionResult Index()
        {
            try
            {
                string userId = User.Identity.GetUserId();
                var model = this.cartService.GetAllPurchased(userId);
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
        }
        
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddAppleInputModel model)
        {
            if (!ModelState.IsValid)
                return NotFound();
            try
            {
                this.cartService.AddApple(model);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            try
            {
                var purchasedAppleId = Request.Form.Get("PurchasedApple");
                this.cartService.Remove(purchasedAppleId);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buy(CartList_All_PurchasedInputModel model)
        {
            if (!ModelState.IsValid)
                return NotFound();
            try
            {
                this.cartService.BuyPurchased(model);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound();
            }
        }
    }
}