﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Interface;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods used to fetch Delivery Points.
    /// </summary>
    [Route("api/deliveryPoints")]
    public class DeliveryPointsController : Controller
    {
        private IDeliveryPointBusinessService businessService = default(IDeliveryPointBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public DeliveryPointsController(IDeliveryPointBusinessService businessService, ILoggingHelper loggingHelper)
        {
            this.businessService = businessService;
            this.loggingHelper = loggingHelper;
        }

        public JsonResult Get()
        {
            return Json("");
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
            return Json(businessService.GetDeliveryPoints(boundaryBox));           
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

    }   
}
