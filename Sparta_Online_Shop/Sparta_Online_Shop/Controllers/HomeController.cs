using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Sparta_Online_Shop.Models;

namespace Sparta_Online_Shop.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();
            using(var dbc = new SpartaShopModel())
            {
                products = dbc.Products.ToList();
            }
            return View(products);
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

        [HttpGet]
        public ActionResult Login()
        {
            if (RedirectLoggedIn()) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnURL = "")
        {
            string message = "";

            using (var dbc = new SpartaShopModel())
            {
                var ExisitingUser = dbc.Users.Where(u => u.UserEmail == login.UserEmail).FirstOrDefault();
                if (ExisitingUser != null)
                {
                    if (string.Compare(ExisitingUser.UserPassword, Crypto.Hash(login.UserPassword)) == 0)
                    {
                        if (ExisitingUser.IsVerified == true)
                        {
                            //ID 1 == Customer
                            //ID 2 == Admin
                            string roles = "";
                            if(ExisitingUser.UserTypeID != null && ExisitingUser.UserTypeID > 1)
                            {
                                roles = "Admin";
                                ReturnURL = ReturnURL == "" ? "/Admin/" : ReturnURL;
                            }

                            int timeout = login.RememberMe ? 525600 : 30;
                            var ticket = new FormsAuthenticationTicket(1, login.UserEmail, DateTime.Now, DateTime.Now.AddMinutes(timeout), login.RememberMe, roles);
                            //var ticket = new FormsAuthenticationTicket(login.UserEmail, login.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            FormsAuthentication.SetAuthCookie(login.UserEmail, login.RememberMe);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            
                            if (Url.IsLocalUrl(ReturnURL))
                            {
                                return Redirect(ReturnURL);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ViewBag.Warning = true;
                            message = "This email address has not yet been verified, please check your inbox and junk folder.";
                        }
                    }
                    else
                    {
                        message = "Invalid password and/or email provided";
                    }
                }
                else
                {
                    message = "Invalid password and/or email provided";
                }
            }

            ViewBag.Username = login.UserEmail;
            ViewBag.Password = login.UserPassword;
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            //Logout user from the web application and redirect to homepage
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public bool RedirectLoggedIn()
        {
            //Check if a user is logged in
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }
    }
}