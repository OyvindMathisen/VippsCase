namespace VippsCaseAPI.Models.Stripe
{
    public class StripeCustomer
    {
        public string FullName { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string County { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        public override string ToString()
        {
            return $"Name: {FullName}, AddressLineOne: {AddressLineOne}, AddressLineTwo: {AddressLineTwo}," +
                   $"PostalCode: {PostalCode}, County: {County}, City: {City}, Country: {Country}," +
                   $"Email: {Email}, Phone: {PhoneNumber}.";
        }
    }
}
