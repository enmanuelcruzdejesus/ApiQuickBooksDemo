using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class Invoices
    {
        [PrimaryKey]
        [AutoIncrement]

        public int IdInvoice { get; set; }
        public string IdInvoiceRef { get; set; }
        public int IdVendor { get; set; }
        public int IdCustomer { get; set; }
        public int IdOrder { get; set; }
        public int DocNumber { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Balance { get; set; }
        public decimal NetAmountTaxable { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmt { get; set; }
        public string City { get; set; }
        public string Line1 { get; set; }
        public string PostalCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [Reference]
        public List<InvoiceDetails> InvoiceDetails { get; set; }

        

    }
}
