using System;
using System.Threading.Tasks;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Integration;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.BusinessService
{
    public class DeliveryPointGroupBusinessService : IDeliveryPointGroupBusinessService
    {
        #region Member Variables

        private IDeliveryPointGroupDataService deliveryPointGroupDataService = default(IDeliveryPointGroupDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IDeliveryPointGroupIntegrationService deliveryPointGroupIntegrationService = default(IDeliveryPointGroupIntegrationService);

        private int priority = LoggerTraceConstants.DeliveryPointGroupManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointGroupBusinessServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointGroupBusinessServiceMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryPointGroupBusinessService"/> class.
        /// </summary>
        public DeliveryPointGroupBusinessService(
            IDeliveryPointGroupDataService deliveryPointGroupDataService,
            ILoggingHelper loggingHelper,
            IDeliveryPointGroupIntegrationService deliveryPointGroupIntegrationService)
        {
            this.deliveryPointGroupDataService = deliveryPointGroupDataService;
            this.loggingHelper = loggingHelper;
            this.deliveryPointGroupIntegrationService = deliveryPointGroupIntegrationService;
        }

        #endregion Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryPointGroupDto">UI Dto to create Delivery group</param>
        /// <returns></returns>
        public Task<DeliveryPointGroupDTO> CreateDeliveryPointGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateDeliveryPointGroup"))
            {
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(CreateDeliveryPointGroup);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointGroupDto == null)
                {
                    throw new ArgumentNullException(nameof(deliveryPointGroupDto), string.Format(ErrorConstants.Err_ArgumentmentNullException, deliveryPointGroupDto));
                }

                deliveryPointGroupDataService.CreateDeliveryGroup();
                throw new NotImplementedException();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return null;
            }            
        }
    }
}