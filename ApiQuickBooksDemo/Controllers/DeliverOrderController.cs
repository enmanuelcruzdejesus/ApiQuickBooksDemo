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
    public class DeliverOrderController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {

                var deliveryOrders = AppConfig.Instance().Db.DeliveryOrders.GetLoadRerefence();
                if (deliveryOrders.Count() > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, deliveryOrders);


                return Request.CreateResponse(HttpStatusCode.NoContent,"");





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

                    var lastUpdateSync = AppConfig.Instance().Db.GetLastUpdateDate(Convert.ToInt32(id), "DeliveryOrders");
                    if (lastUpdateSync == DateTime.MinValue)
                        return Request.CreateResponse(HttpStatusCode.BadRequest,string.Format("DeliveryId = {0} does not exist!", id));

                    var deliveryOrders = AppConfig.Instance().Db.DeliveryOrders.Get(d => d.IdDelivery == Convert.ToInt32(id) && d.LastUpdate > lastUpdateSync);
                    if (deliveryOrders.Count() > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, deliveryOrders);


                    return Request.CreateResponse(HttpStatusCode.NoContent, "");

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("DeliveryId = {0} does not exist!", id));
                }



            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
;

        }


        [HttpPut]
        public HttpResponseMessage Put([FromBody] DeliveryOrders delivery)
        {
            try
            {
                if (delivery != null)
                {
                    delivery.CreatedDate = DateTime.Now;
                    delivery.LastUpdate = DateTime.Now;
                    var s = AppConfig.Instance().Db.DeliveryOrders.Update(delivery);
                    return Request.CreateResponse(HttpStatusCode.OK, delivery);


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



        }



        [HttpPost]
        [Route("CreateDeliveryOrder")]
        public HttpResponseMessage Post(DeliveryOrders delivery)
        {
            try
            {

                if (delivery != null)
                {
                    delivery.CreatedDate = DateTime.Now;
                    delivery.LastUpdate = DateTime.Now;

                    var deliveryOrder = AppConfig.Instance().Db.DeliveryOrders.Insert(delivery);
                    return Request.CreateResponse(HttpStatusCode.OK, delivery);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Id");
                }
            }catch(Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        [HttpPost]
        [Route("Upload")]
        public HttpResponseMessage Post(List<DeliveryOrders> deliveryOrders)
        {

            try
            {
                if (deliveryOrders != null)
                {
                    deliveryOrders.ForEach((x) =>
                    {
                        x.LastUpdate = DateTime.Now;
                    });

                    var deliveryOrder = AppConfig.Instance().Db.DeliveryOrders.BulkMerge(deliveryOrders);
                    return Request.CreateResponse(HttpStatusCode.OK, deliveryOrder);

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "parameter is null");
                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }





    }
}
