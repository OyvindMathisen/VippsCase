using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using VippsCaseAPI.DataAccess;
using VippsCaseAPI.Models;
using VippsCaseAPI.Models.Stripe;
using Order = VippsCaseAPI.Models.Order;

namespace VippsCaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : Controller
    {
        private readonly DBContext _context;
        private readonly StripeErrorHandler _stripeErrorHandler = new StripeErrorHandler();
        private const string StripeApiKey = "sk_test_MXy5T17TOA2npPloYXbFlkfy007am6BLoY";

        public StripeController(DBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("charge")]
        public ActionResult<StripeResult> Post([FromBody] StripeCharge value)
        {
            StripeConfiguration.ApiKey = StripeApiKey;
            ChargeCreateOptions options = new ChargeCreateOptions
            {
                Amount = value.TotalCost,
                Currency = "nok",
                Description = "Purchase from VippsCase.",
                Source = value.StripeToken
            };

            StripeResult result = new StripeResult();
            ChargeService service = new ChargeService();

            try
            {
                // Try to charge the card for the order
                Charge charge = service.Create(options, new RequestOptions
                {
                    IdempotencyKey = value.IdempotencyToken
                });
                // If we continue from here, it went through successfully!
                result.Successful = true;
                result.Data = charge.Id;

                // TODO: If/else check here if the user is signed in or not.

                // TODO: If signed in:
                    // Code here...
                // Else, if not signed in...

                // Adding a new customer if one was not specified in the request
                User newUser = new User
                {
                    Name = value.CustomerDetails.FullName,
                    AddressLineOne = value.CustomerDetails.AddressLineOne,
                    AddressLineTwo = value.CustomerDetails.AddressLineTwo,
                    County = value.CustomerDetails.County,
                    PostalCode = value.CustomerDetails.PostalCode,
                    City = value.CustomerDetails.City,
                    Country = value.CustomerDetails.Country,
                    PhoneNumber = value.CustomerDetails.PhoneNumber,
                    Email = value.CustomerDetails.Email
                };
                // Storing our customer details.
                _context.users.Add(newUser);

                // Adding the new customer to the charge
                Order order = _context.orders.Find(1); // Temp value until we get a cart ID from the front-end
                // Setting the user and charge to the user we just made and the charge token.
                order.StripeChargeToken = charge.Id;
                order.User = newUser;

                // Saving all of our changes.
                _context.SaveChangesAsync();
            }
            catch (StripeException exception)
            {
                result.Successful = false;
                result.ErrorMessage = _stripeErrorHandler.ErrorHandler(exception);
            }

            return result;
        }

        [HttpGet("get-charge")]
        public ActionResult<StripeResult> Get([FromBody] StripeChargeRequest value)
        {
            StripeConfiguration.ApiKey = StripeApiKey;

            StripeResult result = new StripeResult();
            ChargeService service = new ChargeService();
            try
            {
                Charge charge = service.Get(value.Id);
                result.Data = charge;
                result.Successful = true;
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