namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using BusinessServices.Services;
    using Entities;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Helpers.Interface;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private IDeliveryPointBusinessService testCandidate;
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private Mock<ICreateOtherLayersObjects> mockCreateOtherLayers;
        private List<DeliveryPointDTO> deliveryPointDTO = null;

        protected override void OnSetup()
        {
            mockDeliveryPointsRepository = new Mock<IDeliveryPointsRepository>();
            mockCreateOtherLayers = new Mock<ICreateOtherLayersObjects>();

            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
            List<DeliveryPoint> lstDeliveryPoint = new List<DeliveryPoint>();
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>()
                        {
                            new PostalAddressDTO
                            {
                                BuildingName = "Bldg 1",
                                BuildingNumber = 23,
                                Postcode = "123"

        }

    };
            mockDeliveryPointsRepository.Setup(x => x.GetDeliveryPoints(It.IsAny<string>())).Returns(It.IsAny<List<DeliveryPointDTO>>);

            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object);
        }

        [Test]
        public void Test_GetDeliveryPoints()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";
            var result = testCandidate.GetDeliveryPoints(coordinates);
            mockDeliveryPointsRepository.Verify(x => x.GetDeliveryPoints(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(result);
        }

    }
}