using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using Microsoft.SqlServer.Types;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.BusinessService;
using RM.DataManagement.NetworkManager.WebAPI.DataDTO;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.IntegrationService;

namespace RM.Data.NetworkManager.WebAPI.Test
{
    /// <summary>
    /// This class contains test methods for NetworkManagerBusinessService.
    /// </summary>
    [TestFixture]
    public class NetworkManagerBusinessServiceFixture : TestFixtureBase
    {
        private const string PostalAddressType = "Postal Address Type";
        private Mock<IStreetNetworkDataService> mockStreetNetworkDataService;
        private Mock<INetworkManagerIntegrationService> mockNetworkManagerIntegrationService;
        private Mock<IOSRoadLinkDataService> mockOsRoadLinkDataService;
        private Mock<IRoadNameDataService> mockRoadNameDataService;
        private INetworkManagerBusinessService testCandidate;
        private DbGeometry dbGeometry;
        private List<ReferenceDataCategoryDTO> referenceDataCategoryDTOList;
        private string coordinates;
        private DbGeometry lineGeometry;
        private Mock<ILoggingHelper> loggingHelperMock;

        /// <summary>
        /// Test to Get the nearest street for operational object.
        /// </summary>
        [Test]
        public void Test_GetNearestNamedRoad()
        {
            var result = testCandidate.GetNearestNamedRoad(dbGeometry, "abc");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Item1.LinkGeometry, lineGeometry);
            Assert.That(result.Item1.Id, Is.AssignableFrom(typeof(Guid)));
        }

