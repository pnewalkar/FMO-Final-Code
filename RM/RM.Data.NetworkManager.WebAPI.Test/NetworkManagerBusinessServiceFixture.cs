using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.IntegrationService;
using Moq;
using RM.DataManagement.NetworkManager.WebAPI.BusinessService;
using System.Data.Entity.Spatial;
using RM.CommonLibrary.EntityFramework.DTO;
using Microsoft.SqlServer.Types;

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

        [Test]
        public void Test_GetNearestNamedRoad()
        {
            var result = testCandidate.GetNearestNamedRoad(dbGeometry,"abc");

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
            NetworkLinkDTO networkLink = null;
            Tuple < NetworkLinkDTO, SqlGeometry > tuple = new Tuple<NetworkLinkDTO, SqlGeometry>(networkLink, networkIntersectionPoint);

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

            mockStreetNetworkDataService.Setup(x => x.GetNearestNamedRoad(It.IsAny<DbGeometry>(), It.IsAny<string>(), It.IsAny<List<ReferenceDataCategoryDTO>>())).Returns(tuple);
            testCandidate = new NetworkManagerBusinessService(mockStreetNetworkDataService.Object, mockNetworkManagerIntegrationService.Object, mockOsRoadLinkDataService.Object, mockRoadNameDataService.Object);
        }
    }
}
