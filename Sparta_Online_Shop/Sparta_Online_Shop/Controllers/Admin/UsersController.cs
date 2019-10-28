using System;
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

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.UserType);
            return View(users.ToList());
        }

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

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeName", user.UserTypeID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserTypeID,FirstName,LastName,UserPassword,UserEmail,IsVerified,ActivationCode,LastLogin,Locked")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeName", user.UserTypeID);
            return View(user);
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
