using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

// ReSharper disable once CheckNamespace
namespace Sparta_Online_Shop.Controllers
{
    public class ProductsController : Controller
    {
        private SpartaShopModel db = new SpartaShopModel();

        [Authorize(Roles = "Admin")]
        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var productReview = new ProductReviews();
            var product = db.Products.Find(id);
            var reviews = db.Reviews.Where(review => review.ProductID == id).ToList();
            productReview.Product = product;
            productReview.Reviews = reviews;
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(productReview);
        }


        public ActionResult FlagReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }

            return View(review);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FlagReview(Review review)
        {
            var reviewToUpdate = db.Reviews.Find(review.ReviewID);
            if (reviewToUpdate != null)
            {
                reviewToUpdate.Flagged = review.Flagged;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Products/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,SKU,ProductName,ProductDescription,Stock,Price")]
            Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Products/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,SKU,ProductName,ProductDescription,Stock,Price")]
            Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product ?? throw new InvalidOperationException());
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