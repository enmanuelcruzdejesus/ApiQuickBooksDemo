using ApiQuickBooksDemo.Models;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiQuickBooksDemo
{
    public class Payments
    {
        [PrimaryKey]
        public int IdPayment { get; set; }
        public string IdPaymentRef { get; set; }
        public int IdCustomer { get; set; }
        public DateTime PaymentDate { get; set; }
        public int IdInvoice { get; set; }
        public int PaymentMethodRef { get; set; }
        public decimal TotalAmt { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [Reference]
        public Invoices invoice { get; set; }
    }
}