using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryProject.Models;

namespace InventoryProject.ViewModels
{
    public class ItemCustomer
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<Customer> Customers { get; set; }

        public Customer currentCustomer { get; set; }
    }
}