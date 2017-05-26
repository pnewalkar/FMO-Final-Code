using System;
using System.Collections.Generic;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Moq;
using NUnit.Framework;

namespace Fmo.DataServices.Tests.Repositories
{
    public class BlockSequenceRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IBlockSequenceRepository testCandidate;
        private BlockSequenceDTO blockSequenceDTO;

        [Test]
        public void AddBlockSequence()
        {
            bool result = testCandidate.AddBlockSequence(blockSequenceDTO, new Guid("119DBBBB-03FB-489C-8C8D-F1085E0D2A13"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        protected override void OnSetup()
        {
            List<BlockSequence> blockSequence = new List<BlockSequence>() { };

            blockSequenceDTO = new BlockSequenceDTO() { Block_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11") };

            var blocks = new List<Block>()
            {
                new Block() { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A16"), BlockType = "U" }
            };

            var deliveryRouteBlocks = new List<DeliveryRouteBlock>()
            {
                new DeliveryRouteBlock() { Block_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A16"), DeliveryRoute_GUID = new Guid("119DBBBB-03FB-489C-8C8D-F1085E0D2A13") }
            };

            var mockBlockSequenceRepositoryRepository = MockDbSet(blockSequence);
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<BlockSequence>()).Returns(mockBlockSequenceRepositoryRepository.Object);
            mockFmoDbContext.Setup(x => x.BlockSequences).Returns(mockBlockSequenceRepositoryRepository.Object);
            mockBlockSequenceRepositoryRepository.Setup(x => x.Include(It.IsAny<string>())).Returns(mockBlockSequenceRepositoryRepository.Object);
            mockFmoDbContext.Setup(c => c.BlockSequences.AsNoTracking()).Returns(mockBlockSequenceRepositoryRepository.Object);

            var mockBlock = MockDbSet(blocks);
            mockFmoDbContext.Setup(x => x.Set<Block>()).Returns(mockBlock.Object);
            mockFmoDbContext.Setup(x => x.Blocks).Returns(mockBlock.Object);
            mockBlock.Setup(x => x.Include(It.IsAny<string>())).Returns(mockBlock.Object);
            mockFmoDbContext.Setup(c => c.Blocks.AsNoTracking()).Returns(mockBlock.Object);

            var mockDeliveryRouteBlock = MockDbSet(deliveryRouteBlocks);
            mockFmoDbContext.Setup(x => x.Set<DeliveryRouteBlock>()).Returns(mockDeliveryRouteBlock.Object);
            mockFmoDbContext.Setup(x => x.DeliveryRouteBlocks).Returns(mockDeliveryRouteBlock.Object);
            mockDeliveryRouteBlock.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryRouteBlock.Object);
            mockFmoDbContext.Setup(c => c.DeliveryRouteBlocks.AsNoTracking()).Returns(mockDeliveryRouteBlock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new BlockSequenceRepository(mockDatabaseFactory.Object);
        }
    }
}