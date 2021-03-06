﻿using ServiceStack.OrmLite;
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
using Webhooks.Models.Service;

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
            //if (state.Equals(AppController.auth2Client.CSRFToken, StringComparison.Ordinal))
            //{
            //    ViewBag.State = state + " (valid)";
            //}
            //else
            //{
            //    ViewBag.State = state + " (invalid)";
            //}

            AppController.code = HttpContext.Current.Request.QueryString["code"] ?? "none";
            AppController.realmId = HttpContext.Current.Request.QueryString["realmId"] ?? "none";

            DataServiceFactory.code = HttpContext.Current.Request.QueryString["code"] ?? "none";
            DataServiceFactory.realmId = HttpContext.Current.Request.QueryString["realmId"] ?? "none";



          

            await GetAuthTokensAsync(AppController.code, AppController.realmId);
            

            return Ok("TODO BN!");

        }

        /// <summary>
        /// Exchange Auth code with Auth Access and Refresh tokens and add them to Claim list
        /// </summary>
        private async Task GetAuthTokensAsync(string code, string realmId)
        {

            var tokenResponse = await AppController.auth2Client.GetBearerTokenAsync(AppController.code);
  
            var db = AppConfig.Instance().DbFactory.OpenDbConnection();
          
             if (tokenResponse != null)
             {
                 AppController.Token = tokenResponse;
                 DataServiceFactory.Token = tokenResponse;
                 //Save to token in db
                 if(db.Select<OAuthTokens>().Count() < 1)
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

                    db.Insert(authTokens);


                }




            }
            

           


            //var tokenResponse = await AppController.auth2Client.GetBearerTokenAsync(code);

            //if (tokenResponse!= null)
            //{
            //    //Save to token in db
            //    var db = AppConfig.Instance().DbFactory.OpenDbConnection();
            //    OAuthTokens authTokens = new OAuthTokens();
            //    authTokens.access_token = tokenResponse.AccessToken;
            //    authTokens.access_token_expires_at = tokenResponse.AccessTokenExpiresIn;
            //    authTokens.refresh_token = tokenResponse.RefreshToken;
            //    authTokens.refresh_token_expires_at = tokenResponse.RefreshTokenExpiresIn;
            //    authTokens.access_secret =  


            //}








            //AppController.Token = tokenResponse;





            //if (realmId != null)
            //{
            //    HttpContext.Current.Session["realmId"] = realmId;
            //}

            //Request.GetOwinContext().Authentication.SignOut("TempState");
            //var tokenResponse = await AppController.auth2Client.GetBearerTokenAsync(code);

            //var claims = new List<Claim>();

            //if (HttpContext.Current.Session["realmId"] != null)
            //{
            //    claims.Add(new Claim("realmId", HttpContext.Current.Session["realmId"].ToString()));
            //}

            //if (!string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            //{
            //    claims.Add(new Claim("access_token", tokenResponse.AccessToken));
            //    claims.Add(new Claim("access_token_expires_at", (DateTime.Now.AddSeconds(tokenResponse.AccessTokenExpiresIn)).ToString()));
            //}

            //if (!string.IsNullOrWhiteSpace(tokenResponse.RefreshToken))
            //{
            //    claims.Add(new Claim("refresh_token", tokenResponse.RefreshToken));
            //    claims.Add(new Claim("refresh_token_expires_at", (DateTime.Now.AddSeconds(tokenResponse.RefreshTokenExpiresIn)).ToString()));
            //}

            //var id = new ClaimsIdentity(claims, "Cookies");
            //Request.GetOwinContext().Authentication.SignIn(id);


        }

    }
}
