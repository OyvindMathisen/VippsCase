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
        public OrderItem(int OrderId, int ItemId)
        {
            this.OrderId = OrderId;
            this.ItemId = ItemId;
            Active = true;
            Quantity = 1;
        }

        [Key]
        public int OrderItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool Active { get; set; }

        [ForeignKey("FK_OrderItem_OrderId")]
        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey("FK_OrderItem_ItemId")]
        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
