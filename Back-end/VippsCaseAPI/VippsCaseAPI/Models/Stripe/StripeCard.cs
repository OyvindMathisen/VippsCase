namespace VippsCaseAPI.Models.Stripe
{
    public class StripeCard
    {
        public string StripeToken { get; set; }
        public long TotalCost { get; set; }

        public override string ToString()
        {
            return $"StripeToken: {StripeToken}, TotalCost: {TotalCost}.";
        }
    }
}
