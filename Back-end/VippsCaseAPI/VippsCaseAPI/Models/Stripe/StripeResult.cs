namespace VippsCaseAPI.Models.Stripe
{
    public class StripeResult
    {
        public bool Successful { get; set; }
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
    }
}
