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
    public class PaymentController : ApiController
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
                //looking for invoice 
                QueryService<Invoice> itemQueryService = new QueryService<Invoice>(serviceContext);
                Invoice invoice = itemQueryService.ExecuteIdsQuery(string.Format("Select * From Invoice Where DocNumber = '{0}' StartPosition 1 MaxResults 1",id)).FirstOrDefault();
                Payment payment = new Payment();


                if (invoice != null)
                {
                    var custId = invoice.CustomerRef.Value;
                    payment.CustomerRef = new ReferenceType() { Value = custId };
                    payment.TotalAmt = 1500;
                    payment.TxnDate = DateTime.Now;
                    payment.TxnDateSpecified = true;
                    payment.TxnTaxDetail = new TxnTaxDetail() { TotalTax = 0 };
                    payment.TotalAmt = 1500;
                    payment.TotalAmtSpecified = true;
                    payment.PaymentMethodRef = new ReferenceType() { Value = "1" };

                    var resultPayment = dataService.Add(payment);
                    return Request.CreateResponse(HttpStatusCode.OK, resultPayment);


                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "The invoie is invalid");
  
                }




            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }


        }
    }
}
