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
    }
}