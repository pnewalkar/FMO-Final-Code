using System;
using Fmo.Common.Interface;

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
        private Mock<IReferenceDataCategoryRepository> referenceDataCategoryRepositoryMock;
        private Mock<IDeliveryPointsRepository> deliveryPointsRepositoryMock;
        private Mock<IStreetNetworkBusinessService> streetNetworkBusinessServiceMock;
        private Mock<IAccessLinkRepository> mockaccessLinkRepository;
        private Mock<ILoggingHelper> loggingHelperMock;
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
            accessLinkDTO = new List<AccessLinkDTO>() { new AccessLinkDTO() { ID = Guid.NewGuid() } };
            mockaccessLinkRepository.Setup(x => x.GetAccessLinks(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<AccessLinkDTO>>);
            referenceDataCategoryRepositoryMock = new Mock<IReferenceDataCategoryRepository>();
            deliveryPointsRepositoryMock = new Mock<IDeliveryPointsRepository>();
            streetNetworkBusinessServiceMock = new Mock<IStreetNetworkBusinessService>();
            loggingHelperMock = new Mock<ILoggingHelper>();
            testCandidate = new AccessLinkBusinessService(mockaccessLinkRepository.Object, referenceDataCategoryRepositoryMock.Object, deliveryPointsRepositoryMock.Object, streetNetworkBusinessServiceMock.Object, loggingHelperMock.Object);
        }
    }
}