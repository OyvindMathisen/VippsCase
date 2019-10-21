using Stripe;

namespace VippsCaseAPI.Models.Stripe
{
    public class StripeErrorHandler
    {
        private const string CardErrorIntro = "We were unable to charge your payment method for the purchase. ";

        public string ErrorHandler(StripeException exception)
        {
            System.Diagnostics.Debug.WriteLine("Error:" + exception.StripeError);

            switch (exception.StripeError.ErrorType)
            {
                case "api_connection_error":
                // Failure to connect to Stripe's API.
                case "api_error":
                    // API errors cover any other type of problem (e.g., a temporary problem with Stripe's servers), and are extremely uncommon.
                    return "Unable to connect to our payment servers. Please try again at a later time.";
                case "authentication_error":
                    // Failure to properly authenticate yourself in the request.
                    return "We're unable to connect to our services to handle your request. Please contact customer support for further details.";
                case "card_error":
                    // Card errors are the most common type of error you should expect to handle.
                    // They result when the user enters a card that can't be charged for some reason.
                    // Sends in DeclineCode if it is not null, if so, we use the Code instead.
                    return CardError(exception.StripeError.DeclineCode ?? exception.StripeError.Code);
                case "idempotency_error":
                    // Idempotency errors occur when an Idempotency-Key is re-used on a request that does not match the first request's API endpoint and parameters.
                    // NOTE: This is handled already in the StripeController. This is a fallback message.
                    return "Your transaction seems to already have been handled. Please check your purchase history or contact customer support for further details.";
                case "invalid_request_error":
                    // Invalid request errors arise when your request has invalid parameters.
                    return "Your request could not be processed at this current time. Please contact customer support for further details.";
                case "rate_limit_error":
                    // Too many requests hit the API too quickly.
                    return "We're experiencing higher volumes of connections. Please try again at a later time.";
                default:
                    return "Our server encountered an error, and was unable to handle your request. Try again later, and if the issue persists, contact customer support.";
            }
        }

        private string CardError(string declineCode)
        {
            switch (declineCode)
            {
                case "authentication_required":
                    return
                        CardErrorIntro + "Please authenticate your card when prompted to do so.";
                case "approve_with_id":
                case "issuer_not_available":
                case "reenter_transaction":
                case "try_again_later":
                    return
                        CardErrorIntro + "Please try again, and if the issue persists, please contact your card issuer.";
                case "call_issuer":
                case "card_not_supported":
                case "card_velocity_exceeded":
                case "do_not_honor":
                case "do_not_try_again":
                case "fraudulent":
                case "generic_decline":
                case "lost_card":
                case "merchant_blacklist":
                case "new_account_information_available":
                case "no_action_taken":
                case "not_permitted":
                case "pickup_card":
                case "restricted_card":
                case "revocation_of_all_authorizations":
                case "revocation_of_authorization":
                case "security_violation":
                case "service_not_allowed":
                case "stolen_card":
                case "stop_payment_order":
                case "transaction_not_allowed":
                    return
                        CardErrorIntro + "Please contact your card issuer for further details.";
                case "currency_not_supported":
                    return
                        CardErrorIntro + "Check with your card issuer if your card supports our currency.";
                case "duplicate_transaction":
                    // NOTE: This is handled already in the StripeController. This is a fallback message.
                    return
                        "A similar transaction has already been handled. Please check your purchase history or contact customer support for further details.";
                case "expired_card":
                case "insufficient_funds":
                case "pin_try_exceeded":
                case "testmode_decline":
                case "withdrawal_count_limit_exceeded":
                    return
                        CardErrorIntro + "Please try another payment method.";
                case "incorrect_number":
                case "incorrect_cvc":
                case "incorrect_pin":
                case "incorrect_zip":
                case "invalid_cvc":
                case "invalid_expiry_year":
                case "invalid_number":
                case "invalid_pin":
                case "offline_pin_required":
                case "online_or_offline_pin_required":
                    return
                        CardErrorIntro + "Please confirm your details and try again.";
                case "invalid_account":
                case "invalid_amount":
                    return
                        CardErrorIntro + "Contact your card issuer to ensure your payment method is working correctly.";
                case "processing_error":
                    return
                        CardErrorIntro + "Please try again, and if the issue persists try again at a later time.";
                default:
                    return
                        CardErrorIntro + "Please contact our customer support for further assistance.";
            }
        }
    }
}
