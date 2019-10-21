using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    public enum Statuses{
        InProgress,
        Accepted,
        Declined,
        Cart
    }

    [Table("Order")]
    public class Order
    {
        public Order()
        {
            Active = true;
            CreatedAt = DateTime.Now;
            Status = Statuses.Cart;
        }

        [Key]
        public int OrderId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public Statuses Status { get; set; }

        [ForeignKey("FK_Order_UserId")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } // TODO: Remove this value during clean-up
        public string StripeChargeToken { get; set; }
        public string IdempotencyToken { get; set; }
    }
}
