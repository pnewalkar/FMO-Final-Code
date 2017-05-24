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
        public IActionResult FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID)
        {
            var unitGuid = this.CurrentUserUnit;
            List<DeliveryRouteDTO> deliveryRoutes =  deliveryRouteBusinessService.FetchDeliveryRoute(operationStateID, deliveryScenarioID, unitGuid);
            return Ok(deliveryRoutes);
        }

        /// <summary>
        /// Gives list of Route Logs Status.
        /// </summary>
        /// <returns>List</returns>
        [HttpGet("RouteLogsStatus")]
        public IActionResult RouteLogsStatus()
        {
            List<ReferenceDataDTO> referenceDatas = deliveryRouteBusinessService.FetchRouteLogStatus();
            return Ok(referenceDatas);
        }

        /// <summary>
        /// Fetches Route Log Selection Type
        /// </summary>
        /// <returns>List</returns>
        [HttpGet("RouteLogsSelectionType")]
        public IActionResult RouteLogsSelectionType()
        {
            List<ReferenceDataDTO> referenceDatas = deliveryRouteBusinessService.FetchRouteLogSelectionType();
            return Ok(referenceDatas);
        }

        /// <summary>
        /// Fetches Delivery Scenario
        /// </summary>
        /// <param name="operationStateID">operation State ID</param>
        /// <param name="deliveryUnitID">delivery Unit ID</param>
        /// <returns></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("FetchDeliveryScenario")]
        public IActionResult FetchDeliveryScenario(Guid operationStateID, Guid deliveryUnitID)
        {
            List<ScenarioDTO> deliveryScenarios = deliveryRouteBusinessService.FetchDeliveryScenario(operationStateID, deliveryUnitID);
            return Ok(deliveryScenarios);
        }

        /// <summary>
        /// Gets the delivery route details.
        /// </summary>
        /// <param name="deliveryRouteId">The delivery route identifier.</param>
        /// <returns></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewRoutes)]
        [HttpGet("deliveryRoute/{deliveryRouteId}")]
        public async Task<IActionResult> GetDeliveryRouteDetailsForPdf(Guid deliveryRouteId)
        {
            try
            {
                DeliveryRouteDTO deliveryRoute = await deliveryRouteBusinessService.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteId, CurrentUserUnit);
                return Ok(deliveryRoute);
            }
            catch (AggregateException ae)
            {
                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Generates the delivery route log PDF.
        /// </summary>
        /// <param name="deliveryRouteDto">The delivery route dto.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("routelog/summary")]
        public async Task<IActionResult> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto)
        {
            try
            {
                byte[] bytes = await deliveryRouteBusinessService.GenerateRouteLog(deliveryRouteDto, CurrentUserUnit);
                return Ok(bytes);
            }
            catch (AggregateException ae)
            {
                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }
    }
}