using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security;
using Sparta_Online_Shop.Models;


namespace Sparta_Online_Shop.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            List<Product> products = new List<Product>();
            using(var dbc = new SpartaShopModel())
            {
                products = dbc.Products.ToList();
            }
            return View(products);
        }

        [ChildActionOnly]
        public ActionResult LoggedInUserName()
        {
            // set any data you need here with ViewBag object

            ViewBag.Name = "xyz";

            return PartialView("_LoggedInUserName");

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
        
        [HttpGet]
        public ActionResult Products(int? id)
        {
            List<Product> products = new List<Product>();
            if (id == null)
            {
                using (var dbc = new SpartaShopModel())
                {
                    products = dbc.Products.ToList();
                }
            }
            else
            {
                using (var dbc = new SpartaShopModel())
                {
                    Product product = dbc.Products.Find(id);

                    if (product != null)
                    {

                        products.Add(product);
                    }
                    else
                    {
                        products = null;
                    }
                }
            }

            return View(products);
        }

        public ActionResult SearchResults()
        {
            ViewBag.Message = "Your SearchResults page.";

            return View();
        }
        public ActionResult SignUp()
        {
            if (RedirectLoggedIn()) { return RedirectToAction("Index", "Home"); }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(User user)
        {

            bool Status = false;
            string message = "";



            if (ModelState.IsValid)
            {
                #region Check If Email Exists
                var EmailExists = DoesEmailExist(user.UserEmail);
                if (EmailExists)
                {
                    ModelState.Remove("Email");
                    TempData["EmailExists"] = "This email address has already been registered";
                    ModelState.AddModelError("Email", "This email address has already been registered");

                    return View();
                }
                #endregion

                #region Generate Activation Code

                user.IsVerified = false;
                user.ActivationCode = Guid.NewGuid().ToString();
                #endregion

                #region Password Hashing
                user.UserPassword = Crypto.Hash(user.UserPassword);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion

                #region Save to Database - Send Verification Email
                using (var dbc = new SpartaShopModel())
                {
                    //user.UserID = null;
                    user.UserTypeID = 1;
                    //Commit user to database
                    dbc.Users.Add(user);
                    dbc.SaveChanges();

                    SendVerificationLinkEmail(user.UserEmail, user.ActivationCode.ToString());

                    //Set success message for web output
                    message = $"Congratulations, You have successfully set up your Sparta Global Account!" +
                        $" Welcome {user.FirstName}. An account verfication email" +
                        $" has been sent to : {user.UserEmail}";
                    Status = true;
                }


                #endregion
            }
            else
            {
                //Model error handling
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
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
                        if (ExisitingUser.Locked == null || (bool)!ExisitingUser.Locked) { 
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
                                Session["AttemptFrom"] = null;
                                Session["Attempts"] = null;
                                Session["LoggedIn"] = true;
                            
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
                            message = "This account has been temporarily blocked, " +
                                "the password has been entered incorrectly too many times.";
                        }
                    }
                    else
                    {
                        if(Session["AttemptFrom"] == null || (int)Session["AttemptFrom"] != ExisitingUser.UserID)
                        {
                            Session["AttemptFrom"] = ExisitingUser.UserID;
                            Session["Attempts"] = 1; 
                            message = "Invalid password and/or email provided";
                        }
                        else if ((int)Session["AttemptFrom"] == ExisitingUser.UserID)
                        {
                            int Attempts = (int)Session["Attempts"];
                            if (Attempts < 2) {
                                Attempts++;
                                Session["Attempts"] = Attempts;
                                message = "Invalid password and/or email provided";
                            }
                            else
                            {
                                ExisitingUser.Locked = true;
                                ExisitingUser.ConfirmPassword = ExisitingUser.UserPassword;
                                dbc.SaveChanges();
                                message = "This account has been temporarily blocked, " +
                                "the password has been entered incorrectly too many times.";
                            }
                        }
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
        [NonAction]
        public bool DoesEmailExist(string email)
        {
            using (var dbc = new SpartaShopModel())
            {
                var ExistingEmail = dbc.Users.Where(u => u.UserEmail == email).FirstOrDefault();
                return ExistingEmail != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode, bool resend = false)
        {
            #region Build verification link
            var verifyURL = "/Home/VerifyAccount/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyURL);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyURL);
            #endregion

            //Set as Env Variable
            #region SMTP Credentials
            string HubEmail = "sprt.711@gmail.com";
            string HubEmailPassword = "password5!";
            var fromEmail = new MailAddress(HubEmail, "Sparta Eccomerce");
            var fromEmailPassword = HubEmailPassword;
            #endregion

            #region Recipient Details
            var toEmail = new MailAddress(email);
            #endregion

            #region Set up email details
            string subject;
            string body;
            if (resend)
            {
                subject = "Sparta Store Email Verification.";

                body = "<br/><br/>We would like to verify your email address for security purposes." +
                    " Please click on the below link to verify your email address." +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            }
            else
            {
                subject = "Welcome to The Sparta Store, Email Verification.";

                body = "<br/><br/>We are excited to tell you that your Sparta Shopping account was" +
                    " successfully created. Please click on the below link to verify your email address and get started." +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            #endregion

            #region Set up SMTP Client
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,//TLS PORT
                           // Port = 465,//SSL PORT
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            #endregion

            #region Send email
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
            #endregion
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            //Logout user from the web application and redirect to homepage
            FormsAuthentication.SignOut();
            Session["LoggedIn"] = false;
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

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            if (id != null)
            {
                using (var dbc = new SpartaShopModel())
                {
                    dbc.Configuration.ValidateOnSaveEnabled = false;

                    //Find user with the matching activation code
                    var userToValidate = dbc.Users.Where(u => u.ActivationCode == new Guid(id).ToString()).FirstOrDefault();

                    //if a user is found verify their account otherwise send error message to the user as invalid activation attempt
                    if (userToValidate != null)
                    {
                        userToValidate.IsVerified = true;
                        dbc.SaveChanges();
                        Status = true;
                    }
                    else
                    {
                        ViewBag.message = "Verification failed, the activation key does not exist";
                    }
                }
            }
            else
            {
                //If user somehow gets to this page directly, redirect them to the home page
                ViewBag.message = "Invalid Request";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Status = Status;
            return View();
        }

        public ActionResult Credits()
        {
            //ViewBag.Message = "Your TermsAndConditions page.";
            List<Creator> creators = new List<Creator>();
            using(var dbc = new SpartaShopModel())
            {
                creators = dbc.Creators.ToList();
            }
            return View(creators);
        }
    }
}