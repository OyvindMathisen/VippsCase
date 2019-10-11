using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    [Table("Password")]
    public class Password
    {
        [Key]
        public int PasswordId { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public bool Active { get; set; }

        [ForeignKey("FK_Password_UserId")]
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
