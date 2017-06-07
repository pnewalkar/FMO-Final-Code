using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.ResourceFile;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class BlockSequenceDataService : DataServiceBase<BlockSequence, RMDBContext>, IBlockSequenceDataService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSequenceDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public BlockSequenceDataService(IDatabaseFactory<RMDBContext> databaseFactory)
            : base(databaseFactory)
        {
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
                var block_Guid = await (from dr in DataContext.DeliveryRouteBlocks.AsNoTracking()
                                        join b in DataContext.Blocks.AsNoTracking() on dr.Block_GUID equals b.ID
                                        where b.BlockType == Constants.UnSequenced && dr.DeliveryRoute_GUID == deliveryRouteId
                                        select b.ID).SingleOrDefaultAsync();

                BlockSequence blockSequenceEntity = GenericMapper.Map<BlockSequenceDTO, BlockSequence>(blockSequenceDTO);
                blockSequenceEntity.Block_GUID = block_Guid;
                DataContext.BlockSequences.Add(blockSequenceEntity);
                DataContext.SaveChanges();
                isBlockSequencInserted = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbConcurrencyException(ErrorMessageIds.Err_Concurrency);
            }

            return isBlockSequencInserted;
        }
    }
}