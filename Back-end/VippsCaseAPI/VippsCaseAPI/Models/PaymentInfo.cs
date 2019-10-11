using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    [Table("PaymentInfo")]
    public class PaymentInfo
    {
        [Key]
        public int PaymentInfoId { get; set; }
        [Required]
        public string PaymentToken { get; set; }

        [ForeignKey("FK_PaymentInfo_UserId")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
