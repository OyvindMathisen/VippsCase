using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }
        public bool Active { get; set; }

        public ICollection<PaymentInfo> PaymentInfos { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
