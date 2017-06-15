using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.DataServices.Tests.DataService
{
    public class BlockSequenceDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IBlockSequenceDataService testCandidate;
        private BlockSequenceDTO blockSequenceDTO;
        private Mock<ILoggingHelper> mockLoggingHelper;

        [Test]
        public void AddBlockSequence()
        {
            var result = testCandidate.AddBlockSequence(blockSequenceDTO, new Guid("119DBBBB-03FB-489C-8C8D-F1085E0D2A13"));
            Assert.IsNotNull(result);
            // Assert.IsTrue(result);
        }

        protected override void OnSetup()
        {
            List<BlockSequence> blockSequence = new List<BlockSequence>() { };
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            blockSequenceDTO = new BlockSequenceDTO() { Block_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11") };

            var blocks = new List<Block>()
            {
                new Block() { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A16"), BlockType = "U" }
            };

            var deliveryRouteBlocks = new List<DeliveryRouteBlock>()
            {
                new DeliveryRouteBlock() { Block_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A16"), DeliveryRoute_GUID = new Guid("119DBBBB-03FB-489C-8C8D-F1085E0D2A13") }
            };

            var mockBlockSequenceDataServiceDataService = MockDbSet(blockSequence);
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<BlockSequence>()).Returns(mockBlockSequenceDataServiceDataService.Object);
            mockRMDBContext.Setup(x => x.BlockSequences).Returns(mockBlockSequenceDataServiceDataService.Object);
            mockBlockSequenceDataServiceDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockBlockSequenceDataServiceDataService.Object);
            mockRMDBContext.Setup(c => c.BlockSequences.AsNoTracking()).Returns(mockBlockSequenceDataServiceDataService.Object);

            var mockBlock = MockDbSet(blocks);
            mockRMDBContext.Setup(x => x.Set<Block>()).Returns(mockBlock.Object);
            mockRMDBContext.Setup(x => x.Blocks).Returns(mockBlock.Object);
            mockBlock.Setup(x => x.Include(It.IsAny<string>())).Returns(mockBlock.Object);
            mockRMDBContext.Setup(c => c.Blocks.AsNoTracking()).Returns(mockBlock.Object);

            var mockDeliveryRouteBlock = MockDbSet(deliveryRouteBlocks);
            mockRMDBContext.Setup(x => x.Set<DeliveryRouteBlock>()).Returns(mockDeliveryRouteBlock.Object);
            mockRMDBContext.Setup(x => x.DeliveryRouteBlocks).Returns(mockDeliveryRouteBlock.Object);
            mockDeliveryRouteBlock.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryRouteBlock.Object);
            mockRMDBContext.Setup(c => c.DeliveryRouteBlocks.AsNoTracking()).Returns(mockDeliveryRouteBlock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new BlockSequenceDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}