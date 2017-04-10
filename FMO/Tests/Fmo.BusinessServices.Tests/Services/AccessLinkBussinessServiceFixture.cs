namespace Fmo.BusinessServices.Tests.Services
{
    using BusinessServices.Services;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common.TestSupport;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;

    public class AccessLinkBussinessServiceFixture : TestFixtureBase
    {
        private IAccessLinkBussinessService testCandidate;
        private Mock<AccessLinkBussinessService> mockAccessLinkBussinessService;
        private AccessLinkDTO accessLinkDTO;
        string coordinates;

        [Test]
        public void Test_GetAccessLinks()
        {
            mockAccessLinkBussinessService.Verify(x => x.GetAccessLinks(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Test_GetData()
        {
            mockAccessLinkBussinessService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        protected override void OnSetup()
        {
            coordinates = "1234.776";
            accessLinkDTO = new AccessLinkDTO();
            mockAccessLinkBussinessService = new Mock<AccessLinkBussinessService>();
            mockAccessLinkBussinessService.Setup(n => n.GetAccessLinks(It.IsAny<string>())).Returns(accessLinkDTO);
            mockAccessLinkBussinessService.Setup(n => n.GetData(It.IsAny<string>(), It.IsAny<object[]>())).Returns(coordinates);
        }
    }
}
