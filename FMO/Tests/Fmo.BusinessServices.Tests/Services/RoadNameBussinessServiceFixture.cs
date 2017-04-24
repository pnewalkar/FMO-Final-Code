using System;

namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using DataServices.Repositories.Interfaces;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.BusinessServices.Services;
    using Fmo.Common.TestSupport;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class RoadNameBussinessServiceFixture : TestFixtureBase
    {
        private IRoadNameBusinessService testCandidate;
        private Mock<IRoadNameRepository> mockRoadNameRepository;
        private List<OsRoadLinkDTO> osRoadLinkDTO = null;
        private Guid userGuid = Guid.NewGuid();

        [Test]
        public void TestGetRoadName()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            var result = testCandidate.GetRoadRoutes(coordinates, userGuid);
            mockRoadNameRepository.Verify(x => x.GetRoadRoutes(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            mockRoadNameRepository = new Mock<IRoadNameRepository>();

            mockRoadNameRepository.Setup(x => x.GetRoadRoutes(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<OsRoadLinkDTO>>);

            testCandidate = new RoadNameBussinessService(mockRoadNameRepository.Object);
        }
    }
}