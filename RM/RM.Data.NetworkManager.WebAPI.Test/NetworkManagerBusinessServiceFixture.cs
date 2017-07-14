using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Microsoft.SqlServer.Types;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.BusinessService;
using RM.DataManagement.NetworkManager.WebAPI.IntegrationService;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.DTO;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.DataManagement.NetworkManager.WebAPI.DataDTO;

namespace RM.Data.NetworkManager.WebAPI.Test
{
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

        [Test]
        public void Test_GetNearestNamedRoad()
        {
            var result = testCandidate.GetNearestNamedRoad(dbGeometry, "abc");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetNearestSegment()
        {
            var result = testCandidate.GetNearestSegment(dbGeometry);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetNetworkLink()
        {
            var result = testCandidate.GetNetworkLink(Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetCrossingNetworkLinks()
        {
            var result = testCandidate.GetCrossingNetworkLinks(coordinates, dbGeometry);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetOSRoadLink()
        {
            var result = testCandidate.GetOSRoadLink("123");
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, "abc");
        }

        [Test]
        public void Test_GetRoadRoutes()
        {
            var result = testCandidate.GetRoadRoutes(coordinates, Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_FetchStreetNamesForBasicSearch()
        {
            var result = testCandidate.FetchStreetNamesForBasicSearch("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result[0].LocalName, "abc");
        }

        [Test]
        public void Test_GetStreetNameCount()
        {
            var result = testCandidate.GetStreetNameCount("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, 5);
        }

        [Test]
        public void Test_FetchStreetNamesForAdvanceSearch()
        {
            var result = testCandidate.FetchStreetNamesForAdvanceSearch("abc", Guid.NewGuid());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result[0].LocalName, "abc");
        }

        [Test]
        public void TestGetRoadName()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            var result = testCandidate.GetRoadRoutes(coordinates, Guid.NewGuid());
            Assert.IsNotNull(result);
        }

        // TODO  Code to be refactored

        protected override void OnSetup()
        {
            // TODO  Code to be refactored

            //mockStreetNetworkDataService = CreateMock<IStreetNetworkDataService>();
            //mockNetworkManagerIntegrationService = CreateMock<INetworkManagerIntegrationService>();
            //mockOsRoadLinkDataService = CreateMock<IOSRoadLinkDataService>();
            //mockRoadNameDataService = CreateMock<IRoadNameDataService>();
            //mockStreetNetworkDataService = CreateMock<IStreetNetworkDataService>();
            //loggingHelperMock = CreateMock<ILoggingHelper>();
            //dbGeometry = DbGeometry.PointFromText("POINT (488938 197021)", 27700);
            //SqlGeometry networkIntersectionPoint = SqlGeometry.Null;
            //coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
            //lineGeometry = DbGeometry.LineFromText("LINESTRING (512722.70000000019 104752.6799999997, 512722.70000000019 104738)", 27700);
            //NetworkLinkDTO networkLink = new NetworkLinkDTO() { Id = Guid.NewGuid(), LinkGeometry = lineGeometry };
            //Tuple<NetworkLinkDataDTO, SqlGeometry> tuple = new Tuple<NetworkLinkDataDTO, SqlGeometry>(networkLink, networkIntersectionPoint);

            //referenceDataCategoryDTOList = new List<ReferenceDataCategoryDTO>()
            //{
            //    new ReferenceDataCategoryDTO()
            //    {
            //        ReferenceDatas = new List<ReferenceDataDTO>()
            //        {
            //            new ReferenceDataDTO()
            //            {
            //                ReferenceDataValue = FileType.Nyb.ToString(),
            //                ID = Guid.NewGuid(),
            //            }
            //        },
            //        CategoryName = PostalAddressType
            //    }
            //};

            //mockNetworkManagerIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).ReturnsAsync(referenceDataCategoryDTOList);
            //mockNetworkManagerIntegrationService.Setup(x => x.GetReferenceDataNameValuePairs(It.IsAny<List<string>>())).ReturnsAsync(referenceDataCategoryDTOList);

            //mockStreetNetworkDataService.Setup(x => x.GetNearestNamedRoad(It.IsAny<DbGeometry>(), It.IsAny<string>(), It.IsAny<List<ReferenceDataCategoryDTO>>())).Returns(tuple);
            //mockStreetNetworkDataService.Setup(x => x.GetNearestSegment(It.IsAny<DbGeometry>(), It.IsAny<List<ReferenceDataCategoryDTO>>())).Returns(tuple);
            //mockStreetNetworkDataService.Setup(x => x.GetNetworkLink(It.IsAny<Guid>())).Returns(networkLink);
            //mockStreetNetworkDataService.Setup(x => x.GetCrossingNetworkLink(It.IsAny<string>(), It.IsAny<DbGeometry>())).Returns(new List<NetworkLinkDTO>() { networkLink });
            //mockStreetNetworkDataService.Setup(x => x.FetchStreetNamesForBasicSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<StreetNameDTO>() { new StreetNameDTO() { LocalName = "abc" } });
            //mockStreetNetworkDataService.Setup(x => x.GetStreetNameCount(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(5);
            //mockStreetNetworkDataService.Setup(x => x.FetchStreetNamesForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<StreetNameDTO>() { new StreetNameDTO() { LocalName = "abc" } });

            //mockOsRoadLinkDataService.Setup(x => x.GetOSRoadLink(It.IsAny<string>())).ReturnsAsync("abc");
            //mockRoadNameDataService.Setup(x => x.GetRoadRoutes(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<List<ReferenceDataCategoryDTO>>())).Returns(new List<NetworkLinkDTO>() { networkLink });

            //var rmTraceManagerMock = new Mock<IRMTraceManager>();
            //rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            //loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            //SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            //testCandidate = new NetworkManagerBusinessService(mockStreetNetworkDataService.Object, mockNetworkManagerIntegrationService.Object, mockOsRoadLinkDataService.Object, mockRoadNameDataService.Object, loggingHelperMock.Object);
        }
    }
}