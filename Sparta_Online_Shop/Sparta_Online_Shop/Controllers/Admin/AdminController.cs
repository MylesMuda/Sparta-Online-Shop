﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class AdminController : Controller
    {
        private int? _totalSales = 0;
        private decimal? _income = 0;
        private decimal? _incomeDay= 0;
        private decimal? _incomeMonth = 0;
        private decimal? _incomeYear = 0;
        private string _productName = "";
        private decimal? _stockLevel = 0;
        private decimal? _setStockLevel = 10;
        public ActionResult Index()
        {
            using (var db = new SpartaShopModel())
            {
                var orderDetails = db.OrderDetails.ToList();

                foreach (OrderDetail od in orderDetails)
                {
                    _totalSales += od.Quantity;              //Totals the quantity of items sold
                }

                var orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();

                foreach (var order in orders)
                {
                    _income += order.TotalCost;              //Totals all income

                    if (order.OrderDate > DateTime.Now.AddDays(-1))
                    {
                        _incomeDay += order.TotalCost;       //Totals income from last day
                    }

                    if (order.OrderDate > DateTime.Now.AddMonths(-1))
                    {
                        _incomeMonth += order.TotalCost;     //Totals income from last month
                    }

                    if (order.OrderDate > DateTime.Now.AddYears(-1))
                    {
                        _incomeYear += order.TotalCost;      //Totals income from last year
                    }
                }

            }

            ViewBag.TotalSales = _totalSales;
            ViewBag.Income = _income;
            ViewBag.IncomeDay = _incomeDay;
            ViewBag.IncomeMonth = _incomeMonth;
            ViewBag.IncomeYear = _incomeYear;

            return View();
        }

        public ActionResult Orders()
        {
            var ordersToAdd = new List<OrderPageModel>();
            using (var db = new SpartaShopModel())
            {
                var orders = db.Orders.ToList();
                foreach (var order in orders)
                {
                    foreach (var orderToAdd in ordersToAdd)
                    {
                        orderToAdd.order = order;
                        orderToAdd.orderDetails = order.OrderDetails.ToList();
                    
                        foreach (var detail in order.OrderDetails)
                        {
                            orderToAdd.orderProducts.Add(detail.Product);
                        }

                        orderToAdd.orderStatus = order.OrderStatu;
                        orderToAdd.user = order.User;
                    }
                }
            }

            return View(ordersToAdd);
        }

    }
}