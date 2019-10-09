using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    public class PaymentInfo
    {
        [Key]
        public int PaymentInfoId { get; set; }
        public int EardNr { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Cvv { get; set; }
        public string CardType { get; set; }
        public bool Active { get; set; }

        [ForeignKey("FK_PaymentInfo_UserId")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
