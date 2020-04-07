using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class SalesOrders
    {
        [PrimaryKey]
        [AutoIncrement]
        public int IdOrder { get; set; }
        public string IdOrderRef { get; set; }
        public int IdVendor { get; set; }
        public int DocNumber { get; set; }
        public int IdCustomer { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime Duedate { get; set; }
        public DateTime ShipDate { get; set; }
        public int IdVendorVisit { get; set; }
        public string BillEmail { get; set; }
        public string ShipCity { get; set; }
        public string PostalCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal TotalAmt { get; set; }

        public string TranStatus { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [Reference]
        public List<SalesOrdersDetail> SalesOrdersDetails { get; set; }
    }
}
