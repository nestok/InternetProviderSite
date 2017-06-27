using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SuperInternet.Models
{
    public class ServicesContext : DbContext
    {
        public DbSet<Service> AllServices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}