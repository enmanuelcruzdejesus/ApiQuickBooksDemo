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
    public class VendorVisitController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {

                var vendorVisits = AppConfig.Instance().Db.VendorVisits.GetAll();

                return Request.CreateResponse(HttpStatusCode.OK, vendorVisits);



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }



        [HttpGet]
        public HttpResponseMessage Get(string id)
        {
            try
            {

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrWhiteSpace(id))
                {

                    
                    var lastUpdateSync = AppConfig.Instance().Db.GetLastUpdateDate(Convert.ToInt32(id), "VendorVisits");
                    var vendorVisits = AppConfig.Instance().Db.VendorVisits.Get(v => v.IdVendor == Convert.ToInt32(id) && v.LastUpdate > lastUpdateSync);
                    if (vendorVisits.Count() > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, vendorVisits);


                    return Request.CreateResponse(HttpStatusCode.NoContent, "");

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Id");
                }



            }
            catch (Exception ex)
            {

                return  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
;

        }


        [HttpPut]
        public HttpResponseMessage Put([FromBody] VendorVisits visit)
        {
            try
            {
                if (visit != null)
                {
                    visit.CreatedDate = DateTime.Now;
                    visit.LastUpdate = DateTime.Now;
                    var vendorVisits = AppConfig.Instance().Db.VendorVisits.Update(visit);
                    return Request.CreateResponse(HttpStatusCode.OK, vendorVisits);

                }
                else
                {
                    return   Request.CreateErrorResponse(HttpStatusCode.BadRequest, "paramter is null");
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }



        }



        [HttpPost]
        public HttpResponseMessage Post(VendorVisits visit)
        {

            try
            {

                if (visit != null)
                {
                    visit.CreatedDate = DateTime.Now;
                    visit.LastUpdate = DateTime.Now;

                    var vendorVisits = AppConfig.Instance().Db.VendorVisits.Insert(visit);
                    return Request.CreateResponse(HttpStatusCode.OK, vendorVisits);

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
