namespace VippsCaseAPI.Models.Stripe
{
    public class StripeResult
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Error { get; set; }
    }
}
