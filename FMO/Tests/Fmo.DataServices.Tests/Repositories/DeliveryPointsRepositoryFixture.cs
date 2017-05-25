namespace Fmo.DataServices.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Interface;
    using Fmo.Common.AsyncEnumerator;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeliveryPointsRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryPointsRepository testCandidate;
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
            var actualResult = await testCandidate.FetchDeliveryPointsForBasicSearch(null, unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
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
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            var mockAsynEnumerable = new DbAsyncEnumerable<DeliveryPoint>(deliveryPoint);
            var mockDeliveryPointRepository = MockDbSet(deliveryPoint);
            mockDeliveryPointRepository.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockDeliveryPointRepository.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockDeliveryPointRepository.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockDeliveryPointRepository.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPoint>)mockAsynEnumerable).GetAsyncEnumerator());
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointRepository.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointRepository.Object);
            mockFmoDbContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointRepository.Object);
            mockDeliveryPointRepository.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointRepository.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockDeliveryPointRepository2 = MockDbSet(unitLocation);
            mockDeliveryPointRepository2.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockDeliveryPointRepository2.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockDeliveryPointRepository2.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockDeliveryPointRepository2.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UnitLocation>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockFmoDbContext.Setup(x => x.Set<UnitLocation>()).Returns(mockDeliveryPointRepository2.Object);
            mockFmoDbContext.Setup(x => x.UnitLocations).Returns(mockDeliveryPointRepository2.Object);
            mockFmoDbContext.Setup(c => c.UnitLocations.AsNoTracking()).Returns(mockDeliveryPointRepository2.Object);
            mockDeliveryPointRepository2.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointRepository2.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new DeliveryPointsRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}