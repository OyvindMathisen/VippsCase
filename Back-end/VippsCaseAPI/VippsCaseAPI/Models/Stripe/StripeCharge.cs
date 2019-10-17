namespace VippsCaseAPI.Models.Stripe
{
    public class StripeCharge
    {
        public string StripeToken { get; set; }
        public long TotalCost { get; set; }

        public StripeCustomer CustomerDetails { get; set; }

        public override string ToString()
        {
            return $"StripeToken: {StripeToken}, TotalCost: {TotalCost}, CustomerDetails: {CustomerDetails}";
        }
    }
}
