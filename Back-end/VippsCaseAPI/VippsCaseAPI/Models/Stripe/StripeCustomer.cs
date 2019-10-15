namespace VippsCaseAPI.Models.Stripe
{
    public class StripeCustomer
    {
        public string Name { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, AddressLineOne: {AddressLineOne}, AddressLineTwo: {AddressLineTwo}," +
                   $"PostalCode: {PostalCode}, City: {City}, Country: {Country}," +
                   $"Email: {Email}, Phone: {Phone}.";
        }
    }
}
