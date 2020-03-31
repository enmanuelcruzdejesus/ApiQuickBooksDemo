using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class Products
    {
        public int IdProduct { get; set; }
        public string IdProductRef { get; set; }
        public string ProductName { get; set; }
        public int IdCategory { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal QtyOnHand { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
