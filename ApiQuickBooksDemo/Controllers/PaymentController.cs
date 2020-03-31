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
    public class PaymentController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {


                var payments = AppConfig.Instance().Db.Payments.GetAll();
                if (payments.Count() > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, payments);

                return Request.CreateResponse(HttpStatusCode.NoContent, "no records to download");



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
;

        }





        [HttpPost]
        public HttpResponseMessage Post([FromBody] List<Payments> payments)
        {
            try
            {


                if (payments != null)
                {
                    payments.ForEach((x) =>
                    {
                        x.LastUpdate = DateTime.Now;
                    });


                    var payment = AppConfig.Instance().Db.Payments.BulkMerge(payments);

                    return Request.CreateResponse(HttpStatusCode.OK, payment);
                  

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
