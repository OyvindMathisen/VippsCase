using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stripe;
using StripeAPI.Models;

namespace StripeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StripeController : ControllerBase
    {
        [HttpPost]
        public IActionResult Get([FromBody] Request request)
        {
            StripeConfiguration.ApiKey = "sk_test_MXy5T17TOA2npPloYXbFlkfy007am6BLoY";
            var service = new PaymentIntentService();
            var options = new PaymentIntentCreateOptions
            {
                PaymentMethod = request.PaymentMethodId,
                Amount = 1099,
                Currency = "nok",
                ConfirmationMethod = "manual",
                Confirm = true,
            };

            var paymentIntent = service.Create(options);
            return GeneratePaymentResponse(paymentIntent);
        }

        private IActionResult GeneratePaymentResponse(PaymentIntent intent)
        {
            switch (intent.Status)
            {
                case "requires_action" when intent.NextAction.Type == "use_stripe_sdk":
                    // Tell the client to handle the action
                    return Ok(new
                    {
                        requires_action = true,
                        payment_intent_client_secret = intent.ClientSecret
                    });
                case "succeeded":
                    // The payment didn't need any additional actions and completed!
                    // Handle post-payment fulfillment
                    return Ok(new
                    {
                        success = true
                    });
                default:
                    // Invalid status
                    return StatusCode(500, new { error = "Invalid PaymentIntent status" });
            }
        }
    }
}
