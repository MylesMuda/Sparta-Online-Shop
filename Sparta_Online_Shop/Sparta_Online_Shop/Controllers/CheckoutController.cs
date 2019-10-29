using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparta_Online_Shop.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        [HttpPost]
        public ActionResult Checkout(string stripeEmail, string stripeToken, float amount)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            ViewBag.testAmount = amount;
            long newAmount = (long)(amount * 100);

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });


            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = newAmount,
                Description = "Sample Charge",
                Currency = "gbp",
                Customer = customer.Id
            });

            return View();
        }
        public ActionResult Basket()
        {
            var stripePublishKey = "pk_test_ii1MdJAIrjyooeSNgb2tw1lm00PAZuxSp1";
            ViewBag.StripePublishKey = stripePublishKey;
            return View();
        }
    }
}