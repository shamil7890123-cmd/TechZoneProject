using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TechZoneProject.Models
{
    public class TechZoneContext : DbContext
    {
        public TechZoneContext() : base("TechZoneDb") { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
    }
}