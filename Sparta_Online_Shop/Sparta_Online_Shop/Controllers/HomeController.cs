using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FAQs()
        {
            ViewBag.Message = "Your FAQs page.";

            return View();
        }
        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "Your ForgotPassword page.";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Your Login page.";

            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Your PrivacyPolicy page.";

            return View();
        }
        public ActionResult ProductPage()
        {
            ViewBag.Message = "Your ProductPage page.";

            return View();
        }
        public ActionResult SearchResults()
        {
            ViewBag.Message = "Your SearchResults page.";

            return View();
        }
        public ActionResult SignUp()
        {
            ViewBag.Message = "Your SignUp page.";

            return View();
        }
        public ActionResult TermsAndConditions()
        {
            ViewBag.Message = "Your TermsAndConditions page.";

            return View();
        }
    }
}