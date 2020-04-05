using ApiCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/Delivery")]
    public class DeliveryController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public HttpResponseMessage Get()
        {
            try
            {

                var deliverys = AppConfig.Instance().Db.Users.Get(u => u.IdRole == 3);
                return Request.CreateResponse(HttpStatusCode.OK, deliverys);



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }
    }
}
