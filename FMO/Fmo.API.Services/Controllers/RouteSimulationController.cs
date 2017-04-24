using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class RouteSimulationController : FmoBaseController
    {
        protected IDeliveryRouteBusinessService deliveryRouteBusinessService = default(IDeliveryRouteBusinessService);

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteSimulationController"/> class and other classes.
        /// </summary>
        /// <param name="_deliveryRouteBusinessService">IDeliveryRouteBusinessService reference</param>
        public RouteSimulationController(IDeliveryRouteBusinessService _deliveryRouteBusinessService)
        {
            deliveryRouteBusinessService = _deliveryRouteBusinessService;
        }

        /// <summary>
        /// Fetches Delivery Route
        /// </summary>
        /// <param name="operationStateID">operation State ID</param>
        /// <param name="deliveryScenarioID">delivery Scenario ID</param>
        /// <returns>List</returns>
        [HttpGet("FetchDeliveryRoute")]
        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID)
        {
            var unitGuid = this.CurrentUserUnit;
            return deliveryRouteBusinessService.FetchDeliveryRoute(operationStateID, deliveryScenarioID, unitGuid);
        }

        /// <summary>
        /// Fetches Route Log Status
        /// </summary>
        /// <returns>List</returns>
        [HttpGet("RouteLogsStatus")]
        public List<ReferenceDataDTO> RouteLogsStatus()
        {
            return deliveryRouteBusinessService.FetchRouteLogStatus();
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