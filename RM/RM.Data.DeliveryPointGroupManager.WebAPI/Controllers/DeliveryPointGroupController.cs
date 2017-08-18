using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
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
            this.deliveryPointGroupBusinessService = deliveryPointGroupBusinessService;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructors

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
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointGroups"))
            {
                string methodName = typeof(DeliveryPointGroupController) + "." + nameof(GetDeliveryPointGroups);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string deliveryGroups = deliveryPointGroupBusinessService.GetDeliveryPointGroups(bbox, this.CurrentUserUnit);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Ok(deliveryGroups);
            }
        }

        #endregion
    }
}