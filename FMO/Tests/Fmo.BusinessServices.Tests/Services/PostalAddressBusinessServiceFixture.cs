using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MessageBrokerCore.Messaging;
using Fmo.NYBLoader;
using Fmo.NYBLoader.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class PostalAddressBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IAddressRepository> mockAddressRepository;
        private Mock<IReferenceDataCategoryRepository> mockrefDataRepository;
        private Mock<IDeliveryPointsRepository> mockdeliveryPointsRepository;
        private Mock<IAddressLocationRepository> mockaddressLocationRepository;
        private IPostalAddressBusinessService testCandidate;

        [Test]
        public void Test_ValidPostalAddressData()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { Address_Id = 10, UDPRN = 14856 } };
            var result = testCandidate.SavePostalAddress(lstPostalAddressDTO);
            mockrefDataRepository.Verify(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockAddressRepository.Verify(x => x.SaveAddress(It.IsAny<PostalAddressDTO>()), Times.Once());
            mockAddressRepository.Verify(x => x.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<int>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_InvalidPostalAddressData()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { Address_Id = 10 } };
            var result = testCandidate.SavePostalAddress(lstPostalAddressDTO);
            mockrefDataRepository.Verify(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            mockAddressRepository.Verify(x => x.SaveAddress(It.IsAny<PostalAddressDTO>()), Times.Once());
            mockAddressRepository.Verify(x => x.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<int>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        protected override void OnSetup()
        {
            mockAddressRepository = CreateMock<IAddressRepository>();
            mockrefDataRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockdeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            mockaddressLocationRepository = CreateMock<IAddressLocationRepository>();
            mockrefDataRepository.Setup(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>())).Returns(2);
            mockAddressRepository.Setup(x => x.SaveAddress(It.IsAny<PostalAddressDTO>())).Returns(true);
            mockAddressRepository.Setup(x => x.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<int>())).Returns(true);

            testCandidate = new PostalAddressBusinessService(mockAddressRepository.Object, mockrefDataRepository.Object, mockdeliveryPointsRepository.Object, mockaddressLocationRepository.Object);
        }

    }
}
