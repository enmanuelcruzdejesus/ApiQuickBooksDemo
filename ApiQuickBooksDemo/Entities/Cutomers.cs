using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class Customers
    {
        public int IdCustomer { get; set; }
        public string IdCustomerRef { get; set; }
        public string CompanyName { get; set; }
        public string Suffix { get; set; }
        public string Title { get; set; }
        public string MobilePhone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Line1 { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal CreditLimit { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
