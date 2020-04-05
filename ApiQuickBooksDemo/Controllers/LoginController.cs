using ApiCore;
using ApiQuickBooksDemo.Entities;
using ApiQuickBooksDemo.Utils.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        [HttpPost]
        
        public HttpResponseMessage Login (Users user) 
        {
            var db = AppConfig.Instance().Db;
            var logResult = new LoginStatus();
            if(db.Users.Get(s => s.UserName == user.UserName && s.Password == user.Password).Count() > 0)
            {
                logResult.IsValid = true;
                logResult.Message = "Login was completed sucessfully";
            }
            else
            {
                logResult.IsValid = false;
                logResult.Message = "username or password are invalid";
            }

            if (logResult.IsValid)
                return Request.CreateResponse(HttpStatusCode.OK, logResult);
            else
                return Request.CreateResponse(HttpStatusCode.Unauthorized, logResult);

        }
    }
}
