namespace Fmo.BusinessServices.Tests.Services
{
    using Fmo.BusinessServices.Interfaces;
    using Fmo.BusinessServices.Services;
    using Fmo.Common.TestSupport;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;

    public class RoadNameBussinessServiceFixture : TestFixtureBase
    {
        private IRoadNameBussinessService testCandidate;
        private Mock<RoadNameBussinessService> mockRoadNameBussinessService;
        private OsRoadLinkDTO osRoadLinkDTO;
        private string coordinates;

        [Test]
        public void Test_GetRoadLinks()
        {
            mockRoadNameBussinessService.Verify(x => x.GetRoadRoutes(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Test_GetData()
        {
            mockRoadNameBussinessService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        protected override void OnSetup()
        {
            coordinates = "1234.776";
            osRoadLinkDTO = new OsRoadLinkDTO();
            mockRoadNameBussinessService = new Mock<RoadNameBussinessService>();
            mockRoadNameBussinessService.Setup(n => n.GetRoadRoutes(It.IsAny<string>())).Returns(osRoadLinkDTO);
            mockRoadNameBussinessService.Setup(n => n.GetData(It.IsAny<string>(), It.IsAny<object[]>())).Returns(coordinates);
        }
    }

}
