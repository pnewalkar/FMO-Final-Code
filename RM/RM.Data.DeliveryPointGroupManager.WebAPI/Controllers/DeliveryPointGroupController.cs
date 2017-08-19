using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.BusinessService;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Controllers
{
    [Route("api/DeliveryPointGroupManager")]
    public class DeliveryPointGroupController : RMBaseController
    {
        #region Member Variables

        private IDeliveryPointGroupBusinessService deliveryPointGroupBusinessService = default(IDeliveryPointGroupBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        private int priority = LoggerTraceConstants.DeliveryPointGroupManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointGroupControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointGroupControllerMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public DeliveryPointGroupController(IDeliveryPointGroupBusinessService deliveryPointGroupBusinessService, ILoggingHelper loggingHelper)
        {
            // Validate the arguments
            if (deliveryPointGroupBusinessService == null) { throw new ArgumentNullException(nameof(deliveryPointGroupBusinessService)); }
            if (loggingHelper == null) { throw new ArgumentNullException(nameof(loggingHelper)); }

            this.deliveryPointGroupBusinessService = deliveryPointGroupBusinessService;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Create delivery point group.
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto</param>
        /// <returns>createDeliveryPointModelDTO</returns>
        [Route("CreateDeliveryGroup")]
        [HttpPost]
        public async Task<IActionResult> CreateDeliveryGroup([FromBody]DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.AddDeliveryPoint"))
                {
                    DeliveryPointGroupDTO createDeliveryPointGroupModelDTO = null;
                    string methodName = typeof(DeliveryPointGroupController) + "." + nameof(CreateDeliveryGroup);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    createDeliveryPointGroupModelDTO = await businessService.CreateDeliveryPointGroup(deliveryPointGroupDto);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return Ok(createDeliveryPointGroupModelDTO);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        #endregion Methods

        #region Methods

        /// <summary>
        /// This method is used to fetch Delivery Point Groups.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string</param>
        /// <returns>GeoJson string of Delivery Point Groups</returns>
        [Authorize]
        [HttpGet("DeliveryPointGroups")]
        public IActionResult GetDeliveryPointGroups(string bbox)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"WebService.{nameof(GetDeliveryPointGroups)}"))
            {
                string methodName = typeof(DeliveryPointGroupController) + "." + nameof(GetDeliveryPointGroups);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string deliveryGroups = deliveryPointGroupBusinessService.GetDeliveryPointGroups(bbox, this.CurrentUserUnit);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Ok(deliveryGroups);
            }
        }

        /// <summary>
        /// This method updates delivery point group details.
        /// </summary>
        /// <param name="deliveryPointGroupDto">The object containing delivery point group details.</param>
        /// <returns>updateDeliveryPointModelDTO</returns>
        [Route("deliverypointgroup")]
        [HttpPut]
        public async Task<IActionResult> UpdateDeliveryGroup([FromBody] DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace($"WebService.{nameof(UpdateDeliveryGroup)}"))
                {
                    string methodName = typeof(DeliveryPointGroupController) + "." + nameof(UpdateDeliveryGroup);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    // validate the method argument.
                    if (deliveryPointGroupDto == null) { throw new ArgumentNullException(nameof(deliveryPointGroupDto)); }

                    // validate the model state.
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    deliveryPointGroupDto = deliveryPointGroupBusinessService.UpdateDeliveryGroup(deliveryPointGroupDto);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return Ok(deliveryPointGroupDto);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        #endregion Methods
    }
}