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
    public class ReviewsController : Controller
    {
        private SpartaShopModel db = new SpartaShopModel();

        // GET: Reviews
        // A page listing all products for testing
        // Deleted later
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }
        
        // GET: Reviews/Create?ProductID=5
        // Pass in productID of the selected product
        [Authorize]
        public ActionResult Create(int? productID)
        {
            if (productID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Create an empty review with the passed in ProductID and current UserID
            // and return it to View
            Review review = new Review();
            review.UserID = GetUserID();
            // Redirect admin to CMS page
            if(GetUserTypeID() == 2)
            {
                return Redirect("/Admin/Index");
            }
            if(review.UserID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            review.ProductID = productID;
            ViewBag.SelectedProductName = db.Products.Find(productID).ProductName;
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "SKU");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName");

            ViewBag.Rating = RatingList();

            return View(review);
        }
        
        // POST: Reviews/Create?ProductID=5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ReviewID,UserID,ProductID,Rating,ReviewText")] Review review)
        {
            if(review.ProductID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (GetUserTypeID() == 2)
            {
                return Redirect("/Admin/Index");
            }
            if (ModelState.IsValid)
            {
                // Get DateTime.Now when Submit button is pressed
                review.DateOfReview = DateTime.Now;
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "SKU", review.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", review.UserID);
            return View(review);
        }

        // GET: Reviews/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "SKU", review.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", review.UserID);
            ViewBag.Rating = RatingList();
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ReviewID,UserID,ProductID,Rating,ReviewText,DateOfReview")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "SKU", review.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", review.UserID);
            return View(review);
        }
        

        [Authorize(Roles = "Admin")]
        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review ?? throw new InvalidOperationException());
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Populate the rating drop down list with numbers from 1 to 5
        List<SelectListItem> RatingList()
        {
            var list = new List<SelectListItem>();
            for (var i = 1; i <= 5; i++)
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            return list;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Get userid of current logged in user
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
        public int? GetUserTypeID()
        {
            int? id = 0;
            string email = User.Identity.Name;
            if (User.Identity.IsAuthenticated)
            {
                using (var dbc = new SpartaShopModel())
                {
                    id = dbc.Users.Where(u => u.UserEmail == email).Select(u => u.UserTypeID).FirstOrDefault();
                }
            }
            return id;
        }
    }
}
