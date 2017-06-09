﻿using System;
using System.Collections.Generic;
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
        private PostalAddressDTO postalAddressDTO;

        [Test]
        public void Test_ValidPostalAddressData()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { ID = Guid.NewGuid(), UDPRN = 14856 } };
            var result = testCandidate.SavePostalAddressForNYB(lstPostalAddressDTO, "NYB.CSV");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_InvalidPostalAddressData()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { ID = Guid.NewGuid() } };
            var result = testCandidate.SavePostalAddressForNYB(lstPostalAddressDTO, "NYB.CSV");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
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
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
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
                UDPRN = 54162427,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8971")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            AddressLocationDTO objAddressLocation = new AddressLocationDTO()
            {
                UDPRN = 54162426
            };

            var result = testCandidate.SavePAFDetails(lstPostalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public async Task Test_SearchByPostcode()
        {
            var result = await testCandidate.GetPostalAddressDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.IsNotNull(result);
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

            var result = await testCandidate.GetPostalAddressSearchDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            List<ReferenceDataCategoryDTO> referenceDataCategoryDTOList = new List<ReferenceDataCategoryDTO>()
            {
                new ReferenceDataCategoryDTO()
                {
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataValue = FileType.Nyb.ToString(),
                            ID = Guid.NewGuid(),
                        }
                    },
                    CategoryName= Constants.PostalAddressType
                },

                new ReferenceDataCategoryDTO()
                {
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataValue = PostCodeStatus.Live.GetDescription(),
                            ID = Guid.NewGuid(),
                        }
                    },
                    CategoryName= Constants.PostalAddressStatus
                },

                new ReferenceDataCategoryDTO()
                {
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataValue = FileType.Usr.ToString(),
                            ID = Guid.NewGuid(),
                        }
                    },
                    CategoryName= Constants.PostalAddressStatus
                }
            };

            postalAddressDTO = new PostalAddressDTO()
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

            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>() { postalAddressDTO };

            mockPostalAddressDataService = CreateMock<IPostalAddressDataService>();
            mockFileProcessingLogDataService = CreateMock<IFileProcessingLogDataService>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockHttpHandler = CreateMock<IHttpHandler>();
            mockPostalAddressIntegrationService = CreateMock<IPostalAddressIntegrationService>();

            mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<string>())).Returns(Task.FromResult(referenceDataCategoryDTOList[2]));
            mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryDTOList));
            mockPostalAddressIntegrationService.Setup(n => n.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));
            mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));

            mockPostalAddressDataService.Setup(n => n.SaveAddress(It.IsAny<PostalAddressDTO>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            mockPostalAddressDataService.Setup(n => n.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
            mockPostalAddressDataService.Setup(n => n.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(new PostalAddressDTO() { });
            mockPostalAddressDataService.Setup(n => n.GetPostalAddressDetails(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(lstPostalAddress));
            mockPostalAddressDataService.Setup(n => n.GetPostalAddressSearchDetails(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<List<Guid>>())).Returns(Task.FromResult(new List<string>() { "abc" }));

            testCandidate = new PostalAddressBusinessService(mockPostalAddressDataService.Object, mockFileProcessingLogDataService.Object, mockLoggingHelper.Object, mockConfigurationHelper.Object, mockHttpHandler.Object, mockPostalAddressIntegrationService.Object);
        }
    }
}