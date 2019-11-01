using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sparta_Online_Shop.Models;

namespace Sparta_Online_Shop.Controllers
{
    public class AccountManagementController : Controller
    {
        private SpartaShopModel db = new SpartaShopModel();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PasswordConfirmationModel passwordModel)
        {
            var thisUserId = GetUserId();
            var user = db.Users.Find(thisUserId);

            if (passwordModel.NewPassword == passwordModel.ConfirmPassword)
            {
                if (Crypto.Hash(passwordModel.OldPassword) == user.UserPassword)
                {
                    user.UserPassword = Crypto.Hash(passwordModel.NewPassword);
                    user.ConfirmPassword = Crypto.Hash(passwordModel.NewPassword);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AllReviews()
        {
            var userId = GetUserId();
            List<ProductReview> reviewsToReturn = new List<ProductReview>();
            var thisUsersReviews = db.Reviews.Where(review => review.UserID == userId).ToList();
            foreach (var review in thisUsersReviews)
            {
                reviewsToReturn.Add(new ProductReview()
                {
                    Review = review,
                    Product = review.Product
                });
            }
            return View(reviewsToReturn);
        }

        public ActionResult AllOrders()
        {
            return View(GetAllOrders().Where(order => order.user.UserID == GetUserId()).ToList());
        }

        [NonAction]
        public static List<OrderPageModel> GetAllOrders()
        {
            var ordersToAdd = new List<OrderPageModel>();
            using (var db = new SpartaShopModel())
            {
                var orders = db.Orders.OrderByDescending(o => o.OrderDate);
                foreach (var order in orders)
                {
                    var orderToAdd = new OrderPageModel
                    {
                        order = order,

                        orderDetails = order.OrderDetails
                    };

                    orderToAdd.orderProducts = orderToAdd.orderDetails.
                        Select(detail => detail.Product).
                        ToList();

                    orderToAdd.orderStatus = order.OrderStatu;
                    orderToAdd.user = order.User;
                    ordersToAdd.Add(orderToAdd);
                }
            }
            return ordersToAdd.OrderBy(o => o.orderStatus.OrderStatusID).ToList();
        }

        [NonAction]
        public int GetUserId()
        {
            var id = 0;
            var email = User.Identity.Name;
            if (User.Identity.IsAuthenticated)
            {
                id = db.Users.Where(u => u.UserEmail == email).Select(u => u.UserID).FirstOrDefault();
            }
            return id;
        }
    }
}