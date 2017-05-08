using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.Common.Interface;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods used to fetch Delivery Points.
    /// </summary>
    [Route("api/deliveryPoints")]
    public class DeliveryPointsController : FmoBaseController
    {
        private IDeliveryPointBusinessService businessService = default(IDeliveryPointBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public DeliveryPointsController(IDeliveryPointBusinessService businessService, ILoggingHelper loggingHelper)
        {
            this.businessService = businessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        ///This method is used to Get Delivery Point Object.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>Json Result of Delivery Points</returns>
        [Route("GetDeliveryPoints")]
        [HttpGet]
        public JsonResult GetDeliveryPoints(string boundaryBox)
        {
            return Json(businessService.GetDeliveryPoints(boundaryBox, CurrentUserUnit));
        }

        /// <summary>
        /// Get coordinates of the delivery point by UDPRN
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        [Route("GetDeliveryPointByUDPRN")]
        [HttpGet]
        public object GetDeliveryPointByUDPRN(int udprn)
        {
            return businessService.GetDeliveryPointByUDPRN(udprn);
        }

        /// <summary>
        /// Get mapped address location of delivery point by UDPRN
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        [Route("GetAddressLocationByUDPRN")]
        [HttpGet]
        public JsonResult GetDetailDeliveryPointByUDPRN(int udprn)
        {
            return Json(businessService.GetDetailDeliveryPointByUDPRN(udprn));
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDTO</param>
        /// <returns></returns>
        [Route("CreateDeliveryPoint")]
        [HttpPost]
        public string CreateDeliveryPoint([FromBody]AddDeliveryPointDTO deliveryPointDTO)
        {
            return businessService.CreateDeliveryPoint(deliveryPointDTO);
        }
    }
}