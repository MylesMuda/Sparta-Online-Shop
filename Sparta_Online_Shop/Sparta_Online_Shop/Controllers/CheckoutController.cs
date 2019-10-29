using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Basket()
        {
            return View();
        }

        [Authorize]
        public ActionResult CheckoutError()
        {
            return View();
        }

        [Authorize]
        public ActionResult CheckoutSuccessful()
        {
            //TODO: This page should NOT be available directly, only via code
            //TODO: clear basket in database
            if ((string)Session["checkout-successful"] == "yes")
                return View();
            else
                return View("Basket");
        }

        [HttpPost]
        public ActionResult Checkout(string Amount)
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

            Session["checkout-successful"] = "yes";

            return Json(new { redirectUrl = "/checkout/checkoutsuccessful" });
        }
    }
}