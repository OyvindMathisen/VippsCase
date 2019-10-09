using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }

        [ForeignKey("FK_Order_UserId")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
