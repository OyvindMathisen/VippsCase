using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VippsCaseAPI.Models;

namespace VippsCaseAPI.DataAccess
{
    public class DBContext : DbContext 
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
            
        }

        public DbSet<User> users { get; set; }
        public DbSet<Password> passwords { get; set; }
        public DbSet<PaymentInfo> paymentInfos { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<Item> items { get; set; }
    }
}
