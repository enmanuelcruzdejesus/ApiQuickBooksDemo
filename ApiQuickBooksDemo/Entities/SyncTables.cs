using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiQuickBooksDemo.Entities
{
    public class SyncTables
    {
        public int IdVendor { get; set; }
        public  string TableName { get; set; }

        public DateTime LastUpdateSync { get; set; }

    }
}
