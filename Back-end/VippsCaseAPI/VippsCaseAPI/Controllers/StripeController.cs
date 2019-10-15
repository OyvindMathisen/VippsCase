using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Stripe;
using VippsCaseAPI.DataAccess;
using VippsCaseAPI.Models.Stripe;

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
                Description = "Test charge.",
                Source = value.StripeToken,
            };

            StripeResult result = new StripeResult();
            ChargeService service = new ChargeService();

            try
            {
                Charge charge = service.Create(options);
                result.Successful = true;
                result.Data = charge.Id;
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

        [HttpPost]
        [Route("add-customer")]
        public ActionResult<StripeResult> Post([FromBody] StripeCustomer value)
        {
            StripeConfiguration.ApiKey = StripeApiKey;
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
                result.Data = customer.Id;
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