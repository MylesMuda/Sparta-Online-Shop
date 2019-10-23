using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class AccountManagementController : Controller
    {
        // GET: AccountManagement
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LastOrders()
        {
            return View();
        }
    }
}