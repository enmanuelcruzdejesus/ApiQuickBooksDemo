using ApiCore;
using ApiQuickBooksDemo.Helpers;
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
    [RoutePrefix("api/Customer")]
    public class QboCustomerController : ApiController
    {

        [HttpGet]
        [System.Web.Http.Route("AllCustomers")]
        public HttpResponseMessage AllCustomers()
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
                QueryService<Customer> querySvc = new QueryService<Customer>(serviceContext);
                List<Customer> customerInfo = querySvc.ExecuteIdsQuery("SELECT * FROM Customer order by Id DESC ").ToList();


                return Request.CreateResponse(HttpStatusCode.OK, customerInfo);
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
                QueryService<Customer> querySvc = new QueryService<Customer>(serviceContext);

                List<Customer> customerInfo = querySvc.ExecuteIdsQuery("SELECT * FROM Customer where Id = " + "'" + id + "'").ToList();


                return Request.CreateResponse(HttpStatusCode.OK, customerInfo);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); ;
            }



        }

        [HttpGet]
        [System.Web.Http.Route("LastUpdated")]
        public HttpResponseMessage Get()
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
                QueryService<Customer> querySvc = new QueryService<Customer>(serviceContext);

                var lastUpdate = DataBaseHelper.GetLastUpdateDate(AppConfig.Instance().DbFactory.OpenDbConnection(), 1, "Customers").ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK");


                List<Customer> customers = querySvc.ExecuteIdsQuery(string.Format("SELECT * FROM Customer where Metadata.LastUpdatedTime > '{0}' ", lastUpdate)).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, customers);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); ;
            }



        }




        [HttpPost]
        public HttpResponseMessage Post([FromBody] Intuit.Ipp.Data.Customer id)
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

                id.CompanyName = "FelipeDavid";
                id.GivenName = "Felipe";
                id.FamilyName = "Little";
                id.PrimaryEmailAddr = new EmailAddress() { Address = "felipedavidmariacruz@gmail.com", Default = true };
                id.DisplayName = "FelipeDavidMaria";


                Intuit.Ipp.Data.Customer resultCustomer = dataService.Add(id) as Intuit.Ipp.Data.Customer;

                return Request.CreateResponse(HttpStatusCode.OK, resultCustomer);





            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); ;
            }




        }

        //[HttpPost]
        ////public async Task<HttpResponseMessage> BulkInsert()
        ////{

        ////    try
        ////    {

        ////        var token = AppController.Token;
        ////        var realmId = AppController.realmId;


        ////        //getting customer 
        ////        // var principal = User as ClaimsPrincipal;
        ////        //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
        ////        OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(token.AccessToken);

        ////        // Create a ServiceContext with Auth tokens and realmId
        ////        ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
        ////        serviceContext.IppConfiguration.MinorVersion.Qbo = "23";


        ////        // Create a QuickBooks QueryService using ServiceContext
        ////        QueryService<Customer> querySvc = new QueryService<Customer>(serviceContext);

        ////        var lastUpdate = DataBaseHelper.GetLastUpdateDate(AppConfig.Instance().DbFactory.OpenDbConnection(),1,"Customers");


        ////        List<Customer> customers = querySvc.ExecuteIdsQuery(string.Format("SELECT * FROM Customer where Metadata.LastUpdatedTime > {0}",lastUpdate)).ToList();



        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
        ////    }

        ////}


    }
    
}
