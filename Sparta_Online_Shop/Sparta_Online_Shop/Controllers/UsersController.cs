using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sparta_Online_Shop;

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
                List<OrderStatu> matchingOrderStatuses = db.OrderStatus
                    .Where(orderStatus => orderStatus.OrderStatusID == order.OrderStatusID)
                    .ToList()
                    ;
                foreach (var item in matchingOrderStatuses)
                {
                    orderStatuses.Add(item);
                }
            }

            userOrderStatusOrder.user = db.Users.Find(id);
            userOrderStatusOrder.orders = orders;
            userOrderStatusOrder.orderStatuses = orderStatuses;


            if (userOrderStatusOrder == null)
            {
                return HttpNotFound();
            }

            return View(userOrderStatusOrder);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserTypeID,FirstName,LastName,UserPassword,UserEmail,IsVerified,ActivationCode,LastLogin,Locked")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserTypeID = new SelectList(db.UserTypes, "UserTypeID", "TypeName", user.UserTypeID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
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

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
