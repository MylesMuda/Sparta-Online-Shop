using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class AdminController : Controller
    {
        private int? totalSales;
        // GET: Admin
        public ActionResult Index()
        {
            using (var db = new SpartaShopModel())
            {
                var orderDetails = new List<OrderDetail>();
                orderDetails = db.OrderDetails.ToList();

                foreach(OrderDetail od in orderDetails)
                {
                    totalSales += od.Quantity;
                }
            }

            ViewBag.TotalSales = totalSales;
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult StockOrders()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }
    }
}