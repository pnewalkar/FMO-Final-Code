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
        private Mock<IReferenceDataBusinessService> referenceDataCategoryBusinessServiceMock;
        private Mock<IDeliveryPointsRepository> deliveryPointsRepositoryMock;
        private Mock<IStreetNetworkBusinessService> streetNetworkBusinessServiceMock;
        private Mock<IAccessLinkRepository> mockaccessLinkRepository;
        private Mock<IOSRoadLinkRepository> mockosroadLinkRepository;
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
            accessLinkDTO = new List<AccessLinkDTO>() { new AccessLinkDTO() { ID = Guid.NewGuid() } };
            List<string> categoryNames = new List<string>
                {
                    "Access Link Parameters"
                };

            mockaccessLinkRepository = new Mock<IAccessLinkRepository>();
            mockaccessLinkRepository.Setup(x => x.GetAccessLinks(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<AccessLinkDTO>>);

            referenceDataCategoryBusinessServiceMock = new Mock<IReferenceDataBusinessService>();
            referenceDataCategoryBusinessServiceMock.Setup(x => x.GetReferenceDataCategoriesByCategoryNames(It.IsAny<List<string>>())).Returns(It.IsAny<List<ReferenceDataCategoryDTO>>());

            mockosroadLinkRepository = new Mock<IOSRoadLinkRepository>();
            mockosroadLinkRepository.Setup(x => x.GetOSRoadLink(It.IsAny<string>())).Returns(It.IsAny<string>());

            deliveryPointsRepositoryMock = new Mock<IDeliveryPointsRepository>();
            streetNetworkBusinessServiceMock = new Mock<IStreetNetworkBusinessService>();
            loggingHelperMock = new Mock<ILoggingHelper>();

            testCandidate = new AccessLinkBusinessService(mockaccessLinkRepository.Object, referenceDataCategoryBusinessServiceMock.Object, deliveryPointsRepositoryMock.Object, streetNetworkBusinessServiceMock.Object, loggingHelperMock.Object, mockosroadLinkRepository.Object);
        }
    }
}