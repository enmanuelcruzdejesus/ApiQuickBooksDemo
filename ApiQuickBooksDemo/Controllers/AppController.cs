using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.OAuth2PlatformClient;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Webhooks.Models.DTO;
using Webhooks.Models.Utility;

namespace ApiQuickBooksDemo.Controllers
{
    [System.Web.Http.RoutePrefix("api/App")]
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


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("InitiateAuth")]
        public IHttpActionResult InitiateAuth()
        {
            
            List<OidcScopes> scopes = new List<OidcScopes>();
            scopes.Add(OidcScopes.Accounting);
            string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);
            return  this.Redirect(authorizeUrl);
            

           
        }

        [System.Web.Http.HttpGet]
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


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("webhooks")]

        public HttpResponseMessage Post()
        {
            try
            {
                //Add intial Data to DB by running the sql cmds from Scripts->InsertScriptWebhooks.sql

                //Get webhooks response payload
                string jsonData = null;
                object hmacHeaderSignature = null;
                if (System.Web.HttpContext.Current.Request.InputStream.CanSeek)
                {
                    //Move the cursor to beginning of stream if it has already been by json process
                    System.Web.HttpContext.Current.Request.InputStream.Seek(0, SeekOrigin.Begin);
                    jsonData = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
                    //Get the value of webhooks header's signature
                    hmacHeaderSignature = System.Web.HttpContext.Current.Request.Headers["intuit-signature"];
                }

                //Validate webhooks response by hading it with HMACSHA256 algo and comparing it with Intuit's header signature
                bool isRequestvalid = ProcessNotificationData.Validate(jsonData, hmacHeaderSignature);

                //If request is valid, send 200 Status to webhooks sever
                if (isRequestvalid == true)
                {
                    WebhooksNotificationdto.WebhooksData webhooksData = JsonConvert.DeserializeObject<WebhooksNotificationdto.WebhooksData>(jsonData);
                    return Request.CreateResponse(HttpStatusCode.OK, webhooksData);
                }

                return Request.CreateResponse(HttpStatusCode.Conflict, "Error");

                //Defult pgae displayed will be the Index view page when application is running

               


            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            

        }



    }
}