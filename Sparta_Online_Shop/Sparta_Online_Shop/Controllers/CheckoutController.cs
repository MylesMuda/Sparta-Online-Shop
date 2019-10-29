using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class CheckoutController : Controller
    {
        static string checkoutSuccessfulFlag = "checkout-successful";
        private readonly SpartaShopModel db = new SpartaShopModel();

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

        // GET: Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            var userID = GetUserID();

            List<BasketItem> itemsInBasket = db.BasketItems.Where(item => item.Basket.UserID == userID).ToList();

            float totalPrice = 0;
            foreach(BasketItem item in itemsInBasket)
            {
                float itemPrice = (float)db.Products.Find(item.ProductID).Price;
                totalPrice += itemPrice * ((item.Quantity != null) ? (float)item.Quantity : 1);
            }

            ViewBag.Message = Math.Round(totalPrice, 2).ToString();
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
            //TODO: clear basket in database
            if ((string)Session[checkoutSuccessfulFlag] == "yes")
            {
                Session[checkoutSuccessfulFlag] = "";
                return View();
            }
            else
                return View("Basket");
        }

        [HttpPost]
        public ActionResult Checkout(string Amount)
        {
            //float price;
            ////ViewBag.Message = Amount;

            //if (float.TryParse(Amount, out price))
            //{
            //    ViewBag.Message = Amount;
            //    return View("Checkout");
            //}

            //return View("CheckoutError");

            return View("Checkout");
        }

        [HttpPost]
        public ActionResult PaypalPost(string orderID)
        {
            Session["orderID"] = orderID;

            Session[checkoutSuccessfulFlag] = "yes";

            return Json(new { redirectUrl = "/checkout/checkoutsuccessful" });
        }
    }
}