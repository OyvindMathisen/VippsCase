using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<StripeResult>> Post([FromBody] StripeCharge value)
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
                Order order = _context.orders.Find(value.CartId);

                // Try to charge the card for the order
                Charge charge = service.Create(options, new RequestOptions
                {
                    IdempotencyKey = order.IdempotencyToken
                });
                // If we continue from here, it went through successfully!
                result.Successful = true;

                // TODO: Check for anonymous user.
                User user;
                if (true)
                {
                    // Adding a new customer if one was not specified in the request
                    user = new User
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
                    _context.users.Add(user);
                }
                else
                {
                    // Find the user matching the userId and add them to the order.
                    user = _context.users.Find(value.UserId);
                }

                // Setting the user and charge to the user we just made and the charge token.
                order.StripeChargeToken = charge.Id;
                order.UserId = user.UserId;

                // Saving all of our changes.
                await _context.SaveChangesAsync();
            }
            catch (StripeException exception)
            {
                // Idempotency special handling
                if (exception.StripeError.ErrorType == "idempotency_error"
                    || exception.StripeError.DeclineCode == "duplicate_transaction"
                    || exception.StripeError.Code == "duplicate_transaction")
                {
                    result.Successful = true;
                }
                else
                {
                    result.Successful = false;
                    result.ErrorMessage = _stripeErrorHandler.ErrorHandler(exception);
                }
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