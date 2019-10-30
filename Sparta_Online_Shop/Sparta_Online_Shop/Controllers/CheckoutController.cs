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
            var cart = GetCartAndTotalPrice();

            ViewBag.TotalPrice = cart.totalPrice.ToString();
            ViewBag.BasketItems = cart.basketItems;

            return View();
        }

        public ActionResult Basket()
        {
            var cart = GetCartAndTotalPrice();

            ViewBag.TotalPrice = cart.totalPrice.ToString();
            ViewBag.BasketItems = cart.basketItems;

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

                CreateAndSaveOrder();
                ClearBasket();

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

        [NonAction]
        public void CreateAndSaveOrderDetails(int newlyCreatedOrder)
        {
            int UserID = GetUserID();

            List<BasketItem> items = GetItemsInBasket();
            foreach(BasketItem item in items)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderID = 1;
                orderDetail.ProductID = item.ProductID;
                orderDetail.ProductPrice = item.Product.Price;
                orderDetail.Quantity = item.Quantity;
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();
        }

        [NonAction]
        public void ClearBasket()
        {
            Order order = null;
            int UserID = GetUserID();

            order = new Order();
            order.UserID = UserID;
            order.OrderStatusID = 1;
            order.TotalCost = GetCartAndTotalPrice().totalPrice;
            order.OrderDate = DateTime.Now;
            order.ShipDate = null;
            db.Orders.Add(order);
            db.SaveChanges();

            return order.OrderID;
        }

        [NonAction]
        public List<Basket> GetBasket()
        {
            var BasketItems = GetItemsInBasket();
            foreach (BasketItem item in BasketItems)
            {
                db.BasketItems.Remove(item);
            }
            db.SaveChanges();
        }

        [NonAction]
        public List<BasketItem> GetItemsInBasket()
        {
            int userID = GetUserID();
            return db.BasketItems.Where(item => item.Basket.UserID == userID).ToList();
        }

        [NonAction]
        public (List<BasketItem> basketItems, decimal totalPrice) GetCartAndTotalPrice()
        {
            var userID = GetUserID();

            List<BasketItem> itemsInBasket = db.BasketItems.Where(item => item.Basket.UserID == userID).ToList();

            float totalPrice = 0;
            foreach (BasketItem item in itemsInBasket)
            {
                float itemPrice = (float)db.Products.Find(item.ProductID).Price;
                totalPrice += itemPrice * ((item.Quantity != null) ? (float)item.Quantity : 1);
            }

            return (itemsInBasket, (decimal)Math.Round(totalPrice, 2));
        }
    }
}