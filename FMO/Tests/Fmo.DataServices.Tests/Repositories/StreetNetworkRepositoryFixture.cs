using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Fmo.Common.AsyncEnumerator;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;
using Moq;
using NUnit.Framework;

namespace Fmo.DataServices.Tests.Repositories
{
    [TestFixture]
    public class StreetNetworkRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IStreetNetworkRepository testCandidate;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid unit1Guid;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;

        [Test]
        public async Task TestFetchStreetNamesForBasicSearchValid()
        {
            var actualResult = await testCandidate.FetchStreetNamesForBasicSearch("Test", unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestFetchStreetNamesForBasicSearchInvalid()
        {
            var actualResult = await testCandidate.FetchStreetNamesForBasicSearch("invalid_Test", unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestFetchStreetNamesForBasicSearchNull()
        {
            var actualResult = await testCandidate.FetchStreetNamesForBasicSearch(null, unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestGetStreetNameCountValid()
        {
            var actualResultCount = await testCandidate.GetStreetNameCount("Test", unit1Guid);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public async Task TestGetStreetNameCountInvalid()
        {
            var actualResultCount = await testCandidate.GetStreetNameCount("invalid_Test", unit1Guid);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 0);
        }

        [Test]
        public async Task TestGetStreetNameCountNull()
        {
            var actualResultCount = await testCandidate.GetStreetNameCount(null, unit1Guid);
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

            var streetNames = new List<StreetName>()
            {
                new StreetName() { StreetName_Id = 1, StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad1", DesignatedName = "XYZ1", Geometry = unitBoundary },
                new StreetName() { StreetName_Id = 2, StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad2", DesignatedName = "XYZ2", Geometry = unitBoundary },
                new StreetName() { StreetName_Id = 3, StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad3", DesignatedName = "XYZ3", Geometry = unitBoundary },
                new StreetName() { StreetName_Id = 4, StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad4", DesignatedName = "XYZ4", Geometry = unitBoundary },
                new StreetName() { StreetName_Id = 5, StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad5", DesignatedName = "XYZ5", Geometry = unitBoundary },
                new StreetName() { StreetName_Id = 6, StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad6", DesignatedName = "XYZ6", Geometry = unitBoundary },
                new StreetName() { StreetName_Id = 7, StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad7", DesignatedName = "XYZ7", Geometry = unitBoundary }
            };

            var unitLocation = new List<UnitLocation>()
            {
                new UnitLocation() { UnitName = "unit1", ExternalId = "extunit1", ID = unit1Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit1Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit2", ExternalId = "extunit2", ID = unit2Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit2Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit3", ExternalId = "extunit2", ID = unit3Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit3Guid, User_GUID = user2Id } } }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<StreetName>(streetNames);
            var mockStreetNetworkRepository = MockDbSet(streetNames);
            mockStreetNetworkRepository.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockStreetNetworkRepository.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockStreetNetworkRepository.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockStreetNetworkRepository.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<StreetName>)mockAsynEnumerable).GetAsyncEnumerator());
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<StreetName>()).Returns(mockStreetNetworkRepository.Object);
            mockFmoDbContext.Setup(x => x.StreetNames).Returns(mockStreetNetworkRepository.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockStreetNetworkRepository2 = MockDbSet(unitLocation);
            mockStreetNetworkRepository2.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockStreetNetworkRepository2.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockStreetNetworkRepository2.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockStreetNetworkRepository2.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UnitLocation>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockFmoDbContext.Setup(x => x.Set<UnitLocation>()).Returns(mockStreetNetworkRepository2.Object);
            mockFmoDbContext.Setup(x => x.UnitLocations).Returns(mockStreetNetworkRepository2.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new StreetNetworkRepository(mockDatabaseFactory.Object);
        }
    }
}