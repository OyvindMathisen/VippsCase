using System;
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
        public async Task<ActionResult<StripeResult>> Post([FromBody] StripeCharge request)
        {
            StripeConfiguration.ApiKey = StripeApiKey;

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            StripeResult result = new StripeResult();
            Order order = _context.orders.Find(request.CartId);

            // First pass
            if (request.PaymentMethodId != null)
            {
                PaymentIntentCreateOptions options = new PaymentIntentCreateOptions
                {
                    PaymentMethod = request.PaymentMethodId,
                    Amount = request.TotalCost,
                    Currency = "nok",
                    Description = "Purchase from VippsCase.",
                    ConfirmationMethod = "manual",
                    Confirm = true,
                };

                try
                {
                    paymentIntent = service.Create(options, new RequestOptions
                    {
                        IdempotencyKey = order.IdempotencyToken
                    });

                    // Check the user making the purchase
                    User user;
                    if (request.UserId == -1)
                    {
                        // Adding a new customer if one was not specified in the request
                        user = new User
                        {
                            Name = request.CustomerDetails.FullName,
                            AddressLineOne = request.CustomerDetails.AddressLineOne,
                            AddressLineTwo = request.CustomerDetails.AddressLineTwo,
                            County = request.CustomerDetails.County,
                            PostalCode = request.CustomerDetails.PostalCode,
                            City = request.CustomerDetails.City,
                            Country = request.CustomerDetails.Country,
                            PhoneNumber = request.CustomerDetails.PhoneNumber,
                            Email = request.CustomerDetails.Email
                        };

                        // Storing our customer details.
                        _context.users.Add(user);
                    }
                    else
                    {
                        // Find the user matching the userId and add them to the order.
                        user = _context.users.Find(request.UserId);
                    }

                    // Setting the user and charge to the user we just made and the charge token.
                    order.StripeChargeToken = paymentIntent.Id;
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
                        result.Success = true;
                    }
                    else
                    {
                        // When an order fails due to an issue with stripe,
                        // we'll assign the order a new idempotency key to prevent the "idempotency_error" from stripe.
                        order.IdempotencyToken = Guid.NewGuid().ToString();
                        await _context.SaveChangesAsync();

                        // Return the error message to the user.
                        result.Success = false;
                        result.Error = _stripeErrorHandler.ErrorHandler(exception);
                    }
                    return result;
                }
            }
            // Second pass
            else
            {
                PaymentIntentConfirmOptions options = new PaymentIntentConfirmOptions { };
                try
                {
                    paymentIntent = service.Confirm(request.PaymentIntentId, options);
                }
                catch (StripeException exception)
                {
                    // When an order fails due to an issue with stripe,
                    // we'll assign the order a new idempotency key to prevent the "idempotency_error" from stripe.
                    order.IdempotencyToken = Guid.NewGuid().ToString();
                    await _context.SaveChangesAsync();

                    // Return the error message to the user.
                    result.Success = false;
                    result.Error = _stripeErrorHandler.ErrorHandler(exception);
                    return result;
                }
            }

            switch (paymentIntent.Status)
            {
                case "requires_action" when paymentIntent.NextAction.Type == "use_stripe_sdk":
                    // Tell the client to handle the action
                    result.Data = new
                    {
                        requires_action = true,
                        payment_intent_client_secret = paymentIntent.ClientSecret
                    };
                    result.Success = false;
                    break;
                case "succeeded":
                    // The payment didn't need any additional actions and completed!
                    // Handle post-payment fulfillment
                    result.Success = true;
                    break;
                default:
                    // Invalid status
                    // TODO: Change this error message to be user-friendly
                    result.Error = "Invalid PaymentIntent status";
                    result.Success = false;
                    break;
            }

            return result;
        }

        [HttpPost("charge-failed")]
        public async Task<ActionResult<StripeResult>> PostError([FromBody] StripeCharge request)
        {
            StripeResult result = new StripeResult();
            Order order = _context.orders.Find(request.CartId);
            try
            {
                order.IdempotencyToken = Guid.NewGuid().ToString();
                await _context.SaveChangesAsync();

                result.Success = true;
            }
            catch (Exception exception)
            {
                result.Success = false;
                result.Error = exception.Message;
            }

            return result;
        }

        [HttpGet("get-charge")]
        public ActionResult<StripeResult> Get([FromBody] StripeChargeRequest request)
        {
            StripeConfiguration.ApiKey = StripeApiKey;

            StripeResult result = new StripeResult();
            PaymentIntentService service = new PaymentIntentService();
            try
            {
                PaymentIntent charge = service.Get(request.Id);
                result.Data = charge;
                result.Success = true;
            }
            catch (StripeException exception)
            {
                result.Success = false;
                result.Error = _stripeErrorHandler.ErrorHandler(exception);
            }

            return result;
        }
    }
}