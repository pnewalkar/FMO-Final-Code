using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.DataServices.Tests.DataService
{
    [TestFixture]
    public class StreetNetworkDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IStreetNetworkDataService testCandidate;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid unit1Guid;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;
        private DbGeometry accessLinkLine;

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

        [Test]
        public void TestGetCrossingNetworkLink()
        {
            string coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            var result = testCandidate.GetCrossingNetworkLink(coordinates, accessLinkLine);
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            unit1Guid = Guid.NewGuid();
            unit2Guid = Guid.NewGuid();
            unit3Guid = Guid.NewGuid();
            user1Id = System.Guid.NewGuid();
            user2Id = System.Guid.NewGuid();
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);

            var streetNames = new List<StreetName>()
            {
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad1", DesignatedName = "XYZ1", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad2", DesignatedName = "XYZ2", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad3", DesignatedName = "XYZ3", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad4", DesignatedName = "XYZ4", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad5", DesignatedName = "XYZ5", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad6", DesignatedName = "XYZ6", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad7", DesignatedName = "XYZ7", Geometry = unitBoundary }
            };

            var unitLocation = new List<UnitLocation>()
            {
                new UnitLocation() { UnitName = "unit1", ExternalId = "extunit1", ID = unit1Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit1Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit2", ExternalId = "extunit2", ID = unit2Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit2Guid, User_GUID = user1Id } } },
                new UnitLocation() { UnitName = "unit3", ExternalId = "extunit2", ID = unit3Guid, UnitBoundryPolygon = unitBoundary, UserRoleUnits = new List<UserRoleUnit> { new UserRoleUnit { Unit_GUID = unit3Guid, User_GUID = user2Id } } }
            };

            var networkLink = new List<NetworkLink>()
            {
                new NetworkLink()
                {
                    LinkGeometry = accessLinkLine
                }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<StreetName>(streetNames);
            var mockStreetNetworkDataService = MockDbSet(streetNames);
            mockStreetNetworkDataService.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockStreetNetworkDataService.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockStreetNetworkDataService.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockStreetNetworkDataService.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<StreetName>)mockAsynEnumerable).GetAsyncEnumerator());
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<StreetName>()).Returns(mockStreetNetworkDataService.Object);
            mockRMDBContext.Setup(x => x.StreetNames).Returns(mockStreetNetworkDataService.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<UnitLocation>(unitLocation);
            var mockStreetNetworkDataService2 = MockDbSet(unitLocation);
            mockStreetNetworkDataService2.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockStreetNetworkDataService2.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockStreetNetworkDataService2.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockStreetNetworkDataService2.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UnitLocation>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<UnitLocation>()).Returns(mockStreetNetworkDataService2.Object);
            mockRMDBContext.Setup(x => x.UnitLocations).Returns(mockStreetNetworkDataService2.Object);

            var mockAsynEnumerable3 = new DbAsyncEnumerable<NetworkLink>(networkLink);
            var mockNetworkLink = MockDbSet(networkLink);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockNetworkLink.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<NetworkLink>)mockAsynEnumerable3).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<NetworkLink>()).Returns(mockNetworkLink.Object);
            mockRMDBContext.Setup(x => x.NetworkLinks).Returns(mockNetworkLink.Object);
            mockRMDBContext.Setup(x => x.NetworkLinks.AsNoTracking()).Returns(mockNetworkLink.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new StreetNetworkDataService(mockDatabaseFactory.Object);
        }
    }
}