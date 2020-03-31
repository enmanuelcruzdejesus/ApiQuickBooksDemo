using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class DeliveryOrders
    {
        [PrimaryKey]
        [AutoIncrement]
        public int IdDeliveryOrder { get; set; }
        public int IdDelivery { get; set; }
        public int IdInvoice { get; set; }
        public int IdSupervisor { get; set; }
        public string Signature { get; set; }
        public bool Delivered { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdate { get; set; }

        [Reference]
        Invoices Invoice { get;set; }


    }
}

