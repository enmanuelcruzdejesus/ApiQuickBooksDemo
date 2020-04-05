using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class VendorVisits
    {
        [PrimaryKey]
        [AutoIncrement]

        public int IdVendorVisit { get; set; }
        public int IdCustomer { get; set; }


        public int IdVendor { get; set; }
        public int IdSupervisor { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }


        [Reference]
        public Users Vendor { get; set; }
    
    }
}
