using System;
using System.Linq;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryRoute.WebAPI.Entities;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    public class BlockSequenceDataService : DataServiceBase<BlockSequence, RouteDBContext>, IBlockSequenceDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSequenceDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public BlockSequenceDataService(IDatabaseFactory<RouteDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// method to save delivery point and selected route mapping in block sequence table
        /// </summary>
        /// <param name="routeId">selected route id</param>
        /// <param name="deliveryPointId">Delivery point unique id</param>
        public void SaveDeliveryPointRouteMapping(Guid routeId, Guid deliveryPointId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.SaveDeliveryPointRouteMapping"))
            {
                string methodName = typeof(DeliveryRouteDataService) + "." + nameof(SaveDeliveryPointRouteMapping);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.BlockSequenceDataServiceMethodEntryEventId);

                Guid blockId = DataContext.Routes.AsNoTracking().Where(n => n.ID == routeId).SingleOrDefault().UnSequencedBlockID.Value;

                DataContext.BlockSequences.Add(new BlockSequence
                {
                    ID = Guid.NewGuid(),
                    BlockID = blockId,
                    LocationID = deliveryPointId,
                    StartDateTime = DateTime.Now,
                    RowCreateDateTime = DateTime.Now
                });

                DataContext.SaveChanges();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.BlockSequenceDataServiceMethodExitEventId);
            }
        }
    }
}