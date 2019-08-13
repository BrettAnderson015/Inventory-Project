using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryProject.Models;

namespace InventoryProject.ViewModels
{
    public class ItemsCustomersReviewsAll
    {
        public IEnumerable<Item> Items { get; set; }

        public IEnumerable<Customer> Customers { get; set; }

        public IEnumerable<Review> Reviews { get; set; }
    }
}