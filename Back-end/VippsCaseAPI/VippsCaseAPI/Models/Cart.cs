using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int CartId { get; set; }

        [Required]
        public Statuses Status { get; set; }

        public int UserId { get; set; }
    }
}
