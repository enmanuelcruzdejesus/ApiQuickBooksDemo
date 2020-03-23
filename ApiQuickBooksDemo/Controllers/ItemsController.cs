using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/Items")]
    public class ItemsController : ApiController
    {

        [HttpGet]
        [System.Web.Http.Route("AllItems")]
        public HttpResponseMessage AllItems()
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


                // Create a QuickBooks QueryService using ServiceContext
                QueryService<Product> querySvc = new QueryService<Product>(serviceContext);
                List<Product> itemInfo = querySvc.ExecuteIdsQuery("SELECT * FROM Item order by Id DESC ").ToList();


                return Request.CreateResponse(HttpStatusCode.OK, itemInfo);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); ;
            }




        }

        [HttpGet]
        public HttpResponseMessage Get(string id)
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


                // Create a QuickBooks QueryService using ServiceContext
                QueryService<Product> querySvc = new QueryService<Product>(serviceContext);

                List<Product> itemInfo = querySvc.ExecuteIdsQuery("SELECT * FROM Item where Id = " + "'" + id + "'").ToList();


                return Request.CreateResponse(HttpStatusCode.OK, itemInfo);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); ;
            }



        }
    }
}
