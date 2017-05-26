using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Fmo.Common.Constants;
using Fmo.Common.ExceptionManagement;
using Fmo.Common.ResourceFile;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class BlockSequenceRepository : RepositoryBase<Scenario, FMODBContext>, IBlockSequenceRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockSequenceRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public BlockSequenceRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Save block sequence in database
        /// </summary>
        /// <param name="blockSequenceDTO">blockSequenceDTO</param>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <returns>bool</returns>
        public bool AddBlockSequence(BlockSequenceDTO blockSequenceDTO, Guid deliveryRouteId)
        {
            bool isBlockSequencInserted = false;
            try
            {
                var block_Guid = (from dr in DataContext.DeliveryRouteBlocks.AsNoTracking()
                                  join b in DataContext.Blocks.AsNoTracking() on dr.Block_GUID equals b.ID
                                  where b.BlockType == Constants.UnSequenced && dr.DeliveryRoute_GUID == deliveryRouteId
                                  select b.ID).SingleOrDefault();

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