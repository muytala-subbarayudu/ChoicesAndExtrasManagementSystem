using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using Stripe.Issuing;

namespace ChoicesExtrasManagement.Controllers
{
    public class PaymentController : Controller
    {
        public readonly ICatalogueRepository _catalogueRepository;
        public readonly ISubCatalogueRepository _subCatalogueRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ChoicesExtrasManagementDbContext _db;

        public PaymentController(ICatalogueRepository catalogueRepository, ISubCatalogueRepository subCatalogueRepository, UserManager<AppUser> userManager, ChoicesExtrasManagementDbContext context)
        {
            _catalogueRepository = catalogueRepository;
            _subCatalogueRepository = subCatalogueRepository;
            _userManager = userManager;
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CheckOut(int plotid)
        {
            string userEmail = _userManager.GetUserAsync(HttpContext.User).Result.Email;


            Decimal val = Decimal.Parse(TempData["toPay"].ToString());
            int toPay = Decimal.ToInt32(val) * 100;

            TempData["amount"] = val.ToString();

            var options1 = new ChargeListOptions
            {
                Limit = 10, // You can set the number of records you want to retrieve per API call
            };

            //var service1 = new ChargeService();
            //StripeList<Charge> charges = service1.List(
            //  options1
            //);

            var options = new SessionCreateOptions
            {
                SuccessUrl = "https://localhost:7158/" + $"Payment/OrderConfirmation",
                CancelUrl = "https://localhost:7158/" + $"Payment/Cancel",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = userEmail
            };


            var sessionLineItem = new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmount = toPay,
                    Currency = "gbp",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = "CurrentOutstandingAmount"
                    },

                },
                Quantity = 1
            };

            options.LineItems.Add(sessionLineItem);


            var service = new SessionService();
            Session session = service.Create(options);

            TempData["Session"] = session.Id;
            TempData["plotid"] = plotid.ToString();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303); // Re-direct to Stripe url as per session
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString();
                TempData["Transaction"] = transaction;

                int plotid = int.Parse(TempData["plotid"].ToString());
                var records = _db.SavedExtras.Where(x=>x.PlotId == plotid && x.TransactionId == null).ToList();

                var amt = TempData["amount"];
                Decimal val = Decimal.Parse(TempData["amount"].ToString());
                PaymentTransaction paymentTransaction = new PaymentTransaction();

                paymentTransaction.Date = DateTime.Now;
                paymentTransaction.Id = transaction;
                paymentTransaction.Status = "Success";
                paymentTransaction.Amount = val;
                _db.Entry(paymentTransaction).State = EntityState.Added;
                _db.SaveChanges();


                foreach (var item in records)
                {
                    item.TransactionId = transaction;
                    _db.Entry(item).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return View("Success");
            }
            else
            {
                return View("Cancel");
            }
        }
        public IActionResult Success()
        {
            var plotid = TempData["plotid"].ToString();
            TempData["plotid"] = plotid;
            return View();
        }

        public IActionResult Cancel()
        {
            var plotid = TempData["plotid"].ToString();
            TempData["plotid"] = plotid;
            return View();
        }

    }
}
