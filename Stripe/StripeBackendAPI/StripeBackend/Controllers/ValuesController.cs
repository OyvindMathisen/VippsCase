using System;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripeBackend.Models;

namespace StripeBackend.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly StripeErrorHandler _stripeErrorHandler = new StripeErrorHandler();
        // POST api/charge
        [HttpPost]
        [Route("/charge")]
        public ActionResult<StripeResult> Post([FromBody] StripeCard value)
        {
            StripeConfiguration.ApiKey = "sk_test_MXy5T17TOA2npPloYXbFlkfy007am6BLoY";
            ChargeCreateOptions options = new ChargeCreateOptions
            {
                Amount = value.TotalCost,
                Currency = "nok",
                Description = "Test charge.",
                Source = value.StripeToken,
            };

            StripeResult result = new StripeResult();
            ChargeService service = new ChargeService();

            try
            {
                Charge charge = service.Create(options);
                result.Successful = true;
                result.ResultToken = charge.Id;
            }
            catch (StripeException exception)
            {
                result.Successful = false;
                result.ErrorMessage = _stripeErrorHandler.ErrorHandler(exception);
            }

            return result;
        }

        [HttpPost]
        [Route("/add-customer")]
        public ActionResult<StripeResult> Post([FromBody] StripeCustomer value)
        {
            StripeConfiguration.ApiKey = "sk_test_MXy5T17TOA2npPloYXbFlkfy007am6BLoY";
            CustomerCreateOptions options = new CustomerCreateOptions
            {
                Name = value.Name,
                Email = value.Email,
                Phone = value.Phone,
                Address = new AddressOptions
                {
                    Line1 = value.AddressLineOne,
                    Line2 = value.AddressLineTwo,
                    PostalCode = value.PostalCode,
                    City = value.City,
                    Country = value.Country
                },
                Description = "Test customer"
            };

            StripeResult result = new StripeResult();
            CustomerService service = new CustomerService();
            try
            {
                Customer customer = service.Create(options);
                result.Successful = true;
                result.ResultToken = customer.Id;
            }
            catch (StripeException exception)
            {
                result.Successful = false;
                result.ErrorMessage = _stripeErrorHandler.ErrorHandler(exception);
            }

            return result;
        }
    }
}
