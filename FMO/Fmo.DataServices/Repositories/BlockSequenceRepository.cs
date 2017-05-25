using System;
using System.Linq;
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
        public void AddBlockSequence(BlockSequenceDTO blockSequenceDTO, Guid deliveryRouteId)
        {
            var block_Guid = (from dr in DataContext.DeliveryRouteBlocks.AsNoTracking()
                              join b in DataContext.Blocks.AsNoTracking() on dr.Block_GUID equals b.ID
                              where b.BlockType == "U" && dr.DeliveryRoute_GUID == deliveryRouteId
                              select b.ID).SingleOrDefault();

            BlockSequence blockSequenceEntity = GenericMapper.Map<BlockSequenceDTO, BlockSequence>(blockSequenceDTO);
            blockSequenceEntity.Block_GUID = block_Guid;
            DataContext.BlockSequences.Add(blockSequenceEntity);
            DataContext.SaveChanges();
        }
    }
}