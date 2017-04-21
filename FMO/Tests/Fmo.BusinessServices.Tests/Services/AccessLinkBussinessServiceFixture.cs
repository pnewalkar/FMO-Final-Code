using System;

namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using BusinessServices.Services;
    using DataServices.Repositories.Interfaces;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common.TestSupport;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AccessLinkBussinessServiceFixture : TestFixtureBase
    {
        private IAccessLinkBusinessService testCandidate;
        private Mock<IAccessLinkRepository> mockaccessLinkRepository;
        private List<AccessLinkDTO> accessLinkDTO = null;

        [Test]
        public void Test_GetAccessLinks()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            Guid unitGuid = Guid.NewGuid();
            var result = testCandidate.GetAccessLinks(coordinates, unitGuid);
            mockaccessLinkRepository.Verify(x => x.GetAccessLinks(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
        }

        protected override void OnSetup()
        {
            mockaccessLinkRepository = new Mock<IAccessLinkRepository>();
            accessLinkDTO = new List<AccessLinkDTO>() { new AccessLinkDTO() { AccessLink_Id = 1, features = "DI0001", AccessLinkType_Id = 1, type = "UnitOne" } };
            mockaccessLinkRepository.Setup(x => x.GetAccessLinks(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<AccessLinkDTO>>);

            testCandidate = new AccessLinkBussinessService(mockaccessLinkRepository.Object);
        }
    }
}