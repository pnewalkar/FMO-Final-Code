using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService
{
    public class DeliveryPointGroupDataService : DataServiceBase<SupportingDeliveryPoint, DeliveryPointGroupDBContext>, IDeliveryPointGroupDataService
    {
        private int priority = LoggerTraceConstants.DeliveryPointGroupManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointGroupDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointGroupDataServiceMethodExitEventId;

        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initialises new instance of <see cref="DeliveryPointGroupDataService"/> that contains data methods related to delivery point group.
        /// </summary>
        /// <param name="databaseFactory"></param>
        /// <param name="loggingHelper">Helper class for logging related functions.</param>
        public DeliveryPointGroupDataService(IDatabaseFactory<DeliveryPointGroupDBContext> databaseFactory, ILoggingHelper loggingHelper)
        : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }
    }
}