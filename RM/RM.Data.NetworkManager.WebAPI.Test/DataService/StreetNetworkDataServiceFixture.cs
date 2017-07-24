using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Implementation;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.Entities;

namespace RM.Data.NetworkManager.WebAPI.Test.DataService
{
    public class StreetNetworkDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<NetworkDBContext> mockNetworkDBContext;
        private Mock<IDatabaseFactory<NetworkDBContext>> mockDatabaseFactory;
        private IStreetNetworkDataService testCandidate;
        private Mock<ILoggingHelper> mockILoggingHelper;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid unit1Guid;
        private Guid unit2Guid;
        private Guid unit3Guid;
        private Guid user1Id;
        private Guid user2Id;
        private DbGeometry accessLinkLine;
        /*

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
        */

        protected override void OnSetup()
        {
            unit1Guid = Guid.NewGuid();
            unit2Guid = Guid.NewGuid();
            unit3Guid = Guid.NewGuid();
            user1Id = System.Guid.NewGuid();
            user2Id = System.Guid.NewGuid();
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);

            var streetNamesList = new List<StreetName>()
            {
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad1", DesignatedName = "XYZ1", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad2", DesignatedName = "XYZ2", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad3", DesignatedName = "XYZ3", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad4", DesignatedName = "XYZ4", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad5", DesignatedName = "XYZ5", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad6", DesignatedName = "XYZ6", Geometry = unitBoundary },
                new StreetName() { ID = Guid.NewGuid(), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad7", DesignatedName = "XYZ7", Geometry = unitBoundary }
            };

            var networkLinkList = new List<NetworkLink>()
            {
                new NetworkLink()
                {
                    LinkGeometry = accessLinkLine
                }
            };

            List<Location> locationList = new List<Location>()
            {
                new Location()
                {
                    ID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            mockNetworkDBContext = CreateMock<NetworkDBContext>();

            var mockAsynEnumerable = new DbAsyncEnumerable<StreetName>(streetNamesList);
            var mockStreetName = MockDbSet(streetNamesList);
            mockStreetName.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockStreetName.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockStreetName.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockStreetName.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<StreetName>)mockAsynEnumerable).GetAsyncEnumerator());
            mockNetworkDBContext.Setup(x => x.Set<StreetName>()).Returns(mockStreetName.Object);
            mockNetworkDBContext.Setup(x => x.StreetNames).Returns(mockStreetName.Object);
            mockStreetName.Setup(x => x.AsNoTracking()).Returns(mockStreetName.Object);

            var mockAsynEnumerable1 = new DbAsyncEnumerable<Location>(locationList);
            var mockLocation = MockDbSet(locationList);
            mockLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Location>)mockAsynEnumerable1).GetAsyncEnumerator());
            mockNetworkDBContext.Setup(x => x.Set<Location>()).Returns(mockLocation.Object);
            mockNetworkDBContext.Setup(x => x.Locations).Returns(mockLocation.Object);
            mockLocation.Setup(x => x.AsNoTracking()).Returns(mockLocation.Object);
                

            var mockAsynEnumerable3 = new DbAsyncEnumerable<NetworkLink>(networkLinkList);
            var mockNetworkLink = MockDbSet(networkLinkList);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockNetworkLink.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<NetworkLink>)mockAsynEnumerable3).GetAsyncEnumerator());
            mockNetworkDBContext.Setup(x => x.Set<NetworkLink>()).Returns(mockNetworkLink.Object);
            mockNetworkDBContext.Setup(x => x.NetworkLinks).Returns(mockNetworkLink.Object);
            mockNetworkDBContext.Setup(x => x.NetworkLinks.AsNoTracking()).Returns(mockNetworkLink.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<NetworkDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockNetworkDBContext.Object);
            testCandidate = new StreetNetworkDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object);
        }
    }
}