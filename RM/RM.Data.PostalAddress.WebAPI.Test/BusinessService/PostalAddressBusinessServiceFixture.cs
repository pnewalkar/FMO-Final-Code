using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.DataDTO;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;

namespace RM.Data.PostalAddress.WebAPI.Test
{
    [TestFixture]
    public class PostalAddressBusinessServiceFixture : TestFixtureBase
    {
        private const string PostalAddressStatus = "Postal Address Status";
        private const string PostalAddressType = "Postal Address Type";

        private IPostalAddressBusinessService testCandidate;
        private Mock<IPostalAddressDataService> mockPostalAddressDataService;
        private Mock<IConfigurationHelper> mockConfigurationHelper;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IHttpHandler> mockHttpHandler;
        private Mock<IPostalAddressIntegrationService> mockPostalAddressIntegrationService;
        // private Mock<IFileProcessingLogDataService> mockFileProcessingLogDataService;
        private PostalAddressDTO publicPostalAddressDTO = default(PostalAddressDTO);
        private Guid addressTypeGUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974");
        private int paf = 0;
        private List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryDTOList;

        [Test]
        public void Test_SavePostalAddressForNYB_PositiveScenario()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { ID = Guid.NewGuid(), UDPRN = 14856 } };
            var result = testCandidate.SavePostalAddressForNYB(lstPostalAddressDTO, "NYB.CSV");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_SavePostalAddressForNYB_NegativeScenario()
        {
            List<PostalAddressDTO> lstPostalAddressDTO = null;
            var result = testCandidate.SavePostalAddressForNYB(lstPostalAddressDTO, string.Empty);
            Assert.IsFalse(result.Result);
        }

