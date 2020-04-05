using ApiCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/Invoice")]
    public class InvoiceController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public HttpResponseMessage Get()
        {
            try
            {

                var invoices = AppConfig.Instance().Db.Invoices.GetLoadRerefence();

                return Request.CreateResponse(HttpStatusCode.OK, invoices);



            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
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


                    var lastUpdateSync = AppConfig.Instance().Db.GetLastUpdateDate(Convert.ToInt32(id), "Invoices");
                    var invoices = AppConfig.Instance().Db.Invoices.Get(i => i.IdVendor == Convert.ToInt32(id) && i.LastUpdate > lastUpdateSync);
                    if (invoices.Count() > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, invoices);


                       return Request.CreateResponse(HttpStatusCode.NoContent, invoices);


                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Id");
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
