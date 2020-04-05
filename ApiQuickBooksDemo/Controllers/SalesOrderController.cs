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
    [RoutePrefix("api/SalesOrder")]
    public class SalesOrderController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public HttpResponseMessage Get()
        {
            try
            {

                var orders = AppConfig.Instance().Db.SalesOrders.GetLoadRerefence();

                if (orders.Count() > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, orders);

                return Request.CreateResponse(HttpStatusCode.NoContent,orders);



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
;

        }


        [HttpPost]
        [Route("Upload")]
        public HttpResponseMessage Post([FromBody] List<SalesOrders> orders)
        {
            try
            {


                if (orders != null)
                {
                    orders.ForEach((x) =>
                    {
                        x.LastUpdate = DateTime.Now;
                    });


                    var ord = AppConfig.Instance().Db.SalesOrders.BulkMerge(orders);
                    foreach (var item in orders)
                    {
                        var detail = item.SalesOrdersDetails;
                        detail.ForEach(a => a.IdOrder = a.SalesOrdersId);
                        AppConfig.Instance().Db.SalesDetails.BulkMerge(detail);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, orders);

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
