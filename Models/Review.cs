using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryProject.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Author { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}