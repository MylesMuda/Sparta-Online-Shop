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

            ViewBag.TotalPrice = Math.Round(totalPrice, 2).ToString();
            ViewBag.BasketItems = itemsInBasket;
            return View();
        }

        public ActionResult Basket()
        {
            var userID = GetUserID();

            List<BasketItem> itemsInBasket = db.BasketItems.Where(item => item.Basket.UserID == userID).ToList();

            ViewBag.BasketItems = itemsInBasket;
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
        public ActionResult PaypalPost(string orderID)
        {
            Session["orderID"] = orderID;

            Session[checkoutSuccessfulFlag] = "yes";

            return Json(new { redirectUrl = "/checkout/checkoutsuccessful" });
        }

        [Authorize]
        public ActionResult AddItem(int Quantity, int ProductID)
        {
            Basket UserBasket = null;
            BasketItem currentRow = null;
            int UserID = GetUserID();

            List<Basket> Baskets = db.Baskets.Where(b => b.UserID == UserID).ToList();
            if(Baskets.Count > 0)
            {
                UserBasket = Baskets[0];
            }
            else
            {
                UserBasket = new Basket();
                UserBasket.UserID = UserID;

                db.Baskets.Add(UserBasket);
                db.SaveChanges();
            }

            List<BasketItem> BasketItems = db.BasketItems.Where(item => item.BasketID == UserBasket.BasketID && item.ProductID == ProductID).ToList();
            if(BasketItems.Count > 0)
            {
                currentRow = BasketItems[0];
            }
            else
            {
                currentRow = new BasketItem();
                currentRow.BasketID = UserBasket.BasketID;
                currentRow.ProductID = ProductID;
                currentRow.Quantity = 0;
                db.BasketItems.Add(currentRow);
                db.SaveChanges();
            }

            currentRow.Quantity += Quantity;
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