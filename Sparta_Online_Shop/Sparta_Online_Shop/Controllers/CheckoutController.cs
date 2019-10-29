using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class CheckoutController : Controller
    {
        private SpartaShopModel db = new SpartaShopModel();
        // GET: Checkout
        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Basket()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddItem()
        {
            //if (ModelState.IsValid)
            //{

            //}
            return RedirectToAction("Basket");
        }
        public ActionResult CheckoutError()
        {
            return View();
        }

        public ActionResult CheckoutSuccessful()
        {
            //TODO: clear basket in database

            return View();
        }

        [HttpPost]
        public ActionResult BasketPost(string Amount)
        {
            float price;
            ViewBag.Message = Amount;

            if (float.TryParse(Amount, out price))
            {
                ViewBag.Message = Amount;
                return View("Checkout");
            }

            return View("CheckoutError");
        }

        [HttpPost]
        public ActionResult PaypalPost(string orderID)
        {
            Session["orderID"] = orderID;

            // return View("CheckoutSuccessful");
            return Json(new { redirectUrl = "/checkout/checkoutsuccessful" });
        }

    }
}