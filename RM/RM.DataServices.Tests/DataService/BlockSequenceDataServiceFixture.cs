using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
            Assert.IsTrue(result.Result);
        }

        protected override void OnSetup()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

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

            var mockBlockSequenceDataServiceEnumerable = new DbAsyncEnumerable<BlockSequence>(blockSequence);
            var mockBlockSequenceDataServiceDataService = MockDbSet(blockSequence);
            mockBlockSequenceDataServiceDataService.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockBlockSequenceDataServiceEnumerable.AsQueryable().Provider);
            mockBlockSequenceDataServiceDataService.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockBlockSequenceDataServiceEnumerable.AsQueryable().Expression);
            mockBlockSequenceDataServiceDataService.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockBlockSequenceDataServiceEnumerable.AsQueryable().ElementType);
            mockBlockSequenceDataServiceDataService.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<BlockSequence>)mockBlockSequenceDataServiceEnumerable).GetAsyncEnumerator());

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<BlockSequence>()).Returns(mockBlockSequenceDataServiceDataService.Object);
            mockRMDBContext.Setup(x => x.BlockSequences).Returns(mockBlockSequenceDataServiceDataService.Object);
            mockBlockSequenceDataServiceDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockBlockSequenceDataServiceDataService.Object);
            mockRMDBContext.Setup(c => c.BlockSequences.AsNoTracking()).Returns(mockBlockSequenceDataServiceDataService.Object);

            var mockBlockEnumerable = new DbAsyncEnumerable<Block>(blocks);
            var mockBlock = MockDbSet(blocks);
            mockBlock.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockBlockEnumerable.AsQueryable().Provider);
            mockBlock.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockBlockEnumerable.AsQueryable().Expression);
            mockBlock.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockBlockEnumerable.AsQueryable().ElementType);
            mockBlock.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Block>)mockBlockEnumerable).GetAsyncEnumerator());

            mockRMDBContext.Setup(x => x.Set<Block>()).Returns(mockBlock.Object);
            mockRMDBContext.Setup(x => x.Blocks).Returns(mockBlock.Object);
            mockBlock.Setup(x => x.Include(It.IsAny<string>())).Returns(mockBlock.Object);
            mockRMDBContext.Setup(c => c.Blocks.AsNoTracking()).Returns(mockBlock.Object);

            var mockDeliveryRouteBlockEnumerable = new DbAsyncEnumerable<DeliveryRouteBlock>(deliveryRouteBlocks);
            var mockDeliveryRouteBlock = MockDbSet(deliveryRouteBlocks);
            mockDeliveryRouteBlock.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockDeliveryRouteBlockEnumerable.AsQueryable().Provider);
            mockDeliveryRouteBlock.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockDeliveryRouteBlockEnumerable.AsQueryable().Expression);
            mockDeliveryRouteBlock.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockDeliveryRouteBlockEnumerable.AsQueryable().ElementType);
            mockDeliveryRouteBlock.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryRouteBlock>)mockDeliveryRouteBlockEnumerable).GetAsyncEnumerator());

            mockRMDBContext.Setup(x => x.Set<DeliveryRouteBlock>()).Returns(mockDeliveryRouteBlock.Object);
            mockRMDBContext.Setup(x => x.DeliveryRouteBlocks).Returns(mockDeliveryRouteBlock.Object);
            mockDeliveryRouteBlock.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryRouteBlock.Object);
            mockRMDBContext.Setup(c => c.DeliveryRouteBlocks.AsNoTracking()).Returns(mockDeliveryRouteBlock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new BlockSequenceDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}