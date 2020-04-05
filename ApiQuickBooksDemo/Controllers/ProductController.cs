using ApiCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiQuickBooksDemo.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public HttpResponseMessage Get()
        {
            try
            {

                var products = AppConfig.Instance().Db.Products.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, products);
             



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
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


                    var lastUpdateSync = AppConfig.Instance().Db.GetLastUpdateDate(Convert.ToInt32(id), "Products");
                    var products = AppConfig.Instance().Db.Products.Get(c => c.LastUpdate > lastUpdateSync);
                    if (products.Count() > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, products);


                    return Request.CreateResponse(HttpStatusCode.NoContent, products);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,"Invalid Id");
                }



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }


        }
    }
}
