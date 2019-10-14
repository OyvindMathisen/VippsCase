using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;

namespace StripeBackend.Models
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
