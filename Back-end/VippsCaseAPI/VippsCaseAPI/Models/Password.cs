using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.Models
{
    public class Password
    {
        [Key]
        public int PasswordId { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public bool Active { get; set; }

        [ForeignKey("FK_Password_UserId")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
