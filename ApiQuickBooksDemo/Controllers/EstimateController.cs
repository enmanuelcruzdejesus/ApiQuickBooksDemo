using ApiCore;
using ApiQuickBooksDemo.Entities;
using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/Estimate")]
    public class EstimateController : ApiController
    {

        [HttpPost]
 
        public async Task<HttpResponseMessage> Post()
        {

            try
            {
                var token = AppController.Token;
                var realmId = AppController.realmId;

                // var principal = User as ClaimsPrincipal;
                //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(token.AccessToken);

                // Create a ServiceContext with Auth tokens and realmId
                ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                serviceContext.IppConfiguration.MinorVersion.Qbo = "23";

                DataService dataService = new DataService(serviceContext);


                Batch batch = dataService.CreateNewBatch();


                //Getting neworders
                var db = AppConfig.Instance().DbFactory.OpenDbConnection();
                var orders = db.LoadSelect<SalesOrders>(o => o.IdOrder > 2).ToList();

                //converting orders in estimates
                List<Estimate> estimates = new List<Estimate>();

                orders.ForEach((o) => 
                {
                    Estimate e = GetEstimate(o);
                    estimates.Add(e);
                });

                for (int i = 0; i < estimates.Count; i++)
                {
                    batch.Add(estimates[i], string.Format("bID{0}",i), OperationEnum.create);
                }
             

                batch.Execute();

                IntuitBatchResponse addEstimateResponse = batch["bID1"];
                if (addEstimateResponse.ResponseType == ResponseType.Entity)
                {
                    Estimate addedEstimate = addEstimateResponse.Entity as Estimate;
                }

            
                return Request.CreateResponse(HttpStatusCode.OK, estimates);




            }

            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        static Estimate GetEstimate(SalesOrders order)
        {

            Estimate estimate = new Estimate();
            estimate.CustomerRef = new ReferenceType() { Value = order.IdCustomer.ToString() };
            estimate.TotalAmt = order.TotalAmt;
            estimate.TxnDate = DateTime.Now;
            estimate.TxnDateSpecified = true;
            estimate.TxnTaxDetail = new TxnTaxDetail() { TotalTax = 0 };
            estimate.AutoDocNumber = true;
            estimate.AutoDocNumberSpecified = true;


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

                lineSalesItemLineDetail.ItemRef = new ReferenceType()
                {
                    name = item.Description,
                    Value = item.IdItem.ToString()
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


 

     
    }
}
