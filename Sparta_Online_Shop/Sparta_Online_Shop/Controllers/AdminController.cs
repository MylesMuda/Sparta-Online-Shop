using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sparta_Online_Shop;

namespace Sparta_Online_Shop.Controllers
{
    public class AdminController : Controller
    {
        private SpartaShopModel db = new SpartaShopModel();

        private int? totalSales = 0;
        private decimal? Profit = 0;
        private decimal? ProfitDay= 0;
        private decimal? ProfitMonth = 0;
        private decimal? ProfitYear = 0;
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
            return View();
        }
    }
}