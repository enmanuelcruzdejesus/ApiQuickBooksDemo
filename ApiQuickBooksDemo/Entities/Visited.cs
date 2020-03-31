using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class Visited
    {
        public int IdVisited { get; set; }
        public int IdVendorVisit { get; set; }
        public int IdSalesOrder { get; set; }
        public int IdPaymentRef { get; set; }
        public string Signature { get; set; }
        public int IdConfirmerSuperVisor { get; set; }
        public DateTime ConfirmDate { get; set; }
    }
}
