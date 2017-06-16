using System;

namespace RM.DataServices.Tests.DataService
{
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using CommonLibrary.LoggingMiddleware;
    using Moq;
    using NUnit.Framework;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DataService;
    using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
    using RM.CommonLibrary.EntityFramework.DTO;
    using RM.CommonLibrary.EntityFramework.Entities;
    using RM.CommonLibrary.HelperMiddleware;

    [TestFixture]
    public class DeliveryPointDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IDeliveryPointsDataService testCandidate;
        private string coordinates;
        private Guid userId = System.Guid.NewGuid();
        private Guid unit1Guid;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private List<ReferenceDataCategoryDTO> referenceDataCategoryDTO;

        [Test]
        public void Test_GetDeliveryPoints()
        {
            OnSetup();
            coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
            var actualResult = testCandidate.GetDeliveryPoints(coordinates, unit1Guid);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetDeliveryPointRowVersion()
        {
            DeliveryPointRowVersionSetup();
            var actualResult = testCandidate.GetDeliveryPointRowVersion(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetRouteForDeliveryPoint()
        {
            GetRouteForDeliveryPointSetup();
            var actualResult = testCandidate.GetRouteForDeliveryPoint(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == "Primary - test_route");
        }

        [Test]
        public void Test_GetDPUse()
        {
            GetRouteForDeliveryPointSetup();
            var actualResult = testCandidate.GetDPUse(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"), new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"), new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == "Organisation");
        }

        protected override void OnSetup()
        {
            unit1Guid = Guid.NewGuid();
            unit2Guid = Guid.NewGuid();
            unit3Guid = Guid.NewGuid();
            user1Id = System.Guid.NewGuid();
            user2Id = System.Guid.NewGuid();

            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);

            var unitLocation = new List<UnitLocation>()
            {
                new UnitLocation() { UnitName = "unit1", ExternalId = "extunit1", ID = unit1Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit1Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit2", ExternalId = "extunit2", ID = unit2Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit2Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit3", ExternalId = "extunit2", ID = unit3Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit3Guid, User_GUID = user2Id } } }
            };

