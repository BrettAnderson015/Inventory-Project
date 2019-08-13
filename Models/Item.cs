using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryProject.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; } = DateTime.Now;

        public string ImageURL { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>();

        public int ReviewCount { get { return Reviews.Count(); } }

    }
}