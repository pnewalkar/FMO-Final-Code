using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class RouteLogController : FmoBaseController
    {
        protected IDeliveryRouteBusinessService deliveryRouteBusinessService = default(IDeliveryRouteBusinessService);

        public RouteLogController(IDeliveryRouteBusinessService _deliveryRouteBusinessService)
        {
            deliveryRouteBusinessService = _deliveryRouteBusinessService;
        }

        /// <summary>
        /// Fetches Delivery Route
        /// </summary>
        /// <param name="operationStateID"> operationState ID</param>
        /// <param name="deliveryScenarioID">deliveryScenario ID</param>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("FetchDeliveryRoute")]
        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID)
        {
            var unitGuid = this.CurrentUserUnit;
            return deliveryRouteBusinessService.FetchDeliveryRoute(operationStateID, deliveryScenarioID, unitGuid);
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

        /// <summary>
        /// Fetch Delivery Route details by route GUID
        /// </summary>
        /// <returns></returns>
        [HttpGet("FetchRouteDetailsByGUID")]
        public DeliveryRouteDTO FetchRouteDetailsByGUID(Guid routeId)
        {
            return new DeliveryRouteDTO
            {
                RouteName = "CedarCraft Road",
                RouteNumber = "6",
                Totaltime = "1.50 mins",
                Aliases = 4,
                Blocks = 6,
                PairedRoute = "2001",
                Method = "Shared Van",
                DPs = 20,
                BusinessDPs = 12,
                ResidentialDPs = 8
            };
        }
    }
}