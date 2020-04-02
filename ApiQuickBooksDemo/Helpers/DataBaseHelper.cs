

using ApiCore;
using ApiQuickBooksDemo.Entities;
using Intuit.Ipp.Data;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiQuickBooksDemo.Helpers
{
    public class DataBaseHelper
    {
        public static DateTime GetLastUpdateDate(IDbConnection dbConnection, int vendorId, string tableName)
        {
            using (var dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = String.Format("SELECT LastUpdateSync FROM synctables WHERE idvendor = {0} AND tablename = '{1}'", vendorId, tableName);
                var result = dbCmd.ExecuteScalar();
                if (result is DBNull) return DateTime.MinValue;
                return (DateTime)result;
            }
        }

        public static bool Exists(IDbConnection dbConnection, string tableName, string filter)
        {
            using (var dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText =
                    String.Format("SELECT CASE WHEN EXISTS (SELECT 1 FROM {0} WHERE {1}) THEN 1 ELSE 0 END", tableName,
                                  filter);
                var result = dbCmd.ExecuteScalar();
                if (result is DBNull) return false;
                return Convert.ToBoolean(result);
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


        private static string LastUpdateFilter(IDbConnectionFactory dbFactory, int vendorId, string tableName)
        {
            DateTime lastUpdate;
            using (var db = dbFactory.Open())
            {
                lastUpdate = GetLastUpdateDate(db, vendorId, tableName);
            }
            return lastUpdate.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }



        public static string GetCustomerRef(int IdCust)
        {
            var query = AppConfig.Instance().Db.Customers.GetById(IdCust);
            return query.IdCustomerRef;
          
        }

        public static string GetProductRef(int IdProduct)
        {
            var query = AppConfig.Instance().Db.Products.GetById(IdProduct);
            return query.IdProductRef;

        }


        public static string GetInvoiceRef(int IdInvoice)
        {
            var query = AppConfig.Instance().Db.Invoices.GetById(IdInvoice);
            return query.IdInvoiceRef;

        }

        public static int GetCustomerIdByRef(string customerRef)
        {
            var db = AppConfig.Instance().DbFactory.OpenDbConnection();
            var i = db.Single<Customers>(c => c.IdCustomerRef == customerRef);
            if (i != null)
                return i.IdCustomer;

            return -1;

        }


        public static int GetProductIdByRef(string ItemRef)
        {
            var db = AppConfig.Instance().DbFactory.OpenDbConnection();
            var i = db.Single<Products>(p => p.IdProductRef == ItemRef);
            if (i != null)
                return i.IdProduct;

            return -1;

        }

        public  static int GetInvoiceIdByRef(string IdRef)
        {

            var db = AppConfig.Instance().DbFactory.OpenDbConnection();
            var i = db.Single<Invoices>(p => p.IdInvoiceRef == IdRef);
            if (i != null)
                return i.IdInvoice;

            return -1;

        }

        public static int GetInvoiceDetailId(int IdInvoice, int productId)
        {
            var db = AppConfig.Instance().DbFactory.OpenDbConnection();
            var i = db.Single<InvoiceDetails>(d => d.InvoicesId == IdInvoice && d.IdItem == productId);
            if (i != null)
                return i.Id;

            return -1;
        }


        public static int GetOrderIdByRef(string Id)
        {
            var db = AppConfig.Instance().DbFactory.OpenDbConnection();
            var i = db.Single<SalesOrders>(s => s.IdOrderRef == Id);
            if (i != null)
                return i.IdOrder;

            return -1;
        }

        public static Customers GetCustomer(Intuit.Ipp.Data.Customer cust)
        {
            Customers customer = new Customers();
            customer.IdCustomer = GetCustomerIdByRef(cust.Id);
            customer.IdCustomerRef = cust.Id;
            customer.CompanyName = cust.CompanyName;
            if(cust.Mobile!=null)
              customer.MobilePhone = cust.Mobile.Id; ;
            customer.Suffix = cust.Suffix;
            customer.Title = cust.Title;
            customer.CreditLimit = cust.CreditLimit;
            customer.CreatedDate = cust.MetaData.CreateTime;
            customer.LastUpdate = cust.MetaData.LastUpdatedTime;

            if (cust.ShipAddr != null)
            {
                cust.ShipAddr.Country  =  cust.ShipAddr.Country;
                customer.City = cust.ShipAddr.City;
                decimal lat = 0;
                decimal longitude = 0;

                if (Decimal.TryParse(cust.ShipAddr.Lat, out lat))
                    customer.Latitude = lat;

                if (Decimal.TryParse(cust.ShipAddr.Long, out longitude))
                    customer.Longitude = lat;

             

            }
           

            return customer;

        }

        public static Products GetProduct(Intuit.Ipp.Data.Item item)
        {
            Products product = new Products();
            product.IdProduct = GetProductIdByRef(item.Id);
            product.IdProductRef = item.Id;
            product.ProductName = item.Name;
            product.UnitPrice = item.UnitPrice;
            product.QtyOnHand = item.QtyOnHand;
            product.CreatedDate = item.MetaData.CreateTime;
            product.LastUpdate = item.MetaData.LastUpdatedTime;
            product.Active = item.Active;
            

            return product;

        }


        
        

        public static Invoices GetInvoice(Intuit.Ipp.Data.Invoice invoice)
        {

            var orderRef = invoice.LinkedTxn.FirstOrDefault().TxnId;
            var IdOrder = GetOrderIdByRef(orderRef);

            Invoices inv = new Invoices();
            inv.IdInvoice = GetInvoiceIdByRef(invoice.Id);
            inv.IdInvoiceRef = invoice.Id;
            inv.IdOrder = IdOrder;
            inv.DocNumber = Convert.ToInt32(invoice.DocNumber);
            inv.IdCustomer = GetCustomerIdByRef(invoice.CustomerRef.Value);
            inv.DueDate = invoice.DueDate;
            inv.Balance = invoice.Balance;
            inv.NetAmountTaxable = 0;
            inv.TotalTax = invoice.TxnTaxDetail.TotalTax;
            inv.TotalAmt = invoice.TotalAmt;
            

            //  inv.TaxPercent = invoice.TxnTaxDetail.TxnTaxCodeRef.Value;
            if(invoice.ShipAddr != null)
            {
                inv.City = invoice.ShipAddr.City;
                inv.Line1 = invoice.ShipAddr.Line1;
                inv.PostalCode = invoice.ShipAddr.PostalCode;
                inv.Latitude = Convert.ToDecimal(invoice.ShipAddr.Lat);
                inv.Longitude = Convert.ToDecimal(invoice.ShipAddr.Long);
            }
           
            inv.Status = invoice.invoiceStatus;
            inv.CreatedDate = invoice.MetaData.CreateTime;
            inv.LastUpdate = invoice.MetaData.LastUpdatedTime;

            List<InvoiceDetails> detail = new List<InvoiceDetails>();
            List<Line> lines = new List<Line>();

            foreach (var item in invoice.Line)
            {
                if(item.DetailType == LineDetailTypeEnum.SalesItemLineDetail)
                {
                    InvoiceDetails line = new InvoiceDetails();
                    var itemRef = (SalesItemLineDetail)(item.AnyIntuitObject);
                    int IdItem = DataBaseHelper.GetProductIdByRef(itemRef.ItemRef.Value);

                    line.Id = GetInvoiceDetailId(inv.IdInvoice,IdItem);
                    line.IdItem = IdItem;
                    line.Description = item.Description;
                    line.Quantity = (int)itemRef.Qty;
                    line.UnitPrice = Convert.ToDecimal(itemRef.AnyIntuitObject);
                    line.Amount = item.Amount;
                    line.CreatedDate = invoice.MetaData.CreateTime;
                    line.LastUpdate = invoice.MetaData.LastUpdatedTime;

                    detail.Add(line);
                }
             



            }

            inv.InvoiceDetails = detail;

            return inv;
        }

        public  static Estimate GetEstimate(SalesOrders order)
        {
            var customerRef = GetCustomerRef(order.IdCustomer);
            Estimate estimate = new Estimate();
            estimate.CustomerRef = new ReferenceType() { Value = customerRef };
            estimate.TotalAmt = order.TotalAmt;
            estimate.TxnDate = order.OrderDate;
            estimate.TxnDateSpecified = true;
            estimate.TxnTaxDetail = new TxnTaxDetail() { TotalTax = 0 };
            estimate.AutoDocNumber = true;
            estimate.TrackingNum = order.IdOrder.ToString();
            estimate.AutoDocNumberSpecified = true;
            //estimate.CustomField = new CustomField[] 
            //{
            //    new CustomField()
            //    {
            //        DefinitionId = "1",
            //        Name = "IdOrder",
            //        AnyIntuitObject = order.IdOrder.ToString()
                    
            //    }
            //};


            //DueDate
            estimate.DueDate = order.Duedate;
            estimate.DueDateSpecified = true;


            List<Line> lines = new List<Line>();

            foreach (var item in order.SalesOrdersDetails)
            {
                Line line = new Line();
                line.Amount = item.Amount;
                line.AmountSpecified = true;
                line.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                line.DetailTypeSpecified = true;
                line.Description = item.Description;
                line.LineNum = item.LineNum.ToString();
                //Line Sales Item Line Detail
                SalesItemLineDetail lineSalesItemLineDetail = new SalesItemLineDetail();


                //Find Item
                var productRef = GetProductRef(item.IdItem);



                lineSalesItemLineDetail.ItemRef = new ReferenceType()
                {
                    name = item.Description,
                    Value = productRef
                };

                //Line Sales Item Line Detail - UnitPrice
                lineSalesItemLineDetail.AnyIntuitObject = item.UnitPrice;
                lineSalesItemLineDetail.ItemElementName = ItemChoiceType.UnitPrice;

                //Line Sales Item Line Detail - Qty
                lineSalesItemLineDetail.Qty = item.Quantity;
                lineSalesItemLineDetail.QtySpecified = true;

                //Line Sales Item Line Detail - ServiceDate 
                lineSalesItemLineDetail.ServiceDate = DateTime.Now.Date;
                lineSalesItemLineDetail.ServiceDateSpecified = true;

                //Assign Sales Item Line Detail to Line Item
                line.AnyIntuitObject = lineSalesItemLineDetail;

                lines.Add(line);



            }


            Line subTotal = new Line();

            subTotal.Amount = order.TotalAmt;
            subTotal.DetailType = LineDetailTypeEnum.SubTotalLineDetail;
            estimate.Line = lines.ToArray();


            return estimate;
        }

        public static Intuit.Ipp.Data.Payment GetPayment(Payments payment)
        {
            Payment p = new Payment();
            var customerRef = GetCustomerRef(payment.IdCustomer);
            var invoiceRef = GetInvoiceRef(payment.IdInvoice);

           
            p.CustomerRef = new ReferenceType() { Value = customerRef };
            p.TotalAmt = payment.TotalAmt;
            p.TxnDate = DateTime.Now;
            p.TxnDateSpecified = true;
            p.TxnTaxDetail = new TxnTaxDetail() { TotalTax = 0 };
            p.TotalAmtSpecified = true;
            p.PaymentMethodRef = new ReferenceType() { Value = payment.PaymentMethodRef.ToString()};
            p.LinkedTxn = new LinkedTxn[] { new LinkedTxn() { TxnId = invoiceRef, TxnType = "Invoice" } };

            return p;


        }


      






    }
}