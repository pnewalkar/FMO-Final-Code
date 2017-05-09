using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.Interface;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.Helpers.Interface;
using Moq;
using NUnit.Framework;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private IDeliveryPointBusinessService testCandidate;
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private Mock<IAddressLocationRepository> mockaddressLocationRepository;
        private Mock<IConfigurationHelper> mockConfigurationRepository;
        private Mock<ILoggingHelper> mockLoggingRepository;
        private Mock<IAddressRepository> mockAddressRepository;
        private Guid unitGuid = Guid.NewGuid();

        [Test]
        public void Test_GetDeliveryPoints()
        {
            string coordinates = "399545.5590911182,649744.6394892789,400454.4409088818,650255.3605107211";

            var result = testCandidate.GetDeliveryPoints(coordinates, unitGuid);
            mockDeliveryPointsRepository.Verify(x => x.GetDeliveryPoints(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetDeliveryPointByUDPRN()
        {
            int udprn = 10875813;
            List<DeliveryPointDTO> lstDeliveryPointDTO = new List<DeliveryPointDTO>();
            DeliveryPointDTO objdeliverypointDTO = new DeliveryPointDTO();
            objdeliverypointDTO.ID = Guid.NewGuid();
            objdeliverypointDTO.LocationXY = System.Data.Entity.Spatial.DbGeometry.PointFromText("POINT (487431 193658)", 27700);
            objdeliverypointDTO.PostalAddress = new PostalAddressDTO();
            lstDeliveryPointDTO.Add(objdeliverypointDTO);

            mockDeliveryPointsRepository.Setup(x => x.GetDeliveryPointListByUDPRN(It.IsAny<int>())).Returns(It.IsAny<List<DeliveryPointDTO>>);
            var coordinates = testCandidate.GetDeliveryPointByUDPRN(udprn);
            mockDeliveryPointsRepository.Verify(x => x.GetDeliveryPointListByUDPRN(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(coordinates);

            // Assert.AreEqual(lstDeliveryPointDTO, coordinates);
        }

        protected override void OnSetup()
        {
            mockDeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            mockaddressLocationRepository = CreateMock<IAddressLocationRepository>();
            mockConfigurationRepository = CreateMock<IConfigurationHelper>();
            mockLoggingRepository = CreateMock<ILoggingHelper>();
            mockAddressRepository = CreateMock<IAddressRepository>();

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
            mockDeliveryPointsRepository.Setup(x => x.GetDeliveryPoints(It.IsAny<string>(), It.IsAny<Guid>())).Returns(It.IsAny<List<DeliveryPointDTO>>);

            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object, mockaddressLocationRepository.Object, mockAddressRepository.Object, mockLoggingRepository.Object, mockConfigurationRepository.Object, unitOfWorkMock.Object);
        }
    }
}