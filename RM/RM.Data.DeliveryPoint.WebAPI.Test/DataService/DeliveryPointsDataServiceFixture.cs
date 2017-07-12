using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPoint.WebAPI.Entities;
using RM.DataManagement.DeliveryPoint.WebAPI.DataService;

namespace RM.Data.DeliveryPoint.WebAPI.Test.DataService
{
    [TestFixture]
    [Ignore("Ignored since changes not yet implemented completely")]
    public class DeliveryPointsDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<DeliveryPointDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<DeliveryPointDBContext>> mockDatabaseFactory;
        private IDeliveryPointsDataService testCandidate;
        private string coordinates;
        private Guid unit1Guid;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private List<RM.CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryDTO;

        [Test]
        public void Test_GetDeliveryPointRowVersion()
        {
            DeliveryPointRowVersionSetup();
            var actualResult = testCandidate.GetDeliveryPointRowVersion(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetDPUse()
        {
            OnSetup();
            var actualResult = testCandidate.GetDPUse(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"), new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"), new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == "Organisation");
        }

        [Test]
        public void Test_GetDeliveryPointByPostalAddress()
        {
            var result = testCandidate.GetDeliveryPointByPostalAddress(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"));
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            unit1Guid = Guid.NewGuid();

            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);

            var deliveryPoint = new List<RM.Data.DeliveryPoint.WebAPI.Entities.DeliveryPoint>()
            {
               new RM.Data.DeliveryPoint.WebAPI.Entities.DeliveryPoint()
               {
                   ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                   PostalAddressID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                   DeliveryPointUseIndicatorGUID = new Guid("019dbbbb-03fb-489c-8c8d-f1085e0d2a11")
               }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<RM.Data.DeliveryPoint.WebAPI.Entities.DeliveryPoint>(deliveryPoint);
            var mockDeliveryPointDataService = MockDbSet(deliveryPoint);
            mockRMDBContext = CreateMock<DeliveryPointDBContext>();
            mockRMDBContext.Setup(x => x.Set<RM.Data.DeliveryPoint.WebAPI.Entities.DeliveryPoint>()).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointDataService.Object);
            mockDeliveryPointDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<DeliveryPointDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        protected void DeliveryPointRowVersionSetup()
        {
            var deliveryPoint = new List<RM.Data.DeliveryPoint.WebAPI.Entities.DeliveryPoint>()
            {
               new RM.Data.DeliveryPoint.WebAPI.Entities.DeliveryPoint()
               {
                   ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                   RowVersion = BitConverter.GetBytes(1)
               }
            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();

            var mockDeliveryPointDataService = MockDbSet(deliveryPoint);
            mockRMDBContext = CreateMock<DeliveryPointDBContext>();
            mockRMDBContext.Setup(x => x.Set<RM.Data.DeliveryPoint.WebAPI.Entities.DeliveryPoint>()).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDataService.Object);
            mockDeliveryPointDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<DeliveryPointDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}