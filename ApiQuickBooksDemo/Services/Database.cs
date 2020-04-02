using ApiCore.Services;
using ApiQuickBooksDemo.Entities;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webhooks.Models.DTO;

namespace ApiQuickBooksDemo.Services
{
    public class Database
    {
        private IRepository<Customers> _customers = null;
        private IRepository<Products>   _products = null;
        private IRepository<SalesOrders> _salesOrders = null;
        private IRepository<SalesOrdersDetail> _salesOrdersDetails = null;
        private IRepository<Payments> _payments = null;
        private IRepository<VendorVisits> _vendorVisits = null;
        private IRepository<Invoices> _invoices = null;
        private IRepository<InvoiceDetails> _invoicesDetail = null;
        private IRepository<DeliveryOrders> _deliveryOrders = null;
        private IRepository<Roles> _roles = null;
        private IRepository<Users> _users = null;
        private IRepository<SyncTables> _syncTables = null;
        private IRepository<TransactionSyncLog> _transSyncLog = null;
        private IRepository<OAuthTokens> _oAuthToken = null;

        private string _connectionString;
        private IDbConnectionFactory _dbFactory;

        public Database(string connectionString)
        {

            this._connectionString = connectionString;
            _dbFactory = new OrmLiteConnectionFactory(_connectionString, SqlServer2014Dialect.Provider);

        }
        
        public IRepository<Customers> Customers
        {
            get
            {
                if (_customers == null)
                    _customers = new ServiceStackRepository<Customers>(_dbFactory);

                return _customers;
            }

        }

        public IRepository<Products> Products
        {
            get
            {
                if (_products == null)
                    _products = new ServiceStackRepository<Products>(_dbFactory);

                return _products;
            }

        }
        public IRepository<Invoices> Invoices
        {
            get
            {
                if (_invoices == null)
                    _invoices = new ServiceStackRepository<Invoices>(_dbFactory);

                return _invoices;
            }

        }

        public IRepository<InvoiceDetails> InvoiceDetails
        {
            get
            {
                if (_invoicesDetail == null)
                    _invoicesDetail = new ServiceStackRepository<InvoiceDetails>(_dbFactory);

                return _invoicesDetail;
            }

        }
        public IRepository<VendorVisits> VendorVisits
        {
            get
            {
                if (_vendorVisits == null)
                    _vendorVisits = new ServiceStackRepository<VendorVisits>(_dbFactory);

                return _vendorVisits;
            }

        }
        public IRepository<DeliveryOrders> DeliveryOrders
        {
            get
            {
                if (_deliveryOrders == null)
                    _deliveryOrders = new ServiceStackRepository<DeliveryOrders>(_dbFactory);

                return _deliveryOrders;
            }
        }

        public IRepository<SalesOrders> SalesOrders
        {
            get
            {
                if (_salesOrders == null)
                    _salesOrders = new ServiceStackRepository<SalesOrders>(_dbFactory);

                return _salesOrders;
            }
        }


        public IRepository<SalesOrdersDetail> SalesDetails
        {
            get
            {
                if (_salesOrdersDetails == null)
                    _salesOrdersDetails = new ServiceStackRepository<SalesOrdersDetail>(_dbFactory);

                return _salesOrdersDetails;
            }
        }


        public IRepository<Payments> Payments
        {
            get
            {
                if (_payments == null)
                    _payments = new ServiceStackRepository<Payments>(_dbFactory);

                return _payments;
            }
        }

        public IRepository<SyncTables> SyncTables
        {
            get
            {
                if (_syncTables == null)
                    _syncTables = new ServiceStackRepository<SyncTables>(_dbFactory);

                return _syncTables;
            }
        }

        public IRepository<TransactionSyncLog> TransSyncLog
        {
            get
            {
                if (_transSyncLog == null)
                    _transSyncLog = new ServiceStackRepository<TransactionSyncLog>(_dbFactory);

                return _transSyncLog;
            }
        }


         public IRepository<OAuthTokens> Tokens
        {
            get
            {
                if (_oAuthToken == null)
                    _oAuthToken = new ServiceStackRepository<OAuthTokens>(_dbFactory);

                return _oAuthToken;
            }
        }


        public DateTime GetLastUpdateDate(int vendorId, string tableName)
        {
            using (var dbCmd = _dbFactory.Open().CreateCommand())
            {
                dbCmd.CommandText = String.Format("SELECT LastUpdateSync FROM synctables WHERE IdVendor = {0} AND TableName = '{1}'", vendorId, tableName);
                var result = dbCmd.ExecuteScalar();
                if (result == DBNull.Value) return DateTime.MinValue;
                return (DateTime)result;
            }
        }



        private static object GetDataValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }

            return value;
        }





    }
}
