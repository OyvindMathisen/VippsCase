namespace StripeBackend.Models
{
    public class StripeResult
    {
        public bool Successful { get; set; }
        public string ResultToken { get; set; }
        public string ErrorMessage { get; set; }
    }
}
