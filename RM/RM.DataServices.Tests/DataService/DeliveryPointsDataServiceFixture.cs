namespace RM.DataServices.Tests.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Threading.Tasks;
    using CommonLibrary.EntityFramework.DTO;
    using CommonLibrary.LoggingMiddleware;
    using Moq;
    using NUnit.Framework;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DataService;
    using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
    using RM.CommonLibrary.EntityFramework.Entities;
    using RM.CommonLibrary.HelperMiddleware;

    [TestFixture]
    public class DeliveryPointsDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IDeliveryPointsDataService testCandidate;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid unit1Guid;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;
        private Mock<ILoggingHelper> mockLoggingHelper;

        [Test]
        public async Task TestFetchDeliveryPointsForBasicSearchValid()
        {
            var actualResult = await testCandidate.FetchDeliveryPointsForBasicSearch("Org", unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestFetchDeliveryPointsForBasicSearchNull()
        {
            var actualResult = new List<DeliveryPointDTO>() { };
            try
            {
                actualResult = await testCandidate.FetchDeliveryPointsForBasicSearch(null, unit1Guid);
            }
            catch (Exception e)
            {
                Assert.IsNotNull(actualResult);
            }
        }

        [Test]
        public async Task TestGetDeliveryPointsCountValid()
        {
            var actualResultCount = await testCandidate.GetDeliveryPointsCount("Org", unit1Guid);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public async Task TestGetDeliveryPointsCountNull()
        {
            var actualResultCount = await testCandidate.GetDeliveryPointsCount(null, unit1Guid);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public void TestGetDeliveryPointsCrossingOperationalObject()
        {
            string coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            DbGeometry operationalObject = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);

            var result = testCandidate.GetDeliveryPointsCrossingOperationalObject(coordinates, operationalObject);
            Assert.IsNotNull(result);
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
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg2", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 2, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    },
                   LocationXY = unitBoundary
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg3", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 3, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    },
                   LocationXY = unitBoundary
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg4", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 4862, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    },
                   LocationXY = unitBoundary
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg5", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    },
                   LocationXY = unitBoundary
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg6", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    },
                   LocationXY = unitBoundary
               },
               new DeliveryPoint()
               {
                   PostalAddress = new PostalAddress()
                    {
                        OrganisationName = "MyOrg7", BuildingName = "Test", SubBuildingName = "subTest", BuildingNumber = 1, Thoroughfare = "ABC", DependentLocality = "XYZ"
                    },
                   LocationXY = unitBoundary
               }
            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();

            var mockAsynEnumerable = new DbAsyncEnumerable<DeliveryPoint>(deliveryPoint);
            var mockDeliveryPointDataService = MockDbSet(deliveryPoint);
            mockDeliveryPointDataService.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockDeliveryPointDataService.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockDeliveryPointDataService.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockDeliveryPointDataService.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPoint>)mockAsynEnumerable).GetAsyncEnumerator());
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDataService.Object);
            mockRMDBContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointDataService.Object);
            mockDeliveryPointDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockDeliveryPointDataService2 = MockDbSet(unitLocation);
            mockDeliveryPointDataService2.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockDeliveryPointDataService2.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockDeliveryPointDataService2.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockDeliveryPointDataService2.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UnitLocation>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<UnitLocation>()).Returns(mockDeliveryPointDataService2.Object);
            mockRMDBContext.Setup(x => x.UnitLocations).Returns(mockDeliveryPointDataService2.Object);
            mockRMDBContext.Setup(c => c.UnitLocations.AsNoTracking()).Returns(mockDeliveryPointDataService2.Object);
            mockDeliveryPointDataService2.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointDataService2.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}