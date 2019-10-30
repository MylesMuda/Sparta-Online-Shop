using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class CheckoutController : Controller
    {
        private BasketItem BasketItemDb = new BasketItem();
        private Basket BasketDb = new Basket();
        // GET: Checkout
        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Basket()
        {
            return View();
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
        public ActionResult AddItem(int Quantity, int Product)
        {
            GetUserID();


            return RedirectToAction("Products", "Home");
        }
        [NonAction]
        public int GetUserID()
        {
            int id = 0;
            string email = User.Identity.Name;
            if (User.Identity.IsAuthenticated)
            {
                using (var dbc = new SpartaShopModel())
                {
                    id = dbc.Users.Where(u => u.UserEmail == email).Select(u => u.UserID).FirstOrDefault();
                }
            }
            return id;
        }
    }
}