using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
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

        public async Task<HttpResponseMessage> Post(string id)
        {
            var token = AppController.Token;
            var realmId = AppController.realmId;

            try
            {
                // var principal = User as ClaimsPrincipal;
                //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(token.AccessToken);

                // Create a ServiceContext with Auth tokens and realmId
                ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                serviceContext.IppConfiguration.MinorVersion.Qbo = "23";

                DataService dataService = new DataService(serviceContext);
                Estimate estimate = new Estimate();
                estimate.CustomerRef = new ReferenceType() { Value = id };
                estimate.TotalAmt = 1500;
                estimate.TxnDate = DateTime.Now;
                estimate.TxnDateSpecified = true;
                estimate.TxnTaxDetail = new TxnTaxDetail() { TotalTax = 0 };
                estimate.AutoDocNumber = true;
                estimate.AutoDocNumberSpecified = true;

                //DueDate
                estimate.DueDate = DateTime.Now.AddDays(30).Date;
                estimate.DueDateSpecified = true;



                Line line1 = new Line();
                Line subTotal = new Line();

                line1.Amount = 1500;
                line1.AmountSpecified = true;
                line1.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                line1.DetailTypeSpecified = true;
                line1.Description = "Consultoria";
                line1.LineNum = "1";
                //Line Sales Item Line Detail
                SalesItemLineDetail lineSalesItemLineDetail = new SalesItemLineDetail();
                //Find Item
                QueryService<Item> itemQueryService = new QueryService<Item>(serviceContext);
                Item item = itemQueryService.ExecuteIdsQuery("Select * From Item StartPosition 1 MaxResults 1").FirstOrDefault();

                lineSalesItemLineDetail.ItemRef = new ReferenceType()
                {
                    name = item.Name,
                    Value = item.Id
                };

                //Line Sales Item Line Detail - UnitPrice
                lineSalesItemLineDetail.AnyIntuitObject = 1500m;
                lineSalesItemLineDetail.ItemElementName = ItemChoiceType.UnitPrice;

                //Line Sales Item Line Detail - Qty
                lineSalesItemLineDetail.Qty = 1;
                lineSalesItemLineDetail.QtySpecified = true;

                //Line Sales Item Line Detail - ServiceDate 
                lineSalesItemLineDetail.ServiceDate = DateTime.Now.Date;
                lineSalesItemLineDetail.ServiceDateSpecified = true;

                //Assign Sales Item Line Detail to Line Item
                line1.AnyIntuitObject = lineSalesItemLineDetail;


                subTotal.Amount = 1500;
                subTotal.DetailType = LineDetailTypeEnum.SubTotalLineDetail;
                estimate.Line = new Line[] { line1 };
                var resultEstimate = dataService.Add(estimate);


                return Request.CreateResponse(HttpStatusCode.OK, resultEstimate);

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); ;
            }


        }


        [HttpGet]
        [System.Web.Http.Route("DownloadEstimates")]
        public async Task<HttpResponseMessage> DownloadEstimates()
        {
            try
            {

                //var token = AppController.Token;
                //var realmId = AppController.realmId;

                //// var principal = User as ClaimsPrincipal;
                ////OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(token.AccessToken);

                //// Create a ServiceContext with Auth tokens and realmId
                //ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                //serviceContext.IppConfiguration.MinorVersion.Qbo = "23";

                //DataService dataService = new DataService(serviceContext);


                //// Create a QuickBooks QueryService using ServiceContext
                //QueryService<Customer> querySvc = new QueryService<Customer>(serviceContext);
                //List<Customer> customerInfo = querySvc.ExecuteIdsQuery("SELECT * FROM Customer").ToList();

                return null;





            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }





        public async Task<HttpResponseMessage> BulkInsertEstimates()
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

                Estimate e1 = new Estimate();
                Estimate e2 = new Estimate();

                batch.Add(e1, "bID1", OperationEnum.create);
                batch.Add(e2, "bID2", OperationEnum.create);


                batch.Execute();

                IntuitBatchResponse addCustomerResponse = batch["bID1"];
                if (addCustomerResponse.ResponseType == ResponseType.Entity)
                {
                    Estimate addedcustomer = addCustomerResponse.Entity as Estimate;
                }

                IntuitBatchResponse addCustomerResponse2 = batch["bID2"];
                if (addCustomerResponse.ResponseType == ResponseType.Entity)
                {
                    Estimate addedcustomer = addCustomerResponse2.Entity as Estimate;
                }

                List<Estimate> estimates = new List<Estimate>();
                estimates.AddRange(new List<Estimate>() { e1,e2});


                return Request.CreateResponse(HttpStatusCode.OK, estimates);

                



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        Estimate GetEstimates(string custId, string[] Items,decimal[] prices , decimal amount)
        {

            Estimate estimate = new Estimate();
            estimate.CustomerRef = new ReferenceType() { Value = custId };
            estimate.TotalAmt = amount;
            estimate.TxnDate = DateTime.Now;
            estimate.TxnDateSpecified = true;
            estimate.TxnTaxDetail = new TxnTaxDetail() { TotalTax = 0 };
            estimate.AutoDocNumber = true;
            estimate.AutoDocNumberSpecified = true;



            //DueDate
            estimate.DueDate = DateTime.Now.AddDays(30).Date;
            estimate.DueDateSpecified = true;

            List<Line> lines = new List<Line>();
            for (int i = 0; i < Items.Length; i++)
            {
                Line line = new Line();

                line.Amount = amount;
                line.AmountSpecified = true;
                line.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                line.DetailTypeSpecified = true;
                line.Description = Items[i];
                line.LineNum = i.ToString();
                //Line Sales Item Line Detail
                SalesItemLineDetail lineSalesItemLineDetail = new SalesItemLineDetail();
          
            
                lineSalesItemLineDetail.ItemRef = new ReferenceType()
                {
                    name = Items[i],
                    Value = ""
                };

                //Line Sales Item Line Detail - UnitPrice
                lineSalesItemLineDetail.AnyIntuitObject = prices[i];
                lineSalesItemLineDetail.ItemElementName = ItemChoiceType.UnitPrice;

                //Line Sales Item Line Detail - Qty
                lineSalesItemLineDetail.Qty = 1;
                lineSalesItemLineDetail.QtySpecified = true;

                //Line Sales Item Line Detail - ServiceDate 
                lineSalesItemLineDetail.ServiceDate = DateTime.Now.Date;
                lineSalesItemLineDetail.ServiceDateSpecified = true;

                //Assign Sales Item Line Detail to Line Item
                line.AnyIntuitObject = lineSalesItemLineDetail;


                lines.Add(line);

                estimate.Line = lines.ToArray();


            }


            Line subTotal = new Line();

            subTotal.Amount = amount;
            subTotal.DetailType = LineDetailTypeEnum.SubTotalLineDetail;


            return estimate;
        }

     
    }
}
