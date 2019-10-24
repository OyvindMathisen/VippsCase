namespace VippsCaseAPI.Models.Stripe
{
    public class StripeCharge
    {
        public string PaymentMethodId { get; set; }
        public string PaymentIntentId { get; set; }
        public long TotalCost { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }

        public StripeCustomer CustomerDetails { get; set; }

        public override string ToString()
        {
            return $"PaymentMethodId: {PaymentMethodId}, PaymentIntentId: {PaymentIntentId}, TotalCost: {TotalCost}, CustomerDetails: {CustomerDetails}, UserId: {UserId}, CartId: {CartId}.";
        }
    }
}
