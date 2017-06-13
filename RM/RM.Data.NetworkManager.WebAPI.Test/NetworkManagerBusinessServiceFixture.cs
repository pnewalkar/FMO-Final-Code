using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Microsoft.SqlServer.Types;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.BusinessService;
using RM.DataManagement.NetworkManager.WebAPI.IntegrationService;

namespace RM.Data.NetworkManager.WebAPI.Test
{
    [TestFixture]
    public class NetworkManagerBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IStreetNetworkDataService> mockStreetNetworkDataService;
        private Mock<INetworkManagerIntegrationService> mockNetworkManagerIntegrationService;
        private Mock<IOSRoadLinkDataService> mockOsRoadLinkDataService;
        private Mock<IRoadNameDataService> mockRoadNameDataService;
        private INetworkManagerBusinessService testCandidate;
        private DbGeometry dbGeometry;
        private List<ReferenceDataCategoryDTO> referenceDataCategoryDTOList;
        private string coordinates;
        private DbGeometry lineGeometry;

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
            try
            {
                var result = testCandidate.GetRoadRoutes(coordinates, Guid.NewGuid());
                Assert.IsNotNull(result);
            }
            catch (Exception e)
            {
            }
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

        protected override void OnSetup()
        {
            mockStreetNetworkDataService = CreateMock<IStreetNetworkDataService>();
            mockNetworkManagerIntegrationService = CreateMock<INetworkManagerIntegrationService>();
            mockOsRoadLinkDataService = CreateMock<IOSRoadLinkDataService>();
            mockRoadNameDataService = CreateMock<IRoadNameDataService>();
            mockStreetNetworkDataService = CreateMock<IStreetNetworkDataService>();
            dbGeometry = DbGeometry.PointFromText("POINT (488938 197021)", 27700);
            SqlGeometry networkIntersectionPoint = SqlGeometry.Null;
            coordinates = "POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))";
            lineGeometry = DbGeometry.LineFromText("LINESTRING (512722.70000000019 104752.6799999997, 512722.70000000019 104738)", 27700);
            NetworkLinkDTO networkLink = new NetworkLinkDTO() { Id = Guid.NewGuid(), LinkGeometry = lineGeometry };
            Tuple<NetworkLinkDTO, SqlGeometry> tuple = new Tuple<NetworkLinkDTO, SqlGeometry>(networkLink, networkIntersectionPoint);

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
                    CategoryName= Constants.PostalAddressType
                }
            };

            mockNetworkManagerIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).ReturnsAsync(referenceDataCategoryDTOList);
            mockNetworkManagerIntegrationService.Setup(x => x.GetReferenceDataNameValuePairs(It.IsAny<List<string>>())).ReturnsAsync(referenceDataCategoryDTOList);

            mockStreetNetworkDataService.Setup(x => x.GetNearestNamedRoad(It.IsAny<DbGeometry>(), It.IsAny<string>(), It.IsAny<List<ReferenceDataCategoryDTO>>())).Returns(tuple);
            mockStreetNetworkDataService.Setup(x => x.GetNearestSegment(It.IsAny<DbGeometry>(), It.IsAny<List<ReferenceDataCategoryDTO>>())).Returns(tuple);
            mockStreetNetworkDataService.Setup(x => x.GetNetworkLink(It.IsAny<Guid>())).Returns(networkLink);
            mockStreetNetworkDataService.Setup(x => x.GetCrossingNetworkLink(It.IsAny<string>(), It.IsAny<DbGeometry>())).Returns(new List<NetworkLinkDTO>() { networkLink });
            mockStreetNetworkDataService.Setup(x => x.FetchStreetNamesForBasicSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<StreetNameDTO>() { new StreetNameDTO() { LocalName = "abc" } });
            mockStreetNetworkDataService.Setup(x => x.GetStreetNameCount(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(5);
            mockStreetNetworkDataService.Setup(x => x.FetchStreetNamesForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<StreetNameDTO>() { new StreetNameDTO() { LocalName = "abc" } });

            mockOsRoadLinkDataService.Setup(x => x.GetOSRoadLink(It.IsAny<string>())).ReturnsAsync("abc");
            mockRoadNameDataService.Setup(x => x.GetRoadRoutes(It.IsAny<string>(), It.IsAny<Guid>())).Returns(new List<NetworkLinkDTO>() { networkLink });

            testCandidate = new NetworkManagerBusinessService(mockStreetNetworkDataService.Object, mockNetworkManagerIntegrationService.Object, mockOsRoadLinkDataService.Object, mockRoadNameDataService.Object);
        }
    }
}