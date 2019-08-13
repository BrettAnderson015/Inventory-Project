using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using InventoryProject.Models;

namespace InventoryProject.DAL
{
    public class InventoryContext : DbContext
    {
        public InventoryContext() : base("InventoryContext") { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<InventoryProject.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<InventoryProject.Models.Review> Reviews { get; set; }
    }
}