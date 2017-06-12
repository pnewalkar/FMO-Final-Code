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

         //   mockNetworkManagerIntegrationService.Setup()

            testCandidate = new NetworkManagerBusinessService(mockStreetNetworkDataService.Object, mockNetworkManagerIntegrationService.Object, mockOsRoadLinkDataService.Object, mockRoadNameDataService.Object);
        }
    }
}
