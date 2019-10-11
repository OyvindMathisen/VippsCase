using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    public enum Statuses
    {
        InProgress,
        Declined,
        Accepted
    }

    public class Cart
    {
        public Cart()
        {
            items = new List<Item>();
        }

        [Key]
        public int CartId { get; set; }

        [Required]
        public Statuses Status { get; set; }

        [Required]
        public List<Item> items { get; set; }

        [ForeignKey("FK_Cart_UserId")]
        public int UserId { get; set; }
    }
}
