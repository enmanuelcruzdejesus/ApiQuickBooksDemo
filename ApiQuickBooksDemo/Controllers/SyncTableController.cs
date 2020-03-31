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
    public class SyncTableController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            try
            {

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrWhiteSpace(id))
                {



                    var syncTables = AppConfig.Instance().Db.SyncTables.Get(s => s.IdVendor == Convert.ToInt32(id));
                    if (syncTables.Count() > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, syncTables);


                    return Request.CreateResponse(HttpStatusCode.NoContent);

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Id");
                }



            }
            catch (Exception ex)
            {

                return  Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
;

        }



        [HttpPut]
        public HttpResponseMessage Put([FromBody] SyncTables syncT)
        {
            try
            {
                if (syncT != null)
                {

                    var sync = AppConfig.Instance().Db.SyncTables.Update(new SyncTables() { IdVendor = syncT.IdVendor, LastUpdateSync = DateTime.Now }, s => s.IdVendor == syncT.IdVendor);
                    return Request.CreateResponse(HttpStatusCode.OK, sync);

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Id");
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }



        }
    }
}
