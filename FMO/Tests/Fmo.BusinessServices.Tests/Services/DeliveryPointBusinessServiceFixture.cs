using System;

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
        private Mock<IAddressLocationRepository> mockaddressLocationRepository;
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
            objdeliverypointDTO.DeliveryPoint_Id = 11820021;
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
            mockDeliveryPointsRepository = new Mock<IDeliveryPointsRepository>();
            mockaddressLocationRepository = new Mock<IAddressLocationRepository>();

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

            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object, mockaddressLocationRepository.Object);
        }
    }
}