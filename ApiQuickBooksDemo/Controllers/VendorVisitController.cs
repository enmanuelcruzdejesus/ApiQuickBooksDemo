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
    [RoutePrefix("api/VendorVisit")]
    public class VendorVisitController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]

        public HttpResponseMessage Get()
        {
            try
            {
                var db = AppConfig.Instance().Db;
                var vendorVisits = db.VendorVisits.GetAll();
                foreach (var item in vendorVisits)
                {
                    var vendor = db.Users.Get(u => u.IdUser == item.IdVendor).FirstOrDefault();
                    item.Vendor = vendor;
                }

                return Request.CreateResponse(HttpStatusCode.OK, vendorVisits);



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }



        [HttpGet]
        [Route("GetById/{id}")]
        public HttpResponseMessage Get(string id)
        {
            try
            {

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrWhiteSpace(id))
                {


                    var vendorVisits = AppConfig.Instance().Db.VendorVisits.Get(v => v.IdVendorVisit == Convert.ToInt32(id));
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

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
;

        }



        [HttpGet]
        [Route("Download/{id}")]
        public HttpResponseMessage DownLoad(string id)
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
        [Route("Update")]
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
        [Route("Create")]
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
