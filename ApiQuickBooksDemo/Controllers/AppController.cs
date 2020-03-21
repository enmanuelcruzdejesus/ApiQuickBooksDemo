using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;


namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/App")]
    public class AppController : ApiController
    {

        public static string code;
        public static string realmId;
        public static TokenResponse Token;

        public static string clientid = ConfigurationManager.AppSettings["clientid"];
        public static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        public static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        public static string environment = ConfigurationManager.AppSettings["appEnvironment"];


        //public static ServiceContext ServiceContext = null;
        public static OAuth2Client auth2Client = new OAuth2Client(clientid, clientsecret, redirectUrl, environment);


        [HttpGet]
        [System.Web.Http.Route("InitiateAuth")]
        public IHttpActionResult InitiateAuth()
        {
            
            List<OidcScopes> scopes = new List<OidcScopes>();
            scopes.Add(OidcScopes.Accounting);
            string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);
            return  this.Redirect(authorizeUrl);
            

           
        }

        [HttpGet]
        [System.Web.Http.Route("GetCompanyInfo")]
        public HttpResponseMessage ApiCallService()
        {
           
            try
            {
               // var principal = User as ClaimsPrincipal;
                //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("access_token").Value);
                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(Token.AccessToken);

                // Create a ServiceContext with Auth tokens and realmId
                ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                serviceContext.IppConfiguration.MinorVersion.Qbo = "23";


                // Create a QuickBooks QueryService using ServiceContext
                QueryService<CompanyInfo> querySvc = new QueryService<CompanyInfo>(serviceContext);
                CompanyInfo companyInfo = querySvc.ExecuteIdsQuery("SELECT * FROM CompanyInfo").FirstOrDefault();


                return Request.CreateResponse(HttpStatusCode.OK, companyInfo);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message); ;
            }


           
             
        }

    }
}