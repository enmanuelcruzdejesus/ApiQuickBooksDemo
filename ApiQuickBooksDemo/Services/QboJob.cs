using ApiCore;
using ApiQuickBooksDemo.Entities;
using ApiQuickBooksDemo.Helpers;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Quartz;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiQuickBooksDemo.Services
{
    public class QboJob : IJob
    {
        public System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {
            return System.Threading.Tasks.Task.Run(() => 
            {
                ///Execty==
                ///

                if(AppConfig.Instance().ServiceFactory!= null)
                {
                    Console.WriteLine("QBO JOB EXECUTING!!!");
                    var db = AppConfig.Instance().Db;
                    var lastUpdateOrdersSync = db.GetLastUpdateDate(100, "SalesOrders");
                    var lastUpdatePaymentSync =  db.GetLastUpdateDate(100, "Payments");

                    if(AppConfig.Instance().Db.TransSyncLog.Get(x => x.CreatedDate > lastUpdateOrdersSync).Count() > 0 )
                    {
                        var t1Inserted = db.TransSyncLog.Get(t => t.TableName == "SalesOrders" && t.Operation == "Insert" && t.CreatedDate > lastUpdateOrdersSync).Select(x => x.TransId);
                        var t1Updated = db.TransSyncLog.Get(t => t.TableName == "SalesOrders" && t.Operation == "Update" && t.CreatedDate > lastUpdateOrdersSync).Select(x => x.TransId); ;

                        var t2Inserted = db.TransSyncLog.Get(t => t.TableName == "Payments" && t.Operation == "Insert" && t.CreatedDate > lastUpdatePaymentSync).Select(x => x.TransId); ;
                        var t2Updated = db.TransSyncLog.Get(t => t.TableName == "Payments" && t.Operation == "Update" && t.CreatedDate > lastUpdatePaymentSync).Select(x => x.TransId); ;

                        //Getting sales orders and payments
                        var insertedOrders = db.SalesOrders.GetLoadRerefence().Where(s => Sql.In(s.IdOrder, t1Inserted));
                        var updatedOrders = db.SalesOrders.GetLoadRerefence().Where(s => Sql.In(s.IdOrder, t1Updated));

                        var insertedPayments = db.Payments.GetAll().Where(p => Sql.In(p.IdPayment, t2Inserted));
                        var updatedPayments = db.Payments.GetAll().Where(p => Sql.In(p.IdPayment, t2Updated));




                        //Creating Batch
                        var dataService = AppConfig.Instance().ServiceFactory.getDataService();


                        Batch batch = dataService.CreateNewBatch();

                        //converting orders in estimates
                        List<Estimate> insertedEstimates = new List<Estimate>();
                        List<Estimate> updatedEstimates = new List<Estimate>();


                        List<Payment> ptsInserted = new List<Payment>();
                        List<Payment> ptsUpdated = new List<Payment>();

              



                        foreach (var o in insertedOrders)
                        {

                            Estimate e = DataBaseHelper.GetEstimate(o);
                            insertedEstimates.Add(e);
                        
                        }


                        foreach (var o in updatedOrders)
                        {

                            Estimate e = DataBaseHelper.GetEstimate(o);
                            updatedEstimates.Add(e);
                        }

                        foreach (var p in insertedPayments)
                        {

                            Payment pt = DataBaseHelper.GetPayment(p);
                            ptsInserted.Add(pt);
                           
                        }


                        foreach (var p in updatedPayments)
                        {

                            Payment pt = DataBaseHelper.GetPayment(p);
                            ptsUpdated.Add(pt);
                        }




                        for (int i = 0; i < insertedEstimates.Count; i++)
                        {
                            batch.Add(insertedEstimates[i], string.Format("bID-salesOrders{0}", i + insertedEstimates.Count), OperationEnum.create);
                        }

                        for (int i = 0; i < updatedEstimates.Count; i++)
                        {
                            batch.Add(updatedEstimates[i], string.Format("bID-salesOrdersudpated{0}", i + updatedEstimates.Count), OperationEnum.update);
                        }


                        for (int i = 0; i < ptsInserted.Count; i++)
                        {
                            batch.Add(ptsInserted[i], string.Format("bID-paymentsinserted{0}", i + ptsInserted.Count), OperationEnum.create);
                        }

                        for (int i = 0; i < ptsUpdated.Count; i++)
                        {
                            batch.Add(ptsUpdated[i], string.Format("bID-paymentsupdated{0}", i), OperationEnum.update);
                        }

                        batch.Execute();

                        foreach (var item in batch.IntuitBatchItemResponses)
                        {
                            if(item.ResponseType == ResponseType.Entity)
                            {
                                if(item.Entity is Estimate)
                                {
                                    var estimate = (Estimate)item.Entity;
                                    var IdOrder = Convert.ToInt32(estimate.TrackingNum);
                                    var order = db.SalesOrders.GetById(IdOrder);
                                    order.IdOrderRef = estimate.Id;
                                    db.SalesOrders.Update(order, x => x.IdOrder == IdOrder);


                                }

                                if(item.Entity is Payment)
                                {


                                }
                            }

                        }

                  

                     

                        //updating sync table
                        db.SyncTables.Update(new SyncTables() { IdVendor = 100, LastUpdateSync = DateTime.Now }, s => s.IdVendor == 100);

                        //Updating OrderRef 
                  

                    }




                }


            });
        }

       
    }
}