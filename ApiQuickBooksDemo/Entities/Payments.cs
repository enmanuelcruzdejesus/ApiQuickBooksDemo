using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class Payments
    {
        [PrimaryKey]
        [AutoIncrement]
        public int IdPayment { get; set; }
        public string IdPaymentRef { get; set; }
        public int IdCustomer { get; set; }
        public DateTime PaymentDate { get; set; }

      
        public int IdInvoice { get; set; }
        public int PaymentMethodRef { get; set; }
        public decimal TotalAmt { get; set; }

        public string TranStatus { get; set; }
        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }


        [Reference]
        public Invoices Invoice { get; set; }
    }
}
