using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly SpartaShopModel db = new SpartaShopModel();
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
        public ActionResult AddItem(int Quantity, int ProductID)
        {
            Basket userBasket = null;
            int userID = GetUserID();

            List<Basket> baskets = db.Baskets.Where(b => b.UserID == userID).ToList();
            // if(baskets != null)
            // {
            if (baskets.Count > 0)
            {
                //user has a basket
                userBasket = baskets[0];
            }
            else
            {
                userBasket = new Basket();
                userBasket.UserID = userID;

                db.Baskets.Add(userBasket);
                db.SaveChanges();
            }
            // }

            BasketItem basketItem = new BasketItem();
            basketItem.BasketID = userBasket.BasketID;
            basketItem.ProductID = ProductID;
            basketItem.Quantity = Quantity;
            db.BasketItems.Add(basketItem);
            db.SaveChanges();

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