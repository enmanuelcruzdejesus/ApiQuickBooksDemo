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
    public class QboInvoiceController : ApiController
    {
        public async  Task<HttpResponseMessage> Get()
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

                //looking for invoice 
                QueryService<Invoice> invoiceQueryService = new QueryService<Invoice>(serviceContext);
                List<Invoice> invoices = invoiceQueryService.ExecuteIdsQuery("Select * From Invoice ORDER BY Id DESC").ToList();

                return Request.CreateResponse(HttpStatusCode.OK, invoices);

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        public async Task<HttpResponseMessage> Get(string id)
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
                //looking for invoice 
                QueryService<Invoice> itemQueryService = new QueryService<Invoice>(serviceContext);
                Invoice invoice = itemQueryService.ExecuteIdsQuery(string.Format("Select * From Invoice Where CustomerRef = '{0}' StartPosition 1 MaxResults 1", id)).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, invoice);
      

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

    }
}
