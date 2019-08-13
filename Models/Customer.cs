using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryProject.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
        public int ReviewCount { get { return Reviews.Count(); } }

    }
}