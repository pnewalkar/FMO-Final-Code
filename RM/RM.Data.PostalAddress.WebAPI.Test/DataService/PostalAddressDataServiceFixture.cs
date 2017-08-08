using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.DataDTO;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Implementation;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.PostalAddress.WebAPI.Test.DataService
{
    [TestFixture]
    public class AddressDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<PostalAddressDBContext> mockPostalAddressDbContext;
        private Mock<ILoggingHelper> mockLoggingHelper;

        // private Mock<IFileProcessingLogDataService> mockFileProcessingLog;
        private Mock<IDatabaseFactory<PostalAddressDBContext>> mockDatabaseFactory;

        private Mock<IPostalAddressDataService> mockAddressDataService;

        private IPostalAddressDataService testCandidate;
        private List<PostalAddressDataDTO> postalAddressesDataDTO;
        private List<PostalAddressDTO> postalAddressesDTO;
        private AddDeliveryPointDTO addDeliveryPointDTO1;
        private AddDeliveryPointDTO addDeliveryPointDTO2;
        private AddDeliveryPointDTO addDeliveryPointDTO3;
        private PostalAddressDataDTO dtoPostalAddresses;
        private List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> lstPostCodeDTO;

        private PostalAddressDataDTO testObject;
        private Guid id1 = new Guid("00000000-0000-0000-0000-000000000002");

        private Guid id2 = new Guid("00000000-0000-0000-0000-000000000001");

        [Test]
        public void Test_UpdateAddressValidTestCase()
        {
            this.SetUpdataWithDeliverypoints();
            var result = testCandidate.SaveAddress(dtoPostalAddresses, "NYB.CSV", new Guid("EE479380-C4F7-4FA3-96B2-DD54A7091BAA"));
            Assert.NotNull(result);
        }

        [Test]
        public void Test_CreateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDataDTO objstPostalAddressDTO = new PostalAddressDataDTO() { UDPRN = 54471821 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV", new Guid("EE479380-C4F7-4FA3-96B2-DD54A7091BAA"));
            Assert.NotNull(result);
        }

        [Test]
        public void Test_InValidAddress()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDataDTO objstPostalAddressDTO = null;
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV", new Guid("EE479380-C4F7-4FA3-96B2-DD54A7091BAA"));
            mockPostalAddressDbContext.Verify(n => n.SaveChangesAsync(), Times.Never);
            Assert.NotNull(result);
            Assert.IsFalse(result.Result);
        }

        [Test]
        public void Test_GetPostalAddress()
        {
            SetUpdataWithDeliverypoints();
            var result = testCandidate.GetPostalAddress(dtoPostalAddresses);
            Assert.NotNull(result);
        }

        [Test]
        public void Test_UpdateAddress()
        {
            SetUpdataWithDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 53942347, 54502893 };
            var result = testCandidate.UpdateAddress(dtoPostalAddresses, "abc");
            Assert.NotNull(result);
        }

        [Test]
        public void Test_GetPostalAddressDetails()
        {
            SetUpdataPostalAddressDetails();
            var result = testCandidate.GetPostalAddressDetails(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            Assert.NotNull(result);
            Assert.NotNull(result.ID);
        }

        [Test]
        public void Test_CheckForDuplicateNybRecords_Duplicate()
        {
            SetUpDataForDeliveryPoints();
            string results = testCandidate.CheckForDuplicateNybRecords(postalAddressesDataDTO[0], new Guid("222C68A4-D959-4B37-B468-4B1855950A81"));
            Assert.NotNull(results);
            Assert.IsEmpty(results.ToString());
        }

        [Test]
        public void Test_CheckForDuplicateNybRecords_NotDuplicate()
        {
            SetUpDataForDeliveryPoints();
            string results = testCandidate.CheckForDuplicateNybRecords(postalAddressesDataDTO[1], new Guid("222C68A4-D959-4B37-B468-4B1855950A81"));
            Assert.NotNull(results);
            Assert.IsTrue(results == string.Empty);
        }

        [Test]
        public void Test_CreateAddressForDeliveryPoint()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var result = testCandidate.CreateAddressForDeliveryPoint(postalAddressesDataDTO[0]);
            Assert.NotNull(result);
            Assert.IsTrue(result != Guid.Empty);
        }

        [Test]
        public void Test_CheckForDuplicateAddressWithDeliveryPoints()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var result = testCandidate.CheckForDuplicateAddressWithDeliveryPoints(postalAddressesDataDTO[0]);
            Assert.NotNull(result);
        }

        [Test]
        public void Test_InsertAddress()
        {
            OnSetupPaf();
            testObject = new PostalAddressDataDTO()
            {
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                UDPRN = 14856
            };
            var result = testCandidate.InsertAddress(testObject, "PAFTestFile.csv");
            mockPostalAddressDbContext.Verify(n => n.SaveChangesAsync(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_GetPostalAddressByUDPRN()
        {
            OnSetupPaf();
            testObject = new PostalAddressDataDTO()
            {
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                UDPRN = 14856
            };
            var result = testCandidate.GetPostalAddress(testObject.UDPRN);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetPostalAddresses()
        {
            SetUpdataWithDeliverypoints();
            var result = testCandidate.GetPostalAddresses(new List<Guid>() { new Guid("222C68A4-D959-4B37-B468-4B1855950A81") });
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_DeleteNYBPostalAddress()
        {
            SetUpdataPostalAddressDetails();
            var result = testCandidate.DeleteNYBPostalAddress(new List<int> { 54471821, 53942347 }, new Guid("222C68A4-D959-4B37-B468-4B1855950A81"));
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_GetPAFAddress()
        {
            OnSetupPaf();
            var result = testCandidate.GetPAFAddress(54471821, new Guid("A21F3E46-2D0D-4989-A5D5-872D23B479A2"));
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Postal address status updated
        /// </summary>
        [Test]
        public void Test_UpdatePostalAddressStatus()
        {
            OnSetupPaf();
            var result = testCandidate.UpdatePostalAddressStatus(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"), new Guid("A21F3E46-2D0D-4989-A5D5-872D23B479A2"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        /// <summary>
        /// Postal address status not updated
        /// </summary>
        [Test]
        public void Test_UpdatePostalAddressStatus_NegativeScenario1()
        {
            OnSetupPaf();
            var result = testCandidate.UpdatePostalAddressStatus(new Guid("119DBBBB-03FB-489C-8C8D-F1085E0D2A15"), new Guid("A21F3E46-2D0D-4989-A5D5-872D23B479A2"));
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
        }

        /// <summary>
        /// Delete postal Address details
        /// </summary>
        [Test]
        public void Test_DeletePostalAddress()
        {
            SetUpdataForDeletePostalAddress();
            Guid id = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA");
            var result = testCandidate.DeletePostalAddress(id);
            mockPostalAddressDbContext.Verify(n => n.SaveChangesAsync(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public async Task TestCheckForDuplicateNybRecordsForRange_WithDuplicates()
        {
            var postalAddress = new List<PostalAddressDataDTO>
            {
                new PostalAddressDataDTO()
                {
                    BuildingName = "road bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "road subbldg1",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "road ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "road PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                },
            };
            SetUpDataForDeliveryPointsRange();
            var result = await testCandidate.CheckForDuplicateNybRecordsForRange(postalAddress, new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Item1);
            Assert.IsTrue(result.Item2.Count == 1);
        }

        [Test]
        public async Task TestCheckForDuplicateNybRecordsForRange_WithoutDuplicates()
        {
            var postalAddress = new List<PostalAddressDataDTO>
            {
                new PostalAddressDataDTO()
                {
                    BuildingName = "road bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "road subbldg1",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "road ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "road PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                },
            };
            SetUpDataForDeliveryPointsRange();
            var result = await testCandidate.CheckForDuplicateNybRecordsForRange(postalAddress, new Guid("0FCE0E9B-B2EB-4732-916F-7BC30BB85770"));
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Item1);
            Assert.IsTrue(result.Item2.Count == 0);
        }

        [Test]
        public async Task TestCheckForDuplicateAddressWithDeliveryPointsForRange_WithDuplicates()
        {
            var postalAddress = new List<PostalAddressDataDTO>
            {
                new PostalAddressDataDTO()
                {
                    BuildingName = "road bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "road subbldg1",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "road ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "road PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                },
            };
            SetUpDataForDeliveryPointsRange();
            var result = await testCandidate.CheckForDuplicateAddressWithDeliveryPointsForRange(postalAddress);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Item1);
            Assert.IsTrue(result.Item2.Count == 1);
        }

        [Test]
        public async Task TestCheckForDuplicateAddressWithDeliveryPointsForRange_WithoutDuplicates()
        {
            var postalAddress = new List<PostalAddressDataDTO>
            {
                new PostalAddressDataDTO()
                {
                    BuildingName = "road bldg2",
                    BuildingNumber = 10,
                    SubBuildingName = "road subbldg1123",
                    OrganisationName = "organisation",
                    DepartmentName = "department23",
                    Thoroughfare = "road ThoroughFare1456",
                    DependentThoroughfare = "DependentThoroughFare1897",
                    Postcode = "road PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 56789,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                },
            };
            SetUpDataForDeliveryPointsRange();
            var result = await testCandidate.CheckForDuplicateAddressWithDeliveryPointsForRange(postalAddress);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Item1);
            Assert.IsTrue(result.Item2.Count == 0);
        }

        [Test]
        public async Task Test_GetAllPendingDeletePostalAddresses_PositiveScenario()
        {
            SetUpDataForPostalAddressDelete();
            Guid pendingDelete = new Guid("254A92E1-4D6C-411B-A5A5-E0495A4F2525");
            var result = await testCandidate.GetAllPendingDeletePostalAddresses(pendingDelete);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public async Task Test_GetAllPendingDeletePostalAddresses_NegativeScenario()
        {
            SetUpDataForPostalAddressDelete();            
            var result = await testCandidate.GetAllPendingDeletePostalAddresses(Guid.Empty);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public async Task Test_DeletePostalAddressForHousekeeping_PositiveScenario()
        {
            SetUpDataForPostalAddressDelete();

            List<PostalAddressDataDTO> postalAddresses = new List<PostalAddressDataDTO>
            {
                new PostalAddressDataDTO
                {
                    ID = new Guid("5C331F0C-5D6A-4524-AECE-47EBF4BFD2F3"),
                    DeliveryPoints = new List<DeliveryPointDataDTO>
                    {
                        new DeliveryPointDataDTO {
                            ID = new Guid("B2E5DB38-7E57-48E2-A2F6-48A6277457AA"),
                            PostalAddressID = new Guid("5C331F0C-5D6A-4524-AECE-47EBF4BFD2F3")
                        }
                    },
                    PostalAddressStatus = new List<PostalAddressStatusDataDTO>
                    {
                        new PostalAddressStatusDataDTO {
                            PostalAddressGUID = new Guid("5C331F0C-5D6A-4524-AECE-47EBF4BFD2F3"),
                            OperationalStatusGUID = new Guid("254A92E1-4D6C-411B-A5A5-E0495A4F2525")
                        }
                    }

                }
            };
            Guid pendingDelete = new Guid("254A92E1-4D6C-411B-A5A5-E0495A4F2525");
            var result = await testCandidate.DeletePostalAddressForHousekeeping(postalAddresses);
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_DeletePostalAddressForHousekeeping_NegativeScenario()
        {
            List<PostalAddressDataDTO> postalAddresses = new List<PostalAddressDataDTO>();
            SetUpDataForPostalAddressDelete();
            var result = await testCandidate.DeletePostalAddressForHousekeeping(postalAddresses);
            Assert.IsNotNull(result);
            Assert.IsTrue(!result);
        }

        protected override void OnSetup()
        {
            lstPostCodeDTO = new List<CommonLibrary.EntityFramework.DTO.PostCodeDTO>()
            {
                new CommonLibrary.EntityFramework.DTO.PostCodeDTO()
                {
                    ID = id1
                },
                 new CommonLibrary.EntityFramework.DTO.PostCodeDTO()
                {
                    ID = id2
                }
            };
        }

        private void SetUpDataForDeliveryPoints()
        {
            postalAddressesDataDTO = new List<PostalAddressDataDTO>()
            {
                new PostalAddressDataDTO()
                {
                    BuildingName = "Anglo Office Park",
                    BuildingNumber = 1,
                    SubBuildingName = "Unit 2",
                    OrganisationName = null,
                    DepartmentName = "department",
                    Thoroughfare = "Lincoln Road",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "HP12 3FU",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "S",
                    SmallUserOrganisationIndicator = null,
                    DeliveryPointSuffix = "1B",

                    // PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("222C68A4-D959-4B37-B468-4B1855950A81"),
                    ID = new Guid("5275F14A-766D-4DCD-B475-001707F5D905")
            },
                new PostalAddressDataDTO()
                {
                    BuildingName = "bldg1",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "Postcode",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",

                    // PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("222C68A4-D959-4B37-B468-4B1855950A81"),
                    ID = new Guid("5275F14A-766D-4DCD-B475-001707F5D905")
            }
            };

            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress> postalAddresses = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
                {
                    BuildingName = "bldg1",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    AddressType_GUID = new Guid("222C68A4-D959-4B37-B468-4B1855950A81"),
                    Postcode = "Postcode",
                    ID = new Guid("5275F14A-766D-4DCD-B475-001707F5D905"),
            }
            };

            PostalAddressDTO postalAddressDTO = new PostalAddressDTO()
            {
                ID = Guid.NewGuid()
            };

            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO()
            {
                ID = Guid.NewGuid()
            };

            var mockPostalAddressDBSet = MockDbSet(postalAddresses);

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            // mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockAddressDataService = CreateMock<IPostalAddressDataService>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);

            mockAddressDataService.Setup(n => n.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDataDTO>(), It.IsAny<Guid>())).Returns("Postcode");
            mockAddressDataService.Setup(n => n.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDataDTO>())).Returns(Task.FromResult(true));
            mockAddressDataService.Setup(n => n.GetPAFAddress(It.IsAny<int>(), It.IsAny<Guid>())).Returns(Task.FromResult(postalAddressDTO));

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        private void SetUpDataForPostalAddressDelete()
        {
            Mock<List<PostalAddressStatus>> mockPostalAddressStatus = CreateMock<List<PostalAddressStatus>>();
            Mock<List<DeliveryPoint>> mockDeliveryPoint = CreateMock<List<DeliveryPoint>>();

            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus> postalAddressStatuses = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>
                    {
                        new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus {
                            PostalAddressGUID = new Guid("5C331F0C-5D6A-4524-AECE-47EBF4BFD2F3"),
                            OperationalStatusGUID = new Guid("254A92E1-4D6C-411B-A5A5-E0495A4F2525")
                        }
                    };

            List<RM.DataManagement.PostalAddress.WebAPI.Entities.DeliveryPoint> deliveryPoints = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.DeliveryPoint>
                    {
                        new RM.DataManagement.PostalAddress.WebAPI.Entities.DeliveryPoint {
                            ID = new Guid("B2E5DB38-7E57-48E2-A2F6-48A6277457AA"),
                            PostalAddressID = new Guid("5C331F0C-5D6A-4524-AECE-47EBF4BFD2F3")
                        }
                    };

            List<PostalAddressAlias> postalAddressAliases = new List<PostalAddressAlias>();

            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress> postalAddresses = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress
                {
                    ID = new Guid("5C331F0C-5D6A-4524-AECE-47EBF4BFD2F3"),
                    DeliveryPoints = deliveryPoints,
                    PostalAddressStatus = postalAddressStatuses,
                    PostalAddressAlias = postalAddressAliases
                },

                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress
                {
                    ID = new Guid("5C331F0C-5D6A-4524-AECE-47EBF4BFD2F4"),
                    PostalAddressStatus = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>
                    {
                        new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus {
                            PostalAddressGUID = new Guid("5C331F0C-5D6A-4524-AECE-47EBF4BFD2F4"),
                            OperationalStatusGUID = new Guid("254A92E1-4D6C-411B-A5A5-E0495A4F2525")
                        }
                    },
                    PostalAddressAlias = postalAddressAliases

                }
            };

            var mockPostalAddressDBSet = MockDbSet(postalAddresses);
            var mockPostalAddressStatusDBSet = MockDbSet(postalAddressStatuses);
            var mockDeliveryPointDBSet = MockDbSet(deliveryPoints);
            var mockPostalAddressAliasDBSet = MockDbSet(postalAddressAliases);

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockAddressDataService = CreateMock<IPostalAddressDataService>();
            var mockPostalAddressEnumerable = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(postalAddresses);
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>()).Returns(mockPostalAddressStatusDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressAlias>()).Returns(mockPostalAddressAliasDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressStatusDBSet.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>>())).Returns(mockPostalAddressStatusDBSet.Object);
            mockPostalAddressAliasDBSet.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressAlias>>())).Returns(mockPostalAddressAliasDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddressStatus).Returns(mockPostalAddressStatusDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddressAlias).Returns(mockPostalAddressAliasDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockAddressDataService.Setup(n => n.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDataDTO>(), It.IsAny<Guid>())).Returns("Postcode");
            mockAddressDataService.Setup(n => n.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDataDTO>())).Returns(Task.FromResult(true));

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
        }

        private void SetUpDataForCreateAddressAndDeliveryPoint()
        {
            postalAddressesDataDTO = new List<PostalAddressDataDTO>()
            {
                new PostalAddressDataDTO()
                {
                    BuildingName = "bldg1",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",

                    // PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("222C68A4-D959-4B37-B468-4B1855950A81"),
                    ID = new Guid("5275F14A-766D-4DCD-B475-001707F5D905")
            },
                new PostalAddressDataDTO()
                {
                    BuildingName = "bldg1",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "Postcode",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",

                    // PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11")
            }
            };

            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress> postalAddresses = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
                {
                    BuildingName = "bldg1",
                    BuildingNumber = 1,
                    SubBuildingName = "subbldg",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    Postcode = "Postcode",
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    UDPRN = 12345
            }
            };

            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO()
            {
                ID = Guid.NewGuid()
            };

            var mockPostalAddressDBSet = MockDbSet(postalAddresses);

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            // mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockAddressDataService = CreateMock<IPostalAddressDataService>();

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);

            var mockPostalAddressDTO = CreateMock<PostalAddressDataDTO>();
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            mockAddressDataService.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddressesDataDTO[0]);
            mockAddressDataService.Setup(x => x.CreateAddressForDeliveryPoint(mockPostalAddressDTO.Object)).Returns(It.IsAny<Guid>());

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        private void SetUpdataWithDeliverypoints()
        {
            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress> lstPostalAddress = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
                {
                    ID = new Guid("5275F14A-766D-4DCD-B475-001707F5D905"),
                    AddressType_GUID = new Guid("222C68A4-D959-4B37-B468-4B1855950A81"),
                    UDPRN = 14856,
                }
            };

            PostalAddressDataDTO postalAddress = new PostalAddressDataDTO()
            {
                UDPRN = 14856,
                BuildingName = "Building one",
                BuildingNumber = 123,
                AddressType_GUID = new Guid("222C68A4-D959-4B37-B468-4B1855950A81")
            };

            dtoPostalAddresses = new PostalAddressDataDTO()
            {
                BuildingName = "bldg1",
                BuildingNumber = 1,
                SubBuildingName = "subbldg",
                OrganisationName = "org",
                DepartmentName = "department",
                Thoroughfare = "ThoroughFare1",
                DependentThoroughfare = "DependentThoroughFare1",
                Postcode = "PostcodeNew",
                PostTown = "PostTown",
                POBoxNumber = "POBoxNumber",
                UDPRN = 54471821,
                PostcodeType = "xyz",
                SmallUserOrganisationIndicator = "indicator",
                DeliveryPointSuffix = "DeliveryPointSuffix",

                AddressType_GUID = new Guid("222C68A4-D959-4B37-B468-4B1855950A81"),
                ID = new Guid("5275F14A-766D-4DCD-B475-001707F5D905"),
                PostalAddressStatus = new List<PostalAddressStatusDataDTO>()
                    {
                        new PostalAddressStatusDataDTO()
                        {
                         ID = Guid.NewGuid(),
                         OperationalStatusGUID = new Guid("EE479380-C4F7-4FA3-96B2-DD54A7091BAA"),
                         StartDateTime = DateTime.UtcNow,
                         RowCreateDateTime = DateTime.UtcNow
                         }
                    }
            };

            var mockPostalAddressEnumerable = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(lstPostalAddress);
            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include("DeliveryPoint"));

            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(c => c.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(n => n.SaveChangesAsync()).Returns(() => Task.Run(() => { return 1; })).Verifiable();

            // mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();

            mockAddressDataService = CreateMock<IPostalAddressDataService>();

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        private void SetUpdataWithOutDeliverypoints()
        {
            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress> lstPostalAddress = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
                {
                    ID = Guid.NewGuid(),
                    UDPRN = 14856,
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
                }
            };

            PostalAddressDTO postalAddress = new PostalAddressDTO()
            {
                ID = Guid.NewGuid(),
                UDPRN = 14856,
                BuildingName = "Building one",
                BuildingNumber = 123,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
            };

            var mockPostalAddressEnumerable = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(lstPostalAddress);
            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            // mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        private void SetUpdataPostalAddressDetails()
        {
            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress> lstPostalAddress = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    UDPRN = 14856,
                }
            };

            PostalAddressDataDTO postalAddress = new PostalAddressDataDTO()
            {
                UDPRN = 14856,
                BuildingName = "Building one",
                BuildingNumber = 123,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
            };

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockAddressDataService = CreateMock<IPostalAddressDataService>();

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);

            mockAddressDataService.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddress);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        private void OnSetupPaf()
        {
            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress> lstPostalAddress = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()
                        {
                            new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress
                            {
                                BuildingName = "Bldg 1",
                                BuildingNumber = 23,
                                Postcode = "123",
                                PostalAddressStatus = new List<PostalAddressStatus>() { new PostalAddressStatus() { } },
                                UDPRN = 14856
                        }
                    };

            List<PostalAddressStatus> postalAddressStatusList = new List<PostalAddressStatus>()
            {
                new PostalAddressStatus()
                {
                    PostalAddressGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
                }
            };

            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            var mockPostalAddressEnumerable = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(lstPostalAddress);
            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockPostalAddressDBSet.Setup(x => x.Include("PostalAddressStatus"));
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);

            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            var mockPostalAddressEnumerable1 = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>(postalAddressStatusList);
            var mockPostalAddressStatusDBSet = MockDbSet(postalAddressStatusList);
            mockPostalAddressStatusDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressStatusDBSet.Object);

            mockPostalAddressStatusDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable1.AsQueryable().Provider);
            mockPostalAddressStatusDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable1.AsQueryable().Expression);
            mockPostalAddressStatusDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable1.AsQueryable().ElementType);
            mockPostalAddressStatusDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>)mockPostalAddressEnumerable1).GetAsyncEnumerator());

            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>()).Returns(mockPostalAddressStatusDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddressStatus).Returns(mockPostalAddressStatusDBSet.Object);

            mockLoggingHelper = CreateMock<ILoggingHelper>();

            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        private void SetUpDataForDeliveryPointsRange()

        {
            var deliveryPoint = new List<DeliveryPoint>()

            {
               new DeliveryPoint()
               {
                   ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A13"),
                   PostalAddressID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"),

                   DeliveryPointUseIndicatorGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A14"),
                    RowVersion = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
               }
            };

            var postalAddress = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
                {
                    BuildingName = "road bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "road subbldg1",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "road ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "road PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"),
                    DeliveryPoints=deliveryPoint,
        }
            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();

            // setup for PostalAdress
            var mockAsynPostalAdress = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(postalAddress);
            var mocknpostalAddresse = MockDbSet(postalAddress);
            mocknpostalAddresse.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynPostalAdress.AsQueryable().Provider);
            mocknpostalAddresse.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynPostalAdress.AsQueryable().Expression);
            mocknpostalAddresse.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynPostalAdress.AsQueryable().ElementType);
            mocknpostalAddresse.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockAsynPostalAdress).GetAsyncEnumerator());
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mocknpostalAddresse.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mocknpostalAddresse.Object);
            mocknpostalAddresse.Setup(x => x.Include(It.IsAny<string>())).Returns(mocknpostalAddresse.Object);
            mocknpostalAddresse.Setup(x => x.AsNoTracking()).Returns(mocknpostalAddresse.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            // mockConfigurationHelper = new Mock<IConfigurationHelper>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }

        private void SetUpdataForDeletePostalAddress()
        {
            var postalAddress = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
                {
                    BuildingName = "road bldg2",
                    BuildingNumber = 1,
                    SubBuildingName = "road subbldg1",
                    OrganisationName = "org",
                    DepartmentName = "department",
                    Thoroughfare = "road ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    Postcode = "road PostcodeNew",
                    PostTown = "PostTown",
                    POBoxNumber = "POBoxNumber",
                    UDPRN = 12345,
                    PostcodeType = "xyz",
                    SmallUserOrganisationIndicator = "indicator",
                    DeliveryPointSuffix = "DeliveryPointSuffix",
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A19"),
                    ID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA"),
                    PostalAddressAlias = new List<PostalAddressAlias>()
                {
                    new PostalAddressAlias()
                    {
                        PostalAddressID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                    }
                },
                    PostalAddressStatus = new List<PostalAddressStatus>()
                {
                    new PostalAddressStatus()
                    {
                        PostalAddressGUID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                    }
                }
                },
            };

            var postalAddressAlias = new List<PostalAddressAlias>()
                {
                    new PostalAddressAlias()
                    {
                        PostalAddressID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                    }
                };
            var postalAddressStatus = new List<PostalAddressStatus>()
                {
                    new PostalAddressStatus()
                    {
                        PostalAddressGUID = new Guid("619AF1F3-AE0C-4157-9BDE-A7528C1482BA")
                    }
                };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();

            // setup for PostalAdress
            var mockAsynPostalAdress = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(postalAddress);
            var mocknpostalAddress = MockDbSet(postalAddress);
            mocknpostalAddress.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynPostalAdress.AsQueryable().Provider);
            mocknpostalAddress.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynPostalAdress.AsQueryable().Expression);
            mocknpostalAddress.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynPostalAdress.AsQueryable().ElementType);
            mocknpostalAddress.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockAsynPostalAdress).GetAsyncEnumerator());
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mocknpostalAddress.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mocknpostalAddress.Object);
            mocknpostalAddress.Setup(x => x.Include(It.IsAny<string>())).Returns(mocknpostalAddress.Object);
            mocknpostalAddress.Setup(x => x.AsNoTracking()).Returns(mocknpostalAddress.Object);

            // setup for PostalAdress Aliases
            var mockAsynPostalAdressAlias = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressAlias>(postalAddressAlias);
            var mocknpostalAddressAlias = MockDbSet(postalAddressAlias);
            mocknpostalAddressAlias.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynPostalAdressAlias.AsQueryable().Provider);
            mocknpostalAddressAlias.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynPostalAdressAlias.AsQueryable().Expression);
            mocknpostalAddressAlias.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynPostalAdressAlias.AsQueryable().ElementType);
            mocknpostalAddressAlias.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressAlias>)mockAsynPostalAdressAlias).GetAsyncEnumerator());
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressAlias>()).Returns(mocknpostalAddressAlias.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddressAlias).Returns(mocknpostalAddressAlias.Object);
            mocknpostalAddressAlias.Setup(x => x.Include(It.IsAny<string>())).Returns(mocknpostalAddressAlias.Object);
            mocknpostalAddressAlias.Setup(x => x.AsNoTracking()).Returns(mocknpostalAddressAlias.Object);

            // setup for PostalAdress Aliases
            var mockAsynPostalAdressStatus = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>(postalAddressStatus);
            var mocknpostalAddressStatus = MockDbSet(postalAddressStatus);
            mocknpostalAddressStatus.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynPostalAdressStatus.AsQueryable().Provider);
            mocknpostalAddressStatus.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynPostalAdressStatus.AsQueryable().Expression);
            mocknpostalAddressStatus.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynPostalAdressStatus.AsQueryable().ElementType);
            mocknpostalAddressStatus.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>)mockAsynPostalAdressStatus).GetAsyncEnumerator());
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddressStatus>()).Returns(mocknpostalAddressStatus.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddressStatus).Returns(mocknpostalAddressStatus.Object);
            mocknpostalAddressStatus.Setup(x => x.Include(It.IsAny<string>())).Returns(mocknpostalAddressStatus.Object);
            mocknpostalAddressStatus.Setup(x => x.AsNoTracking()).Returns(mocknpostalAddressStatus.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            // mockConfigurationHelper = new Mock<IConfigurationHelper>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(n => n.SaveChangesAsync()).ReturnsAsync(1);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}