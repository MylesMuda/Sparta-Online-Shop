using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class AdminController : Controller
    {
        private int? totalSales = 0;
        private decimal? Profit = 0;
        private decimal? ProfitDay= 0;
        private decimal? ProfitMonth = 0;
        private decimal? ProfitYear = 0;
        private string ProductName = "";
        private decimal? StockLevel = 0;
        private decimal? SetStockLevel = 10;
        
[Authorize(Roles="Admin")]
        // GET: Admin
        public ActionResult Index()
        {
            using (var db = new SpartaShopModel())
            {
                var orderDetails = new List<OrderDetail>();
                orderDetails = db.OrderDetails.ToList();

                foreach (OrderDetail od in orderDetails)
                {
                    totalSales += od.Quantity;              //Totals the quanity of items sold
                }

                var orders = new List<Order>();

                orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();

                foreach (var order in orders)
                {
                    Profit += order.TotalCost;              //Totals all income

                    if (order.OrderDate > DateTime.Now.AddDays(-1))
                    {
                        ProfitDay += order.TotalCost;       //Totals income from last day
                    }

                    if (order.OrderDate > DateTime.Now.AddMonths(-1))
                    {
                        ProfitMonth += order.TotalCost;     //Totals income from last month
                    }

                    if (order.OrderDate > DateTime.Now.AddYears(-1))
                    {
                        ProfitYear += order.TotalCost;      //Totals income from last year
                    }
                }

            }

            ViewBag.TotalSales = totalSales;
            ViewBag.Profit = Profit;
            ViewBag.ProfitDay = ProfitDay;
            ViewBag.ProfitMonth = ProfitMonth;
            ViewBag.ProfitYear = ProfitYear;

            return View();
        }

        public ActionResult StockOrders()
        {
            using(var db = new SpartaShopModel())
            {
                var products = new List<Product>();
                products = db.Products.ToList();

                foreach(Product p in products)
                {
                    ProductName = p.ProductName;
                    StockLevel = p.Stock;
                }
            }
            ViewBag.ProductName = ProductName;
            ViewBag.StockLevel = StockLevel;
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

    }
}