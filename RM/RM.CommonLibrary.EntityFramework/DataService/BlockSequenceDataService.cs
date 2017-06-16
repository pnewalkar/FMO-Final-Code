using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.ResourceFile;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class BlockSequenceDataService : DataServiceBase<BlockSequence, RMDBContext>, IBlockSequenceDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSequenceDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public BlockSequenceDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Save block sequence in database
        /// </summary>
        /// <param name="blockSequenceDTO">blockSequenceDTO</param>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <returns>bool</returns>
        public async Task<bool> AddBlockSequence(BlockSequenceDTO blockSequenceDTO, Guid deliveryRouteId)
        {
            bool isBlockSequencInserted = false;
            try
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                using (loggingHelper.RMTraceManager.StartTrace(methodName))
                {
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    var block_Guid = await (from dr in DataContext.DeliveryRouteBlocks.AsNoTracking()
                                            join b in DataContext.Blocks.AsNoTracking() on dr.Block_GUID equals b.ID
                                            where b.BlockType == Constants.UnSequenced && dr.DeliveryRoute_GUID == deliveryRouteId
                                            select b.ID).SingleOrDefaultAsync();

                    BlockSequence blockSequenceEntity = GenericMapper.Map<BlockSequenceDTO, BlockSequence>(blockSequenceDTO);
                    blockSequenceEntity.Block_GUID = block_Guid;
                    DataContext.BlockSequences.Add(blockSequenceEntity);
                    DataContext.SaveChanges();
                    isBlockSequencInserted = true;
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbConcurrencyException(ErrorConstants.Err_Concurrency);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("creating block sequence for selected delivery route")));
            }

            return isBlockSequencInserted;
        }
    }
}