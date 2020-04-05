using ApiCore;
using ApiQuickBooksDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/Register")]
    public class RegisterController : ApiController
    {


        [HttpPost]
        [Route("Create")]
        public HttpResponseMessage Post(Users user)
        {

            try
            {

                if (user != null)
                {
                    user.Created = DateTime.Now;
                    

                    var result = AppConfig.Instance().Db.Users.Insert(user);
                    return Request.CreateResponse(HttpStatusCode.OK, result);

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "parameter is null");
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }



        }
    }
}
