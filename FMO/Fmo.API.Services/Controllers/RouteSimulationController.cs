using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;
using Fmo.Helpers;

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
        /// <param name="fields">The fields to be returned</param>
        /// <returns>List</returns>
        [HttpGet("FetchDeliveryRoute")]
        public IActionResult FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID, [FromQuery]string fields)
        {
            var unitGuid = this.CurrentUserUnit;
            List<object> deliveryRoutesList = null;
            List<DeliveryRouteDTO> deliveryRoutes = deliveryRouteBusinessService.FetchDeliveryRoute(operationStateID, deliveryScenarioID, unitGuid);
            CreateSummaryObject<DeliveryRouteDTO> createSummary = new CreateSummaryObject<DeliveryRouteDTO>();

            if (!string.IsNullOrEmpty(fields))
            {
                deliveryRoutesList = createSummary.SummarisePropertiesForList(deliveryRoutes, fields);
            }
            return Ok(deliveryRoutesList);
        }

        /// <summary>
        /// Fetches Route Log Status
        /// </summary>
        /// <returns>List</returns>
        [HttpGet("RouteLogsStatus")]
        public IActionResult RouteLogsStatus()
        {
            List<ReferenceDataDTO> referenceDatas = deliveryRouteBusinessService.FetchRouteLogStatus();
            return Ok(referenceDatas);
        }

        /// <summary>
        /// Fetches Delivery Scenario
        /// </summary>
        /// <param name="operationStateID">operation State ID</param>
        /// <param name="deliveryUnitID">delivery Unit ID</param>
        /// <param name="fields">The fields to be returned</param>
        /// <returns></returns>
        [HttpGet("FetchDeliveryScenario")]
        public IActionResult FetchDeliveryScenario(Guid operationStateID, Guid deliveryUnitID, [FromQuery]string fields)
        {
            List<object> deliveryScenerioList = null;
            List<ScenarioDTO> deliveryScenerio = deliveryRouteBusinessService.FetchDeliveryScenario(operationStateID, deliveryUnitID);
            CreateSummaryObject<ScenarioDTO> createSummary = new CreateSummaryObject<ScenarioDTO>();

            if (!string.IsNullOrEmpty(fields))
            {
                deliveryScenerioList = createSummary.SummarisePropertiesForList(deliveryScenerio, fields);
            }
            return Ok(deliveryScenerioList);
        }
    }
}