﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.DTO;
using Fmo.BusinessServices.Interfaces;
using System.Security.Claims;
using System.Threading;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    //[EnableCors("AllowCors")]
    [Route("api/[controller]")]
    public class RouteLogController : Controller
    {
        protected IDeliveryRouteBusinessService deliveryRouteBusinessService = default(IDeliveryRouteBusinessService);

        public RouteLogController(IDeliveryRouteBusinessService _deliveryRouteBusinessService)
        {
            deliveryRouteBusinessService = _deliveryRouteBusinessService;
        }

        /// <summary>
        /// Fetches Delivery Unit
        /// </summary>
        /// <returns>List</returns>
        // [Authorize]
        [HttpGet("DeliveryUnit")]
        public List<DeliveryUnitLocationDTO> DeliveryUnit()
        {
            var u = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)
                               .Select(c => c.Value).SingleOrDefault();

            var g = User.Claims.Where(c => c.Type == ClaimTypes.UserData)
                               .Select(c => c.Value).SingleOrDefault();

            return deliveryRouteBusinessService.FetchDeliveryUnit();
        }

        /// <summary>
        /// Fetches Delivery Route
        /// </summary>
        /// <param name="operationStateID"> operationState ID</param>
        /// <param name="deliveryScenarioID">deliveryScenario ID</param>
        /// <returns>List</returns>
        [HttpGet("FetchDeliveryRoute")]
        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID)
        {
            return deliveryRouteBusinessService.FetchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        /// <summary>
        /// Gives list of Route Logs Status.
        /// </summary>
        /// <returns>List</returns>
        [HttpGet("RouteLogsStatus")]
        public List<ReferenceDataDTO> RouteLogsStatus()
        {
            return deliveryRouteBusinessService.FetchRouteLogStatus();
        }

        /// <summary>
        /// Fetches Route Log Selection Type
        /// </summary>
        /// <returns>List</returns>
        [HttpGet("RouteLogsSelectionType")]
        public List<ReferenceDataDTO> RouteLogsSelectionType()
        {
            return deliveryRouteBusinessService.FetchRouteLogSelectionType();
        }

        /// <summary>
        /// Fetches Delivery Scenario
        /// </summary>
        /// <param name="operationStateID">operation State ID</param>
        /// <param name="deliveryUnitID">delivery Unit ID</param>
        /// <returns></returns>
        [HttpGet("FetchDeliveryScenario")]
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryUnitID)
        {
            return deliveryRouteBusinessService.FetchDeliveryScenario(operationStateID, deliveryUnitID);
        }
    }
}
