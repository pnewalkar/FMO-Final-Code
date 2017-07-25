using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.Data.PostalAddress.WebAPI.Test
{
    [TestFixture]
    public class PostalAddressBusinessServiceFixture : TestFixtureBase
    {
        private const string PostalAddressStatus = "Postal Address Status";
        private const string PostalAddressType = "Postal Address Type";

        // private Mock<IPostalAddressDataService> mockPostalAddressDataService;
        // private Mock<IFileProcessingLogDataService> mockFileProcessingLogDataService;
        // private Mock<IConfigurationHelper> mockConfigurationHelper;
        // private Mock<ILoggingHelper> mockLoggingHelper;
        // private Mock<IHttpHandler> mockHttpHandler;
        // private Mock<IPostalAddressIntegrationService> mockPostalAddressIntegrationService;
        // private RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface.IPostalAddressBusinessService testCandidate;
        // private PostalAddressDataDTO postalAddressDTO;
        // private PostalAddressDTO publicPostalAddressDTO;
        // private AddDeliveryPointDTO addDeliveryPointDTO;
        // private List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryDTOList;

        //TODO: Nunits to be fixed
        //[Test]
        //public void Test_ValidPostalAddressData()
        //{
        //    List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { ID = Guid.NewGuid(), UDPRN = 14856 } };
        //    var result = testCandidate.SavePostalAddressForNYB(lstPostalAddressDTO, "NYB.CSV");
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Result);
        //}

        //[Test]
        //public void Test_InvalidPostalAddressData()
        //{
        //    List<PostalAddressDTO> lstPostalAddressDTO = new List<PostalAddressDTO>() { new PostalAddressDTO() { ID = Guid.NewGuid() } };
        //    var result = testCandidate.SavePostalAddressForNYB(lstPostalAddressDTO, "NYB.CSV");
        //    Assert.IsNotNull(result);
        //    Assert.IsFalse(result.Result);
        //}

        //[Test]
        //public void Test_GetPostalAddressDetails()
        //{
        //    var result = testCandidate.GetPostalAddressDetails(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void SavePAFDetails_Check_MatchPostalAddressOnAddress()
        //{
        //    PostalAddressDTO objPostalAddress = new PostalAddressDTO()
        //    {
        //        Time = "7/19/2016",
        //        Date = "8:37:00",
        //        AmendmentType = "I",
        //        AmendmentDesc = "new insert",
        //        Postcode = "YO23 1DQ",
        //        PostTown = "York",
        //        UDPRN = 54162429,
        //        DeliveryPointSuffix = "1A",
        //        AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
        //    };
        //    List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
        //    lstPostalAddress.Add(objPostalAddress);
        //    AddressLocationDTO objAddressLocation = new AddressLocationDTO()
        //    {
        //        UDPRN = 54162428
        //    };

        //    var result = testCandidate.SavePAFDetails(lstPostalAddress);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Result);
        //}

        //[Test]
        //public void SavePAFDetails_Check_NotMatchPostalAddress()
        //{
        //    PostalAddressDTO objPostalAddress = new PostalAddressDTO()
        //    {
        //        Time = "7/19/2016",
        //        Date = "8:37:00",
        //        AmendmentType = "I",
        //        AmendmentDesc = "new insert",
        //        Postcode = "YO23 1DQ",
        //        PostTown = "York",
        //        UDPRN = 54162427,
        //        DeliveryPointSuffix = "1A",
        //        AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8971")
        //    };
        //    List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();
        //    lstPostalAddress.Add(objPostalAddress);
        //    AddressLocationDTO objAddressLocation = new AddressLocationDTO()
        //    {
        //        UDPRN = 54162426
        //    };

        //    var result = testCandidate.SavePAFDetails(lstPostalAddress);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.Result);
        //}

        //[Test]
        //public async Task Test_SearchByPostcode()
        //{
        //    var result = await testCandidate.GetPostalAddressDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public async Task Test_PostalAddressSearchDetails()
        //{
        //    PostalAddressDTO objPostalAddress = new PostalAddressDTO()
        //    {
        //        Time = "7/19/2016",
        //        Date = "8:37:00",
        //        AmendmentType = "I",
        //        AmendmentDesc = "new insert",
        //        Postcode = "YO23 1DQ",
        //        PostTown = "York",
        //        UDPRN = 54162429,
        //        DeliveryPointSuffix = "1A",
        //        AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
        //    };

        //    List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>() { objPostalAddress };

        //    var result = await testCandidate.GetPostalAddressSearchDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_GetPostalAddress()
        //{
        //    mockPostalAddressDataService.Setup(n => n.GetPostalAddress(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDTO));
        //    var result = testCandidate.GetPostalAddress(12345);
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_CheckForDuplicateNybRecords()
        //{
        //    mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<string>())).Returns(Task.FromResult(referenceDataCategoryDTOList[0]));
        //    mockPostalAddressDataService.Setup(n => n.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDataDTO>(), It.IsAny<Guid>())).Returns("abc");
        //    var result = testCandidate.CheckForDuplicateNybRecords(publicPostalAddressDTO);
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(result, "abc");
        //}

        //[Test]
        //public void Test_CheckForDuplicateAddressWithDeliveryPoints()
        //{
        //    mockPostalAddressDataService.Setup(n => n.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDataDTO>())).Returns(true);
        //    var result = testCandidate.CheckForDuplicateAddressWithDeliveryPoints(publicPostalAddressDTO);
        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result);
        //}

        //[Test]
        //public void Test_CreateAddressAndDeliveryPoint()
        //{
        //    mockPostalAddressDataService.Setup(n => n.CreateAddressAndDeliveryPoint(It.IsAny<AddDeliveryPointDTO>(), It.IsAny<Guid>())).Returns(new CreateDeliveryPointModelDTO() { ID = Guid.NewGuid() });
        //    var result = testCandidate.CreateAddressAndDeliveryPoint(addDeliveryPointDTO);
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void Test_GetPostalAddresses()
        //{
        //    var result = testCandidate.GetPostalAddresses(new List<Guid>() { Guid.NewGuid() });
        //    Assert.IsNotNull(result);
        //}
        protected override void OnSetup()
        {
            //OnSetup to be configured

            /*  referenceDataCategoryDTOList = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>()
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
                     CategoryName= PostalAddressType
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
                     CategoryName= PostalAddressStatus
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
                     CategoryName= PostalAddressStatus
                 }
             };

             postalAddressDTO = new PostalAddressDataDTO()
             {
                 Postcode = "YO23 1DQ",
                 PostTown = "York",
                 UDPRN = 54162429,
                 DeliveryPointSuffix = "1A",
                 AddressType_GUID = new Guid("A08C5212-6123-4EAF-9C27-D4A8035A8974")
             };

             List<PostalAddressDataDTO> lstPostalAddress = new List<PostalAddressDataDTO>() { postalAddressDTO };

             addDeliveryPointDTO = new AddDeliveryPointDTO()
             {
                 PostalAddressDTO = new PostalAddressDataDTO()
                 {
                     PostCodeGUID = Guid.NewGuid(),
                     AddressType_GUID = Guid.NewGuid(),
                     AddressStatus_GUID = Guid.NewGuid()
                 }
             };

             mockPostalAddressDataService = CreateMock<IPostalAddressDataService>();
             mockFileProcessingLogDataService = CreateMock<IFileProcessingLogDataService>();
             mockConfigurationHelper = CreateMock<IConfigurationHelper>();
             mockLoggingHelper = CreateMock<ILoggingHelper>();
             mockHttpHandler = CreateMock<IHttpHandler>();
             mockPostalAddressIntegrationService = CreateMock<IPostalAddressIntegrationService>();

             var rmTraceManagerMock = new Mock<IRMTraceManager>();
             rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
             mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

             mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<string>())).Returns(Task.FromResult(referenceDataCategoryDTOList[2]));
             mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryDTOList));
             mockPostalAddressIntegrationService.Setup(n => n.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));
             mockPostalAddressIntegrationService.Setup(n => n.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));

             mockPostalAddressDataService.Setup(n => n.SaveAddress(It.IsAny<PostalAddressDataDTO>(), It.IsAny<string>())).Returns(Task.FromResult(true));
             mockPostalAddressDataService.Setup(n => n.DeleteNYBPostalAddress(It.IsAny<List<int>>(), It.IsAny<Guid>())).Returns(Task.FromResult(true));
             mockPostalAddressDataService.Setup(n => n.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(new PostalAddressDataDTO() { });
             mockPostalAddressDataService.Setup(n => n.GetPostalAddressDetails(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<List<CommonLibrary.EntityFramework.DTO.PostCodeDTO>>())).Returns(Task.FromResult(lstPostalAddress));
             mockPostalAddressDataService.Setup(n => n.GetPostalAddressSearchDetails(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<List<Guid>>(), It.IsAny<List<CommonLibrary.EntityFramework.DTO.PostCodeDTO>>())).Returns(Task.FromResult(new List<string>() { "abc" }));
             mockPostalAddressDataService.Setup(n => n.GetPostalAddresses(It.IsAny<List<Guid>>())).ReturnsAsync(new List<PostalAddressDataDTO>() { });

             /* testCandidate = new DataManagement.PostalAddress.WebAPI.BusinessService.Implementation.PostalAddressBusinessService(mockPostalAddressDataService.Object, mockFileProcessingLogDataService.Object, mockLoggingHelper.Object, mockConfigurationHelper.Object, mockHttpHandler.Object, mockPostalAddressIntegrationService.Object);*/
        }
    }
}