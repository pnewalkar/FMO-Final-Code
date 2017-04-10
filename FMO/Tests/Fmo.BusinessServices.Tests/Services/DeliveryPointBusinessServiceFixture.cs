namespace Fmo.BusinessServices.Tests.Services
{
    using System.Collections.Generic;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;
    using BusinessServices.Services;
    using Entities;
    using DataServices.DBContext;
    using DataServices.Infrastructure;

    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private IDeliveryPointBusinessService testCandidate;
        private Mock<DeliveryPointBusinessService> mockDeliveryPointBussinessService;
        DeliveryPointDTO deliveryPointDTO;
        string coordinates;
        //private Mock<FMODBContext> mockFmoDbContext;

        //private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;

        protected override void OnSetup()
        {
            deliveryPointDTO = new DeliveryPointDTO();
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

            //mockFmoDbContext = CreateMock<FMODBContext>();
            //mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();

             coordinates = "1234.55";
            mockDeliveryPointBussinessService = new Mock<DeliveryPointBusinessService>();
            mockDeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object);
            mockDeliveryPointsRepository.Setup(n => n.GetDeliveryPoints1(It.IsAny<string>())).Returns(lstDeliveryPointDTO);
            mockDeliveryPointsRepository.Setup(n => n.GetData(It.IsAny<string>())).Returns(lstDeliveryPoint);
            mockDeliveryPointBussinessService.Setup(n => n.GetDeliveryPoints1(It.IsAny<string>())).Returns(deliveryPointDTO);
            mockDeliveryPointBussinessService.Setup(n => n.GetData(It.IsAny<string>(), It.IsAny<object[]>())).Returns(coordinates);
        }

        public void TestDeliveryPointGetData()
        {
            mockDeliveryPointBussinessService.Verify(x => x.GetDeliveryPoints1(It.IsAny<string>()), Times.Once);
            Assert.IsNotNull(deliveryPointDTO);
        }

        public void TestGetData()
        {
            mockDeliveryPointBussinessService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
            Assert.IsNotNull(coordinates);
        }
    }
}