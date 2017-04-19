namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using DataServices.Repositories.Interfaces;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.BusinessServices.Services;
    using Fmo.Common.TestSupport;
    using Fmo.DTO;
    using Helpers.Interface;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class RoadNameBussinessServiceFixture : TestFixtureBase
    {
        private IRoadNameBussinessService testCandidate;
        private Mock<IRoadNameRepository> mockRoadNameRepository;
        private Mock<ICreateOtherLayersObjects> mockCreateOtherLayers;
        private List<OsRoadLinkDTO> osRoadLinkDTO = null;

        [Test]
        public void TestGetRoadName()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            var result = testCandidate.GetRoadRoutes(coordinates);
            mockRoadNameRepository.Verify(x => x.GetRoadRoutes(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            mockRoadNameRepository = new Mock<IRoadNameRepository>();
            mockCreateOtherLayers = new Mock<ICreateOtherLayersObjects>();

            mockRoadNameRepository.Setup(x => x.GetRoadRoutes(It.IsAny<string>())).Returns(It.IsAny<List<OsRoadLinkDTO>>);

            testCandidate = new RoadNameBussinessService(mockRoadNameRepository.Object);
        }
    }
}
