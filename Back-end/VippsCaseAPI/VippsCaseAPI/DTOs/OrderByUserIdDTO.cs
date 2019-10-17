using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VippsCaseAPI.Models;

namespace VippsCaseAPI.DTOs
{
    public class OrderByUserIdDTO
    {
        public OrderByUserIdDTO()
        {
            Items = new List<Item>();
        }
        public Order Order { get; set; }
        public List<Item> Items { get; set; }
    }
}
