using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VippsCaseAPI.Models;

namespace VippsCaseAPI.DTOs
{
    public class CartDTO
    {
        public CartDTO(int OrderId, List<Item> Items)
        {
            this.OrderId = OrderId;
            this.Items = Items;
        }
        public int OrderId { get; set; }
        public List<Item> Items { get; set; }
    }
}
