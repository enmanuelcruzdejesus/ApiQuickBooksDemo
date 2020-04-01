using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiQuickBooksDemo.Entities
{
    public class TransactionSyncLog
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int IdVendor { get; set; }
        public string TableName { get; set; }
        public string Operation { get; set; }
        public int TransId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}