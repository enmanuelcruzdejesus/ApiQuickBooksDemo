
using ApiCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public HttpResponseMessage Get()
        {
            try
            {

                var customers = AppConfig.Instance().Db.Customers.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, customers);
             


            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,ex);
            }

        }


        [HttpGet]
        [Route("Download/{id}")]
        public HttpResponseMessage Get(string id)
        {
            try
            {

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrWhiteSpace(id))
                {


                    var lastUpdateSync = AppConfig.Instance().Db.GetLastUpdateDate(Convert.ToInt32(id), "Customers");
                    var customers = AppConfig.Instance().Db.Customers.Get(c => c.LastUpdate > lastUpdateSync);
                    if (customers.Count() > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, customers);


                      return Request.CreateResponse(HttpStatusCode.NoContent, customers);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Id is invalid");
                }



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
;

        }
    }
}
