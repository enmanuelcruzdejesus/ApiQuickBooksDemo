using ApiCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    public class InvoiceController : ApiController
    {
        [HttpGet]
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
        public HttpResponseMessage Get(int id)
        {
            try
            {

                if (id > 0)
                {


                    var lastUpdateSync = AppConfig.Instance().Db.GetLastUpdateDate(id, "Invoices");
                    var invoices = AppConfig.Instance().Db.Invoices.Get(i => i.IdVendor == id && i.LastUpdate > lastUpdateSync);
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
