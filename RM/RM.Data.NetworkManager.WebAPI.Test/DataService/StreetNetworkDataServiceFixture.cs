using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Implementation;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.Entities;

namespace RM.Data.NetworkManager.WebAPI.Test.DataService
{
    /// <summary>
    /// This class contains test methods for StreetNetworkDataService
    /// </summary>
    public class StreetNetworkDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<NetworkDBContext> mockNetworkDBContext;
        private Mock<IDatabaseFactory<NetworkDBContext>> mockDatabaseFactory;
        private IStreetNetworkDataService testCandidate;
        private Mock<ILoggingHelper> mockILoggingHelper;
        private Mock<IConfigurationHelper> mockIConfigurationHelper;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid unit1Guid;
        private DbGeometry accessLinkLine;
        private DbGeometry point;
        private List<ReferenceDataCategoryDTO> referenceDataCategoryList;

        /// <summary>
        /// Test for getting street names for advance search
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetStreetNamesForAdvanceSearch()
        {
            var actualResult = await testCandidate.GetStreetNamesForAdvanceSearch("Test", unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 7);
        }

        /// <summary>
        /// Test for fetching street name for Basic Search
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetStreetNamesForBasicSearch()
        {
            var actualResult = await testCandidate.GetStreetNamesForBasicSearch("Test", unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        /// <summary>
        /// Negative testing for fetching street name for Basic Search by passing null as a serch text
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetStreetNamesForBasicSearch_NegativeScenario()
        {
            var actualResult = await testCandidate.GetStreetNamesForBasicSearch(null, unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        /// <summary>
        /// Test for getting the count of street name
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetStreetNameCount()
        {
            var actualResult = await testCandidate.GetStreetNameCount("Test", unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == 7);
        }

        /// <summary>
        /// Negative testing for for getting the count of street name by passing null as a serch text
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetStreetNameCount_NegativeScenario()
        {
            var actualResult = await testCandidate.GetStreetNameCount(null, unit1Guid);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == 7);
        }

        /// <summary>
        /// Test for getting the nearest street for operational object
        /// </summary>
        [Test]
        public void Test_GetNearestNamedRoad()
        {
            var actualResult = testCandidate.GetNearestNamedRoad(point, "XYZ1", referenceDataCategoryList);
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.Item1);
            Assert.IsNotNull(actualResult.Item2);
        }

        /// <summary>
        /// Test for getting the nearest segment for operational object
        /// </summary>
        [Test]
        public void Test_GetNearestSegment()
        {
            var actualResult = testCandidate.GetNearestSegment(point, referenceDataCategoryList);
            Assert.IsNotNull(actualResult);
            Assert.IsNotNull(actualResult.Item1);
            Assert.IsNotNull(actualResult.Item2);
        }

        /// <summary>
        /// Test for getting the street DTO for operational object
        /// </summary>
        [Test]
        public void Test_GetNetworkLink()
        {
            var actualResult = testCandidate.GetNetworkLink(new Guid("8134AA41-391F-4579-A18D-D7EDF5B5F911"));
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(actualResult.ID, new Guid("8134AA41-391F-4579-A18D-D7EDF5B5F911"));
        }

        /// <summary>
        /// Test for getting the Network Links crossing the operational Object for a given extent
        /// </summary>
        [Test]
        public void Test_GetCrossingNetworkLink()
        {
            string coordinates = "POLYGON((511570.8590967182 106965.35195621933, 511570.8590967182 107474.95297542136, 512474.1409032818 107474.95297542136, 512474.1409032818 106965.35195621933, 511570.8590967182 106965.35195621933))";
            var actualResult = testCandidate.GetCrossingNetworkLink(coordinates, accessLinkLine);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(actualResult.Count, 0);
        }

        /// <summary>
        /// Data and Methods setup required to run test methods
        /// </summary>
        protected override void OnSetup()
        {
            // Data setup
            unit1Guid = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");
            mockIConfigurationHelper = CreateMock<IConfigurationHelper>();

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            accessLinkLine = DbGeometry.LineFromText("LINESTRING (488938 197021, 488929.9088937093 197036.37310195228)", 27700);
            point = DbGeometry.PointFromText("POINT (488938 197021)", 27700);

            var streetNamesList = new List<StreetName>()
            {
                new StreetName() { ID = new Guid("8134AA41-391F-4579-A18D-D7EDF5B5F918"), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad1", DesignatedName = "XYZ1", Geometry = unitBoundary },
                new StreetName() { ID = new Guid("8234AA41-391F-4579-A18D-D7EDF5B5F918"), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad2", DesignatedName = "XYZ2", Geometry = unitBoundary },
                new StreetName() { ID = new Guid("8334AA41-391F-4579-A18D-D7EDF5B5F918"), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad3", DesignatedName = "XYZ3", Geometry = unitBoundary },
                new StreetName() { ID = new Guid("8434AA41-391F-4579-A18D-D7EDF5B5F918"), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad4", DesignatedName = "XYZ4", Geometry = unitBoundary },
                new StreetName() { ID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918"), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad5", DesignatedName = "XYZ5", Geometry = unitBoundary },
                new StreetName() { ID = new Guid("8634AA41-391F-4579-A18D-D7EDF5B5F918"), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad6", DesignatedName = "XYZ6", Geometry = unitBoundary },
                new StreetName() { ID = new Guid("8734AA41-391F-4579-A18D-D7EDF5B5F918"), StreetType = "t1", Descriptor = "d1", NationalRoadCode = "Testroad7", DesignatedName = "XYZ7", Geometry = unitBoundary }
            };

            var networkLinkList = new List<NetworkLink>()
            {
                new NetworkLink()
                {
                    LinkGeometry = accessLinkLine,
                    StreetNameGUID = new Guid("8134AA41-391F-4579-A18D-D7EDF5B5F918"),
                    NetworkLinkTypeGUID = Guid.Empty,
                    ID = new Guid("8134AA41-391F-4579-A18D-D7EDF5B5F911")
                }
            };

            List<Location> locationList = new List<Location>()
            {
                new Location()
                {
                    ID = unit1Guid,
                    Shape = unitBoundary
                }
            };

            referenceDataCategoryList = new List<ReferenceDataCategoryDTO>()
            {
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.NetworkLinkType,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataValue = ReferenceDataValues.NetworkLinkPathLink
                        }
                    },
                    Id = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
                },

                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.NetworkLinkType,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataValue = ReferenceDataValues.NetworkLinkRoadLink
                        }
                    },
                    Id = new Guid("2534AA41-391F-4579-A18D-D7EDF5B5F918")
                },

                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.AccessLinkParameters,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = ReferenceDataValues.AccessLinkDiffRoadMaxDistance,
                            ReferenceDataValue = "5"
                        }
                    },
                    Id = new Guid("3534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            mockNetworkDBContext = CreateMock<NetworkDBContext>();
            mockILoggingHelper = CreateMock<ILoggingHelper>();

            // Setup for StreetName
            var mockAsynEnumerable = new DbAsyncEnumerable<StreetName>(streetNamesList);
            var mockStreetName = MockDbSet(streetNamesList);
            mockStreetName.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockStreetName.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockStreetName.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockStreetName.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<StreetName>)mockAsynEnumerable).GetAsyncEnumerator());
            mockNetworkDBContext.Setup(x => x.Set<StreetName>()).Returns(mockStreetName.Object);
            mockNetworkDBContext.Setup(x => x.StreetNames).Returns(mockStreetName.Object);
            mockStreetName.Setup(x => x.AsNoTracking()).Returns(mockStreetName.Object);

            // Setup for Location
            var mockAsynEnumerable1 = new DbAsyncEnumerable<Location>(locationList);
            var mockLocation = MockDbSet(locationList);
            mockLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Location>)mockAsynEnumerable1).GetAsyncEnumerator());
            mockNetworkDBContext.Setup(x => x.Set<Location>()).Returns(mockLocation.Object);
            mockNetworkDBContext.Setup(x => x.Locations).Returns(mockLocation.Object);
            mockLocation.Setup(x => x.AsNoTracking()).Returns(mockLocation.Object);

            // Setup for NetworkLink
            var mockAsynEnumerable3 = new DbAsyncEnumerable<NetworkLink>(networkLinkList);
            var mockNetworkLink = MockDbSet(networkLinkList);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockNetworkLink.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockNetworkLink.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<NetworkLink>)mockAsynEnumerable3).GetAsyncEnumerator());
            mockNetworkDBContext.Setup(x => x.Set<NetworkLink>()).Returns(mockNetworkLink.Object);
            mockNetworkDBContext.Setup(x => x.NetworkLinks).Returns(mockNetworkLink.Object);
            mockNetworkDBContext.Setup(x => x.NetworkLinks.AsNoTracking()).Returns(mockNetworkLink.Object);

            // Mock trace manager
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockIConfigurationHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns("5");
            mockDatabaseFactory = CreateMock<IDatabaseFactory<NetworkDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockNetworkDBContext.Object);
            testCandidate = new StreetNetworkDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object, mockIConfigurationHelper.Object);
        }
    }
}