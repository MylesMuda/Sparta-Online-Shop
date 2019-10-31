using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

// ReSharper disable once CheckNamespace
namespace Sparta_Online_Shop.Controllers
{
    public class UsersController : Controller
    {
        private SpartaShopModel db = new SpartaShopModel();

        [Authorize(Roles = "Admin")]
        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.UserType);
            return View(users.ToList());
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Model to return
            UserOrderStatusOrder userOrderStatusOrder = new UserOrderStatusOrder();

            //Orders matching the user
            List<Order> orders = db.Orders.Where(order => order.UserID == id).ToList();

            // Order statuses that match the orders
            List<OrderStatu> orderStatuses = new List<OrderStatu>();


            foreach (var order in orders)
            {
                var matchingOrderStatuses = db.OrderStatus
                    .Where(orderStatus => orderStatus.OrderStatusID == order.OrderStatusID)
                    .ToList();
                orderStatuses.AddRange(matchingOrderStatuses);
            }

            userOrderStatusOrder.user = db.Users.Find(id);
            userOrderStatusOrder.orders = orders;
            userOrderStatusOrder.orderStatuses = orderStatuses;

            return View(userOrderStatusOrder);
        }

        [Authorize(Roles = "Admin")]
        // GET: Users/Edit/5
        public ActionResult LockOutUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LockOutUser(User user)
        {
            var userToUpdate = db.Users.Find(user.UserID);
            if (userToUpdate != null)
            {
                userToUpdate.Locked = user.Locked;
                userToUpdate.ConfirmPassword = userToUpdate.UserPassword;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
