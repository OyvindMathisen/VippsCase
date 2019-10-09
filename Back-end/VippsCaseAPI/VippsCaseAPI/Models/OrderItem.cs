using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {

        [Key]
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public bool Active { get; set; }

        [ForeignKey("FK_OrderItem_OrderId")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey("FK_OrderItem_ItemId")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
