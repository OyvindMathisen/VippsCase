using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    [Table("Item")]
    public class Item
    {
        [Key]
        public int ItemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}
