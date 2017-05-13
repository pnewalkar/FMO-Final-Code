using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
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

        public RouteLogController(IDeliveryRouteBusinessService deliveryRouteBusinessService)
        {
            this.deliveryRouteBusinessService = deliveryRouteBusinessService;
        }

        /// <summary>
        /// Fetches Delivery Route
        /// </summary>
        /// <param name="operationStateID"> operationState ID</param>
        /// <param name="deliveryScenarioID">deliveryScenario ID</param>
        /// <returns>List</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
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
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("FetchDeliveryScenario")]
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryUnitID)
        {
            return deliveryRouteBusinessService.FetchDeliveryScenario(operationStateID, deliveryUnitID);
        }

        /// <summary>
        /// Gets the delivery route details.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryRoute/{deliveryRouteId}")]
        public async Task<DeliveryRouteDTO> GetDeliveryRouteDetailsForPdf(Guid deliveryRouteId)
        {
            return await deliveryRouteBusinessService.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteId, CurrentUserUnit);
        }

        /// <summary>
        /// Generates the delivery route log PDF.
        /// </summary>
        /// <param name="deliveryRouteDto">The delivery route dto.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("routelog/summary")]
        public async Task<byte[]> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto)
        {
            return await deliveryRouteBusinessService.GenerateRouteLog(deliveryRouteDto, CurrentUserUnit);
        }
    }
}