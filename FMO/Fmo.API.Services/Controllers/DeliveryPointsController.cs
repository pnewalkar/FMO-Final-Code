using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.Common.Interface;
using Fmo.DTO;
using Fmo.DTO.Model;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = UserAccessFunctionsConstants.ViewDeliveryPoints)]
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
        [Authorize(Roles = UserAccessFunctionsConstants.ViewDeliveryPoints)]
        [Route("GetDeliveryPointByUDPRN")]
        [HttpGet]
        public IActionResult GetDeliveryPointByUDPRN(int udprn)
        {
            var geoJsonfeature = businessService.GetDeliveryPointByUDPRN(udprn);
            return Ok(geoJsonfeature);
        }

        /// <summary>
        /// Get mapped address location of delivery point by UDPRN
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewDeliveryPoints)]
        [Route("GetAddressLocationByUDPRN")]
        [HttpGet]
        public JsonResult GetDetailDeliveryPointByUDPRN(int udprn)
        {
            return Json(businessService.GetDetailDeliveryPointByUDPRN(udprn));
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto</param>
        /// <returns></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [Route("CreateDeliveryPoint")]
        //[Route("CreateDeliveryPoint")]
        [HttpPost]
        public IActionResult CreateDeliveryPoint([FromBody]AddDeliveryPointDTO deliveryPointDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateDeliveryPointModelDTO createDeliveryPointModelDto = businessService.CreateDeliveryPoint(deliveryPointDto);
            return Ok(createDeliveryPointModelDto);
        }

        /// <summary>
        /// Update delivery point
        /// </summary>
        /// <param name="deliveryPointModelDto">deliveryPointDTO</param>
        /// <returns></returns>
        [Route("UpdateDeliveryPoint")]
        [HttpPut]
        public async Task<IActionResult> UpdateDeliveryPoint([FromBody] DeliveryPointModelDTO deliveryPointModelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                UpdateDeliveryPointModelDTO updateDeliveryPoint = await businessService.UpdateDeliveryPointLocation(deliveryPointModelDto);
                return Ok(updateDeliveryPoint);
            }
            catch (AggregateException ae)
            {
                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }
        [Route("GetRouteForDeliveryPoint")]
        [HttpGet]
        public List<KeyValuePair<string, string>> GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            return businessService.GetRouteForDeliveryPoint(deliveryPointId);
        }

    }
}