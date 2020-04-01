using ApiCore;
using ApiQuickBooksDemo.Helpers;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
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
                if(AppConfig.Token != null)
                {
                    Console.WriteLine("QBO JOB EXECUTING!!!");
                    var lastUpdateOrdersSync = AppConfig.Instance().Db.GetLastUpdateDate(100, "SalesOrders");
                    var lastUpdatePaymentSync = AppConfig.Instance().Db.GetLastUpdateDate(100, "Payments");


                    
                    var t1Inserted = AppConfig.Instance().Db.TransSyncLog.Get(t => t.TableName == "SalesOrders" && t.Operation == "Insert" && t.CreatedDate > lastUpdateOrdersSync).Select(x => x.TransId);
                    var t1Updated = AppConfig.Instance().Db.TransSyncLog.Get(t => t.TableName == "SalesOrders" && t.Operation == "Update" && t.CreatedDate > lastUpdateOrdersSync).Select(x => x.TransId); ;

                    var t2Inserted = AppConfig.Instance().Db.TransSyncLog.Get(t => t.TableName == "Payments" && t.Operation == "Insert" && t.CreatedDate > lastUpdatePaymentSync).Select(x => x.TransId); ;
                    var t2Updated = AppConfig.Instance().Db.TransSyncLog.Get(t => t.TableName == "Payments" && t.Operation == "Update" && t.CreatedDate > lastUpdatePaymentSync).Select(x => x.TransId); ;

                    //Getting sales orders and payments
                    var insertedOrders = AppConfig.Instance().Db.SalesOrders.GetLoadRerefence().Where(s => Sql.In(s.IdOrder, t1Inserted));
                    var updatedOrders = AppConfig.Instance().Db.SalesOrders.GetLoadRerefence().Where(s => Sql.In(s.IdOrder, t1Updated));

                    var insertedPayments = AppConfig.Instance().Db.Payments.GetLoadRerefence().Where(p => Sql.In(p.IdPayment, t2Inserted));
                    var updatedPayments = AppConfig.Instance().Db.Payments.GetLoadRerefence().Where(p => Sql.In(p.IdPayment, t2Updated));




                    //Creating Batch
                    var token = AppConfig.Token;
                    var realmId = AppConfig.realmId;


                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(token.AccessToken);

                    // Create a ServiceContext with Auth tokens and realmId
                    ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                    serviceContext.IppConfiguration.MinorVersion.Qbo = "23";

                    DataService dataService = new DataService(serviceContext);


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
                        batch.Add(insertedEstimates[i], string.Format("bID-inserted{0}", i+insertedEstimates.Count), OperationEnum.create);
                    }

                    for (int i = 0; i < updatedEstimates.Count; i++)
                    {
                        batch.Add(updatedEstimates[i], string.Format("bID-udpated{0}", i + updatedEstimates.Count), OperationEnum.update);
                    }


                    for (int i = 0; i < ptsInserted.Count; i++)
                    {
                        batch.Add(ptsInserted[i], string.Format("bID-inserted{0}", i + ptsInserted.Count), OperationEnum.create);
                    }

                    for (int i = 0; i < ptsUpdated.Count; i++)
                    {
                        batch.Add(ptsUpdated[i], string.Format("bID-updated{0}", i), OperationEnum.update);
                    }

                    batch.Execute();

                    //IntuitBatchResponse addEstimateResponse = batch["bID1"];
                    //if (addEstimateResponse.ResponseType == ResponseType.Entity)
                    //{
                    //    Estimate addedEstimate = addEstimateResponse.Entity as Estimate;
                    //}




              







                }







            });
        }

       
    }
}