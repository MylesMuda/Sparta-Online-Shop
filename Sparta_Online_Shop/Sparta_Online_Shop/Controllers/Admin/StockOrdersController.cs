using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

// ReSharper disable once CheckNamespace
namespace Sparta_Online_Shop.Controllers
{
    public class StockOrdersController : Controller
    {
        private SpartaShopModel db = new SpartaShopModel();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,SKU,ProductName,ProductDescription,Stock,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
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