        [Test]
        public void Test_GetPostalAddressDetails_PositiveScenario()
        {
            mockPostalAddressDataService.Setup(n => n.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(new PostalAddressDataDTO() { ID = Guid.Empty });
            var result = testCandidate.GetPostalAddressDetails(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetPostalAddressDetails_NegativeScenario()
        {
            mockPostalAddressDataService.Setup(n => n.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(new PostalAddressDataDTO() { ID = Guid.Empty });
            var result = testCandidate.GetPostalAddressDetails(Guid.Empty);
            Assert.NotNull(result);
            Assert.IsTrue(result.ID == Guid.Empty);
        }

        /// <summary>
        /// Insert PAF record in Postal address
        /// </summary>
        [Test]
        public void SavePAFDetails_Check_MatchPostalAddressOnAddress_Insert()
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

            var result = testCandidate.ProcessPAFDetails(lstPostalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        /// <summary>
        /// Update record in Postal address and update DeliveryPointUseIndicatorGUID in delivery point table
        /// </summary>
        [Test]
        public void ProcessPAFDetails_Update_PositiveScenario()
        {
            mockPostalAddressIntegrationService.Setup(n => n.UpdateDPUse(It.IsAny<PostalAddressDTO>())).ReturnsAsync(true);
            mockPostalAddressDataService.Setup(x => x.UpdateAddress(It.IsAny<PostalAddressDataDTO>(), It.IsAny<string>())).ReturnsAsync(true);
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "C",
                AmendmentDesc = "update",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            var result = testCandidate.ProcessPAFDetails(lstPostalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        /// <summary>
        /// Matching UDPRN not found for postal address
        /// </summary>
        [Test]
        public void ProcessPAFDetails_Update_NegativeScenario1()
        {
            mockPostalAddressIntegrationService.Setup(n => n.UpdateDPUse(It.IsAny<PostalAddressDTO>())).ReturnsAsync(true);
            mockPostalAddressDataService.Setup(x => x.UpdateAddress(It.IsAny<PostalAddressDataDTO>(), It.IsAny<string>())).ReturnsAsync(false);
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "C",
                AmendmentDesc = "update",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            var result = testCandidate.ProcessPAFDetails(lstPostalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        /// <summary>
        /// Matching Delivery point not found for given UDPRN
        /// </summary>
        [Test]
        public void ProcessPAFDetails_Update_NegativeScenario2()
        {
            mockPostalAddressIntegrationService.Setup(n => n.UpdateDPUse(It.IsAny<PostalAddressDTO>())).ReturnsAsync(false);
            mockPostalAddressDataService.Setup(x => x.UpdateAddress(It.IsAny<PostalAddressDataDTO>(), It.IsAny<string>())).ReturnsAsync(true);
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "C",
                AmendmentDesc = "update",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            var result = testCandidate.ProcessPAFDetails(lstPostalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        /// <summary>
        /// Update postal address status to Pending delete
        /// </summary>
        [Test]
        public void ProcessPAFDetails_Delete_PositiveScenario()
        {
            mockPostalAddressDataService.Setup(x => x.UpdatePostalAddressStatus(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "D",
                AmendmentDesc = "delete",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            var result = testCandidate.ProcessPAFDetails(lstPostalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        /// <summary>
        /// No matching UDPRN found for postal address
        /// </summary>
        [Test]
        public void ProcessPAFDetails_Delete_NegativeScenario1()
        {
            PostalAddressDataDTO postalAddressDataDTO = null;
            mockPostalAddressDataService.Setup(n => n.GetPostalAddress(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDataDTO));
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "D",
                AmendmentDesc = "delete",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            var result = testCandidate.ProcessPAFDetails(lstPostalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        /// <summary>
        /// Postal Address type is not PAF
        /// </summary>
        [Test]
        public void ProcessPAFDetails_Delete_NegativeScenario2()
        {
            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO() { AddressType_GUID = Guid.NewGuid() };
            mockPostalAddressDataService.Setup(n => n.GetPostalAddress(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDataDTO));
            PostalAddressDTO objPostalAddress = new PostalAddressDTO()
            {
                Time = "7/19/2016",
                Date = "8:37:00",
                AmendmentType = "D",
                AmendmentDesc = "delete",
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("C08C5212-6123-4EAF-9C27-D4A8035A8974")
            };
            List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
            lstPostalAddress.Add(objPostalAddress);
            var result = testCandidate.ProcessPAFDetails(lstPostalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_GetPostalAddress_PositiveScenario()
        {
            var result = testCandidate.GetPostalAddress(12345);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_CheckForDuplicateNybRecords()
        {
            var result = testCandidate.CheckForDuplicateNybRecords(publicPostalAddressDTO);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, "PO1234");
        }

        [Test]
        public async Task Test_CheckForDuplicateAddressWithDeliveryPoints_PositiveScenario()
        {
            bool result = await testCandidate.CheckForDuplicateAddressWithDeliveryPoints(publicPostalAddressDTO);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_CreateAddressForDeliveryPoint_PositiveScenario()
        {
            AddDeliveryPointDTO addDeliveryPointDTO = new AddDeliveryPointDTO()
            {
                PostalAddressDTO = new PostalAddressDTO()
                {
                    Postcode = "1234",
                    ID = Guid.Empty
                },
                DeliveryPointDTO = new DeliveryPointDTO()
                {
                    PostalAddress = new PostalAddressDTO()
                    {
                        Postcode = "1234",
                        ID = Guid.Empty
                    }
                },
                PostalAddressAliasDTOs = new List<PostalAddressAliasDTO>()
                {
                    new PostalAddressAliasDTO()
                    {
                    }
                }
            };
            var result = testCandidate.CreateAddressForDeliveryPoint(addDeliveryPointDTO);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Test_GetPAFAddress()
        {
            paf = 123;
            PostalAddressDTO expectedresult = await testCandidate.GetPAFAddress(paf);
            Assert.IsNotNull(expectedresult);
        }

        [Test]
        public void Test_GetPostalAddresses()
        {
            var result = testCandidate.GetPostalAddresses(new List<Guid>() { Guid.NewGuid() });
            Assert.IsNotNull(result);
        }

        protected override void OnSetup()
        {
            // OnSetup to be configured
            referenceDataCategoryDTOList = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>()
             {
                 new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO()
                 {
                     ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>()
                     {
                         new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                         {
                             ReferenceDataValue = FileType.Nyb.ToString(),
                             ID = Guid.NewGuid(),
                         }
                     },
                     CategoryName = PostalAddressType
                 },

                 new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO()
                 {
                     ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>()
                     {
                         new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                         {
                             ReferenceDataValue = FileType.Usr.ToString(),
                             ID = Guid.NewGuid(),
                         }
                     },
                     CategoryName = PostalAddressType
                 },

                 new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO()
                 {
                     ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>()
                     {
                         new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                         {
                             ReferenceDataValue = FileType.Paf.ToString(),
                             ID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974"),
                         }
                     },
                     CategoryName = PostalAddressType
                 },

                 new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO()
                 {
                     ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>()
                     {
                         new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                         {
                             ReferenceDataValue = PostCodeStatus.Live.GetDescription(),
                             ID = Guid.NewGuid(),
                         }
                     },
                     CategoryName = PostalAddressStatus
                 },

                 new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO()
                 {
                     ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>()
                     {
                         new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                         {
                             ReferenceDataValue = PostCodeStatus.PendingDeleteInFMO.GetDescription(),
                             ID = Guid.NewGuid(),
                         }
                     },
                     CategoryName = PostalAddressStatus
                 }
             };

            AddressLocationDTO addressLocationDTO = new AddressLocationDTO()
            {
                UDPRN = 1234,
                LocationXY = DbGeometry.PointFromText("POINT(512722.70000000019 104752.6799999997)", 27700)
            };

            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO()
            {
                Postcode = "YO23 1DQ",
                PostTown = "York",
                UDPRN = 54162429,
                DeliveryPointSuffix = "1A",
                AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
            };

            PostalAddressDTO postalAddressDTO = new PostalAddressDTO()
            {
                ID = Guid.Empty
            };

            mockPostalAddressDataService = CreateMock<IPostalAddressDataService>();
            // mockFileProcessingLogDataService = CreateMock<IFileProcessingLogDataService>();
            mockConfigurationHelper = CreateMock<IConfigurationHelper>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockHttpHandler = CreateMock<IHttpHandler>();
            mockPostalAddressIntegrationService = CreateMock<IPostalAddressIntegrationService>();

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);
            mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<string>())).Returns(Task.FromResult(referenceDataCategoryDTOList[2]));
            mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryDTOList));
            mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));
            mockPostalAddressIntegrationService.Setup(n => n.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(Task.FromResult(addressLocationDTO));
            mockPostalAddressDataService.Setup(n => n.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));

            mockPostalAddressDataService.Setup(n => n.GetPostalAddresses(It.IsAny<List<Guid>>())).ReturnsAsync(new List<PostalAddressDataDTO>() { new PostalAddressDataDTO() { Postcode = "1234" } });
            mockPostalAddressDataService.Setup(n => n.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDataDTO>(), It.IsAny<Guid>())).Returns("PO1234");
            mockPostalAddressDataService.Setup(n => n.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDataDTO>())).Returns(Task.FromResult(true));
            mockPostalAddressDataService.Setup(n => n.CreateAddressForDeliveryPoint(It.IsAny<PostalAddressDataDTO>())).Returns(addressTypeGUID);
            mockPostalAddressDataService.Setup(n => n.GetPostalAddress(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDataDTO));
            mockPostalAddressDataService.Setup(n => n.GetPAFAddress(It.IsAny<int>(), It.IsAny<Guid>())).Returns(Task.FromResult(postalAddressDTO));

            // mockFileProcessingLogDataService.Setup(x => x.LogFileException(It.IsAny<FileProcessingLogDTO>()));

            testCandidate = new DataManagement.PostalAddress.WebAPI.BusinessService.Implementation.PostalAddressBusinessService(mockPostalAddressDataService.Object, mockLoggingHelper.Object, mockConfigurationHelper.Object, mockHttpHandler.Object, mockPostalAddressIntegrationService.Object);
        }
    }
}