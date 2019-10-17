using Microsoft.AspNetCore.Mvc;
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
                // TODO: Update description to be more descriptive.
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

                // TODO: Send charge and value.CustomerDetails into our database for further storage.
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