        /// <summary>
        /// Test to get the nearest street for operational object.
        /// </summary>
        [Test]
        public void Test_GetNearestSegment()
        {
            var result = testCandidate.GetNearestSegment(dbGeometry);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.First().Item1.LinkGeometry, lineGeometry);
            Assert.That(result.First().Item1.Id, Is.AssignableFrom(typeof(Guid)));
        }

        /// <summary>
        /// Test for Get the street DTO for operational object.
        /// </summary>
        [Test]
        public void Test_GetNetworkLink()
        {
            var result = testCandidate.GetNetworkLink(Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.LinkGeometry, lineGeometry);
            Assert.That(result.Id, Is.AssignableFrom(typeof(Guid)));
        }

        /// <summary>
        ///  Test for get the road links crossing the access link.
        /// </summary>
        [Test]
        public void Test_GetCrossingNetworkLinks()
        {
            var result = testCandidate.GetCrossingNetworkLinks(coordinates, dbGeometry);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }

        /// <summary>
        /// Test to fetch data for Ordinance survey Road Link.
        /// </summary>
        [Test]
        public void Test_GetOSRoadLink()
        {
            var result = testCandidate.GetOSRoadLink("123");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, "abc");
        }

        /// <summary>
        /// Test to fetches data for RoadLinks.
        /// </summary>
        [Test]
        public void Test_GetRoadRoutes()
        {
            var result = testCandidate.GetRoadRoutes(coordinates, Guid.NewGuid(), "Delivery Office");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test to Fetch the street name for Basic Search.
        /// </summary>
        [Test]
        public void Test_FetchStreetNamesForBasicSearch()
        {
            var result = testCandidate.GetStreetNamesForBasicSearch("abc", Guid.NewGuid(), "Delivery Office");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result[0].LocalName, "abc");
        }

        /// <summary>
        /// Test for Get the count of street name.
        /// </summary>
        [Test]
        public void Test_GetStreetNameCount()
        {
            var result = testCandidate.GetStreetNameCount("abc", Guid.NewGuid(), "Delivery Office");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, 5);
        }

        /// <summary>
        /// Test for Fetch street names for advance search.
        /// </summary>
        [Test]
        public void Test_FetchStreetNamesForAdvanceSearch()
        {
            var result = testCandidate.GetStreetNamesForAdvanceSearch("abc", Guid.NewGuid(), "Delivery Office");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result[0].LocalName, "abc");
        }

        /// <summary>
        /// Test for fetches data for RoadLinks.
        /// </summary>
        [Test]
        public void TestGetRoadName()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            var result = testCandidate.GetRoadRoutes(coordinates, Guid.NewGuid(), "Delivery Office");
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Setup for Nunit Tests.
        /// </summary>
        protected override void OnSetup()
        {
            mockStreetNetworkDataService = CreateMock<IStreetNetworkDataService>();
            mockNetworkManagerIntegrationService = CreateMock<INetworkManagerIntegrationService>();
            mockOsRoadLinkDataService = CreateMock<IOSRoadLinkDataService>();
            mockRoadNameDataService = CreateMock<IRoadNameDataService>();
            loggingHelperMock = CreateMock<ILoggingHelper>();

            dbGeometry = DbGeometry.PointFromText("POINT (488938 197021)", 27700);
            List<SqlGeometry> listOfnetworkIntersectionPoint = new List<SqlGeometry>();
            SqlGeometry networkIntersectionPoint = new SqlGeometry();

            coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
            lineGeometry = DbGeometry.LineFromText("LINESTRING (512722.70000000019 104752.6799999997, 512722.70000000019 104738)", 27700);

            NetworkLinkDataDTO networkLink = new NetworkLinkDataDTO() { ID = Guid.NewGuid(), LinkGeometry = lineGeometry };
            List<Tuple<NetworkLinkDataDTO, SqlGeometry>> listOfTuple = new List<Tuple<NetworkLinkDataDTO, SqlGeometry>> { new Tuple<NetworkLinkDataDTO, SqlGeometry>(networkLink, networkIntersectionPoint) };
            Tuple<NetworkLinkDataDTO, SqlGeometry> tuple = new Tuple<NetworkLinkDataDTO, SqlGeometry>(networkLink, networkIntersectionPoint);

            referenceDataCategoryDTOList = new List<ReferenceDataCategoryDTO>()
            {
                new ReferenceDataCategoryDTO()
                {
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataValue = FileType.Nyb.ToString(),
                            ID = Guid.NewGuid(),
                        }
                    },
                    CategoryName = PostalAddressType
                }
            };

            // Setup Methods.
            mockNetworkManagerIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).ReturnsAsync(referenceDataCategoryDTOList);
            mockNetworkManagerIntegrationService.Setup(x => x.GetReferenceDataNameValuePairs(It.IsAny<List<string>>())).ReturnsAsync(referenceDataCategoryDTOList);
            mockStreetNetworkDataService.Setup(x => x.GetNearestNamedRoad(It.IsAny<DbGeometry>(), It.IsAny<string>(), It.IsAny<List<ReferenceDataCategoryDTO>>())).Returns(tuple);
            mockStreetNetworkDataService.Setup(x => x.GetNearestSegment(It.IsAny<DbGeometry>(), It.IsAny<List<ReferenceDataCategoryDTO>>())).Returns(listOfTuple);
            mockStreetNetworkDataService.Setup(x => x.GetNetworkLink(It.IsAny<Guid>())).Returns(networkLink);
            mockStreetNetworkDataService.Setup(x => x.GetCrossingNetworkLink(It.IsAny<string>(), It.IsAny<DbGeometry>())).Returns(new List<NetworkLinkDataDTO>() { networkLink });
            mockStreetNetworkDataService.Setup(x => x.GetStreetNamesForBasicSearch(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Office")).ReturnsAsync(new List<StreetNameDataDTO>() { new StreetNameDataDTO() { LocalName = "abc" } });
            mockStreetNetworkDataService.Setup(x => x.GetStreetNameCount(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Office")).ReturnsAsync(5);
            mockStreetNetworkDataService.Setup(x => x.GetStreetNamesForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Office")).ReturnsAsync(new List<StreetNameDataDTO>() { new StreetNameDataDTO() { LocalName = "abc" } });
            mockOsRoadLinkDataService.Setup(x => x.GetOSRoadLink(It.IsAny<string>())).ReturnsAsync("abc");
            mockRoadNameDataService.Setup(x => x.GetRoadRoutes(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<List<ReferenceDataCategoryDTO>>(), It.IsAny<string>())).Returns(new List<NetworkLinkDataDTO>() { networkLink });

            // Setup for IRMTraceManager.
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            testCandidate = new NetworkManagerBusinessService(mockStreetNetworkDataService.Object, mockNetworkManagerIntegrationService.Object, mockOsRoadLinkDataService.Object, mockRoadNameDataService.Object, loggingHelperMock.Object);
        }
    }
}