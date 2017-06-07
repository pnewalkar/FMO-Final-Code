using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Implementation;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;

namespace RM.Data.PostalAddress.WebAPI.Test
{
    [TestFixture]
    public class PostalAddressBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IPostalAddressDataService> mockPostalAddressDataService;
        private Mock<IFileProcessingLogDataService> mockFileProcessingLogDataService;
        private Mock<IConfigurationHelper> mockConfigurationHelper;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IHttpHandler> mockHttpHandler;
        private Mock<IPostalAddressIntegrationService> mockPostalAddressIntegrationService;
        private IPostalAddressBusinessService testCandidate;

        [Test]
        public void Test_ValidPostalAddressData()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { ID = Guid.NewGuid(), UDPRN = 14856 } };
            //  var result = testCandidate.SavePostalAddress(lstPostalAddressDTO, "NYB.CSV");            
            // Assert.IsNotNull(result);
            // Assert.IsTrue(result);
        }

        [Test]
        public void Test_InvalidPostalAddressData()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { ID = Guid.NewGuid() } };
            //var result = testCandidate.SavePostalAddress(lstPostalAddressDTO, "NYB.CSV");
            //mockrefDataRepository.Verify(x => x.GetReferenceDataId(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(2));
            //mockAddressRepository.Verify(x => x.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>()), Times.Never());
            //mockAddressRepository.Verify(x => x.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<Guid>()), Times.Never());
            //Assert.IsNotNull(result);
            //Assert.IsFalse(result);
        }

        [Test]
        public void Test_GetPostalAddressDetails()
        {
            var result = testCandidate.GetPostalAddressDetails(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            Assert.IsNotNull(result);
        }

        [Test]
        public void SavePAFDetails_Check_MatchPostalAddressOnAddress()
        {
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "I",
                AmendmentDesc = "new insert",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            AddressLocationDTO objAddressLocation = new AddressLocationDTO()
            {
                UDPRN = 54162428
            };


            var result = testCandidate.SavePAFDetails(lstPostalAddress);

        }

        [Test]
        public void SavePAFDetails_Check_NotMatchPostalAddress()
        {
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "I",
                AmendmentDesc = "new insert",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            AddressLocationDTO objAddressLocation = new AddressLocationDTO()
            {
                UDPRN = 54162428
            };


            var result = testCandidate.SavePAFDetails(lstPostalAddress);


        }

        [Test]
        public async Task Test_SearchByPostcode()
        {
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "I",
                AmendmentDesc = "new insert",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };

            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>() { objPostalAddress };

            await testCandidate.GetPostalAddressDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));

        }

        [Test]
        public async Task Test_PostalAddressSearchDetails()
        {
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "I",
                AmendmentDesc = "new insert",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };

            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>() { objPostalAddress };

            await testCandidate.GetPostalAddressSearchDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));

        }
        protected override void OnSetup()
        {
            mockPostalAddressDataService = CreateMock<IPostalAddressDataService>();
            mockFileProcessingLogDataService = CreateMock<IFileProcessingLogDataService>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockHttpHandler = CreateMock<IHttpHandler>();
            mockPostalAddressIntegrationService = CreateMock<IPostalAddressIntegrationService>();
            testCandidate = new PostalAddressBusinessService(mockPostalAddressDataService.Object, mockFileProcessingLogDataService.Object, mockLoggingHelper.Object, mockConfigurationHelper.Object, mockHttpHandler.Object, mockPostalAddressIntegrationService.Object);

        }
    }


}
