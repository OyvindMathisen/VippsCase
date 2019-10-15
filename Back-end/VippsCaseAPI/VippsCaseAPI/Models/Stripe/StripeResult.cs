namespace VippsCaseAPI.Models.Stripe
{
    public class StripeResult
    {
        public bool Successful { get; set; }
        public string ResultToken { get; set; }
        public string ErrorMessage { get; set; }
    }
}
