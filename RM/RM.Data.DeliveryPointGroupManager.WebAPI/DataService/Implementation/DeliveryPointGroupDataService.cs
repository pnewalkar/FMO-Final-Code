using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities;
using System.Linq;

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

        public DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"DataService.{nameof(UpdateDeliveryGroup)}"))
            {
                //fetch all delivery point locations
                var existingDeliveryLocation = DataContext.LocationRelationships.Where(x => x.RelatedLocationID == deliveryPointGroupDto.ID);
            }

            return deliveryPointGroupDto;
        }
    }
}