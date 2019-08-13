using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InventoryProject.Models;

namespace InventoryProject.ViewModels
{
    public class ItemCustomerReview
    {
        public Item Item { get; set; }
        public Customer currentCustomer { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

    }
}