            var deliveryPoint = new List<DeliveryPoint>()
            {
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg1", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    },
                   LocationXY = unitBoundary
               }
            };
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var mockAsynEnumerable = new DbAsyncEnumerable<DeliveryPoint>(deliveryPoint);
            var mockDeliveryPointDataService = MockDbSet(deliveryPoint);
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointDataService.Object);
            mockDeliveryPointDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService.Object);
            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockDeliveryPointDataService2 = MockDbSet(unitLocation);
            mockRMDBContext.Setup(x => x.Set<UnitLocation>()).Returns(mockDeliveryPointDataService2.Object);
            mockRMDBContext.Setup(x => x.UnitLocations).Returns(mockDeliveryPointDataService2.Object);
            mockRMDBContext.Setup(c => c.UnitLocations.AsNoTracking()).Returns(mockDeliveryPointDataService2.Object);
            mockDeliveryPointDataService2.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService2.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        protected void DeliveryPointRowVersionSetup()
        {
            var deliveryPoint = new List<DeliveryPoint>()
            {
               new DeliveryPoint()
               {
                   ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                   RowVersion = BitConverter.GetBytes(1)
               }
            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            //  mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            var mockDeliveryPointDataService = MockDbSet(deliveryPoint);
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDataService.Object);
            mockDeliveryPointDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        protected void GetRouteForDeliveryPointSetup()
        {
            var deliveryPoint = new List<DeliveryPoint>()
            {
               new DeliveryPoint()
               {
                   ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                   Address_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A12"),
                   DeliveryPointUseIndicator_GUID = new Guid("019dbbbb-03fb-489c-8c8d-f1085e0d2a11")
               }
            };

            var blockSequences = new List<BlockSequence>()
            {
                new BlockSequence() { Block_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A16"), OperationalObject_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11") }
            };

            var blocks = new List<Block>()
            {
                new Block() { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A16"), BlockType = "U" }
            };

            var deliveryRouteBlocks = new List<DeliveryRouteBlock>()
            {
                new DeliveryRouteBlock() { Block_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A16"), DeliveryRoute_GUID = new Guid("119DBBBB-03FB-489C-8C8D-F1085E0D2A13") }
            };

            var deliveryRoutes = new List<DeliveryRoute>()
            {
                new DeliveryRoute() { ID = new Guid("119DBBBB-03FB-489C-8C8D-F1085E0D2A13"), RouteName = "test_route" }
            };

            var postalAddress = new List<PostalAddress>()
            {
                new PostalAddress() { ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A12"), PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13") }
            };

            var deliveryRoutePostcodes = new List<DeliveryRoutePostcode>()
            {
                new DeliveryRoutePostcode() { Postcode_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"), DeliveryRoute_GUID = new Guid("119DBBBB-03FB-489C-8C8D-F1085E0D2A13"), IsPrimaryRoute = true }
            };

            referenceDataCategoryDTO = new List<ReferenceDataCategoryDTO>()
            {
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = "DeliveryPoint Use Indicator",
                    ReferenceDatas = new List<ReferenceDataDTO>() { new ReferenceDataDTO() { ReferenceDataValue = "Residential" }, new ReferenceDataDTO() { ReferenceDataValue = "Organisation" } }
                }
            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            //   mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            var mockDeliveryPointDataService = MockDbSet(deliveryPoint);
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDataService.Object);
            mockDeliveryPointDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointDataService.Object);

            var mockPostalAddressDataService = MockDbSet(postalAddress);
            mockRMDBContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDataService.Object);
            mockRMDBContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDataService.Object);
            mockPostalAddressDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDataService.Object);
            mockRMDBContext.Setup(c => c.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDataService.Object);

            var mockdeliveryRoutePostcodesDataService = MockDbSet(deliveryRoutePostcodes);
            mockRMDBContext.Setup(x => x.Set<DeliveryRoutePostcode>()).Returns(mockdeliveryRoutePostcodesDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryRoutePostcodes).Returns(mockdeliveryRoutePostcodesDataService.Object);
            mockdeliveryRoutePostcodesDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockdeliveryRoutePostcodesDataService.Object);
            mockRMDBContext.Setup(c => c.DeliveryRoutePostcodes.AsNoTracking()).Returns(mockdeliveryRoutePostcodesDataService.Object);

            var mockDeliveryRouteDataService = MockDbSet(deliveryRoutes);
            mockRMDBContext.Setup(x => x.Set<DeliveryRoute>()).Returns(mockDeliveryRouteDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryRoutes).Returns(mockDeliveryRouteDataService.Object);
            mockDeliveryRouteDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryRouteDataService.Object);
            mockRMDBContext.Setup(c => c.DeliveryRoutes.AsNoTracking()).Returns(mockDeliveryRouteDataService.Object);

            var mockBlockSequenceDataService = MockDbSet(blockSequences);
            mockRMDBContext.Setup(x => x.Set<BlockSequence>()).Returns(mockBlockSequenceDataService.Object);
            mockRMDBContext.Setup(x => x.BlockSequences).Returns(mockBlockSequenceDataService.Object);
            mockBlockSequenceDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockBlockSequenceDataService.Object);
            mockRMDBContext.Setup(c => c.BlockSequences.AsNoTracking()).Returns(mockBlockSequenceDataService.Object);

            var mockBlockDataService = MockDbSet(blocks);
            mockRMDBContext.Setup(x => x.Set<Block>()).Returns(mockBlockDataService.Object);
            mockRMDBContext.Setup(x => x.Blocks).Returns(mockBlockDataService.Object);
            mockBlockDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockBlockDataService.Object);
            mockRMDBContext.Setup(c => c.Blocks.AsNoTracking()).Returns(mockBlockDataService.Object);

            var mockDeliveryRouteBlockDataService = MockDbSet(deliveryRouteBlocks);
            mockRMDBContext.Setup(x => x.Set<DeliveryRouteBlock>()).Returns(mockDeliveryRouteBlockDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryRouteBlocks).Returns(mockDeliveryRouteBlockDataService.Object);
            mockDeliveryRouteBlockDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryRouteBlockDataService.Object);
            mockRMDBContext.Setup(c => c.DeliveryRouteBlocks.AsNoTracking()).Returns(mockDeliveryRouteBlockDataService.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}