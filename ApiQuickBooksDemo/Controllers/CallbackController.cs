using ApiCore;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Webhooks.Models.DTO;
using Webhooks.Models.Services;

namespace ApiQuickBooksDemo.Controllers
{

  
    public class CallbackController : ApiController
    {


         /// <summary>
        /// Code and realmid/company id recieved on Index page after redirect is complete from Authorization url
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            //Sync the state info and update if it is not the same
            var state = HttpContext.Current.Request.QueryString["state"];
         
            AppController.code = HttpContext.Current.Request.QueryString["code"] ?? "none";
            AppController.realmId = HttpContext.Current.Request.QueryString["realmId"] ?? "none";


            AppConfig.code = HttpContext.Current.Request.QueryString["code"] ?? "none";
            AppConfig.realmId = HttpContext.Current.Request.QueryString["realmId"] ?? "none";


            await GetAuthTokensAsync(AppController.code, AppController.realmId);
            

            return Ok("TODO BN!");

        }

        /// <summary>
        /// Exchange Auth code with Auth Access and Refresh tokens and add them to Claim list
        /// </summary>
        private async Task GetAuthTokensAsync(string code, string realmId)
        {

            var tokenResponse = await AppConfig.Instance().Auth2Client.GetBearerTokenAsync(AppConfig.code);        
            var db = AppConfig.Instance().Db;
             if (tokenResponse != null)
             {

                 AppController.Token = tokenResponse;
          
                //Save to token in db
                if (db.Tokens.GetAll().Count() > 0)
                    db.Tokens.DeleteAll();

                else
                {

                    OAuthTokens authTokens = new OAuthTokens();
                    authTokens.Id = 1;
                    authTokens.realmid = HttpContext.Current.Request.QueryString["realmId"] ?? "none";
                    authTokens.access_token = tokenResponse.AccessToken;
                    authTokens.access_token_expires_at = tokenResponse.AccessTokenExpiresIn;
                    authTokens.realmlastupdated = DateTime.Now;
                    authTokens.refresh_token = tokenResponse.RefreshToken;
                    authTokens.refresh_token_expires_at = tokenResponse.RefreshTokenExpiresIn;
                    authTokens.access_secret = HttpContext.Current.Request.QueryString["code"] ?? "none";
                    db.Tokens.Insert(authTokens);

                }




            }
            


        }

    }
}
