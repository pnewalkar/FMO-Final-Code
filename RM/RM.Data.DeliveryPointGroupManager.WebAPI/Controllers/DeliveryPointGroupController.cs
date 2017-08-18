using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

        private IDeliveryPointGroupBusinessService businessService = default(IDeliveryPointGroupBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        private int priority = LoggerTraceConstants.DeliveryPointGroupManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointGroupControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointGroupControllerMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public DeliveryPointGroupController(IDeliveryPointGroupBusinessService businessService, ILoggingHelper loggingHelper)
        {
            this.businessService = businessService;
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
    }
}