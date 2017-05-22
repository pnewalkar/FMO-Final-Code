using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Fmo.Common.AsyncEnumerator;
using Fmo.Common.Interface;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Moq;
using NUnit.Framework;

namespace Fmo.DataServices.Tests.Repositories
{
    [TestFixture]
    public class AddressRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IFileProcessingLogRepository> mockFileProcessingLog;
        private Mock<IPostCodeRepository> mockPostCodeRepository;
        private Mock<IReferenceDataCategoryRepository> mockReferenceDataCategoryRepository;
        private Mock<IAddressRepository> mockAddressRepository;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IAddressRepository testCandidate;
        private List<PostalAddressDTO> postalAddressesDTO;
        private AddDeliveryPointDTO addDeliveryPointDTO1;
        private AddDeliveryPointDTO addDeliveryPointDTO2;
        private AddDeliveryPointDTO addDeliveryPointDTO3;

        [Test]
        public void Test_UpdateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = new PostalAddressDTO() { UDPRN = 14856 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_CreateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = new PostalAddressDTO() { UDPRN = 15862 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_InValidAddress()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = null;
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Never);
            Assert.NotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_DeleteAddressWithDeliveryPoint()
        {
            SetUpdataWithDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 158623, 85963 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_DeleteAddressWithoutDeliveryPoint()
        {
            SetUpdataWithOutDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 158623, 85963 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_DeleteAddressWithoutMatchingUDPRN()
        {
            SetUpdataWithOutDeliverypoints();
            List<int> lstUDPRNS = new List<int>() { 14856 };
            var result = testCandidate.DeleteNYBPostalAddress(lstUDPRNS, new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Never);
            Assert.NotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_GetPostalAddressDetails()
        {
            SetUpdataPostalAddressDetails();
            var result = testCandidate.GetPostalAddressDetails(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            Assert.NotNull(result);
        }

        [Test]
        public async Task Test_SearchByPostcode()
        {
            SetupDataForSearch();
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare1,Postcode1");
        }

        [Test]
        public async Task Test_SearchByThoroughFare()
        {
            SetupDataForSearch();
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("ThoroughFare2", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare2,Postcode2");
        }

        [Test]
        public async Task Test_SearchByDependentThoroughFare()
        {
            SetupDataForSearch();
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare1,Postcode1");
        }

        [Test]
        public async Task Test_SearchByPostcode1()
        {
            SetupDataForSearch();
            List<PostalAddressDTO> results = await testCandidate.GetPostalAddressDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].Postcode == "Postcode1");
            Assert.IsTrue(results[0].RouteDetails[0].DisplayText == "Secondary - DeliveryRoute1");
        }

        [Test]
        public async Task Test_SearchByPostcode2()
        {
            SetupDataForSearch();
            List<PostalAddressDTO> results = await testCandidate.GetPostalAddressDetails("Postcode2", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].Postcode == "Postcode2");
            Assert.IsTrue(results[0].RouteDetails[0].DisplayText == "Primary - DeliveryRoute2");
        }

        [Test]
        public void Test_CheckForDuplicateNybRecords_Duplicate()
        {
            SetUpDataForDeliveryPoints();
            string results = testCandidate.CheckForDuplicateNybRecords(postalAddressesDTO[0]);
            Assert.NotNull(results);
            Assert.IsTrue(results == "Postcode");
        }

        [Test]
        public void Test_CheckForDuplicateNybRecords_NotDuplicate()
        {
            SetUpDataForDeliveryPoints();
            string results = testCandidate.CheckForDuplicateNybRecords(postalAddressesDTO[1]);
            Assert.NotNull(results);
            Assert.IsTrue(results == string.Empty);
        }

        [Test]
        public void Test_CheckForDuplicateAddressWithDeliveryPoints_Duplicate()
        {
            SetUpDataForDeliveryPoints();
            bool results = testCandidate.CheckForDuplicateAddressWithDeliveryPoints(postalAddressesDTO[1]);
            Assert.NotNull(results);
            Assert.IsTrue(results == true);
        }

        [Test]
        public void Test_CheckForDuplicateAddressWithDeliveryPoints_NotDuplicate()
        {
            SetUpDataForDeliveryPoints();
            bool results = testCandidate.CheckForDuplicateAddressWithDeliveryPoints(postalAddressesDTO[0]);
            Assert.NotNull(results);
            Assert.IsTrue(results == false);
        }

        [Test]
        public void Test_CreateAddressAndDeliveryPoint_SamePostcode()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var results = testCandidate.CreateAddressAndDeliveryPoint(addDeliveryPointDTO2);
            Assert.NotNull(results.ID);
            Assert.IsTrue(results.ID != Guid.Empty);
        }

        [Test]
        public void Test_CreateAddressAndDeliveryPoint_DifferentPostcode()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var results = testCandidate.CreateAddressAndDeliveryPoint(addDeliveryPointDTO1);
            Assert.NotNull(results);
            Assert.IsTrue(results.ID != Guid.Empty);
        }

        [Test]
        public void Test_CreateAddressAndDeliveryPoint_Null()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var results = testCandidate.CreateAddressAndDeliveryPoint(addDeliveryPointDTO3);
            Assert.NotNull(results);
            Assert.IsTrue(results.ID == Guid.Empty);
        }

        protected override void OnSetup()
        {
        }

        private void SetUpDataForDeliveryPoints()
        {
            postalAddressesDTO = new List<PostalAddressDTO>()
            {
                new PostalAddressDTO()
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
                    PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11")
            },
                new PostalAddressDTO()
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
                    PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11")
            }
            };

            List<DeliveryPoint> deliveryPoint = new List<DeliveryPoint>()
            {
                new DeliveryPoint()
                {
                }
            };

            List<PostalAddress> postalAddresses = new List<PostalAddress>()
            {
                new PostalAddress()
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
                    DeliveryPoints = deliveryPoint
            }
            };

            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO()
            {
                ID = Guid.NewGuid()
            };

            addDeliveryPointDTO1 = new AddDeliveryPointDTO()
            {
                PostalAddressDTO = postalAddressesDTO[0],
                DeliveryPointDTO = deliveryPointDTO
            };

            var mockPostalAddressDBSet = MockDbSet(postalAddresses);
            var mockDeliveryPointsDBSet = MockDbSet(deliveryPoint);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogRepository>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockPostCodeRepository = CreateMock<IPostCodeRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockAddressRepository = CreateMock<IAddressRepository>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);

            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointsDBSet.Object);
            mockDeliveryPointsDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointsDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointsDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointsDBSet.Object);

            mockPostCodeRepository.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Guid.NewGuid);
            mockAddressRepository.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddressesDTO[0]);

            mockReferenceDataCategoryRepository.Setup(x => x.GetReferenceDataId("Postal Address Type", "Nyb")).Returns(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));

            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }

        private void SetUpDataForCreateAddressAndDeliveryPoint()
        {
            postalAddressesDTO = new List<PostalAddressDTO>()
            {
                new PostalAddressDTO()
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
                    PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A12")
            },
                new PostalAddressDTO()
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
                    PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"),
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11")
            }
            };

            List<DeliveryPoint> deliveryPoint = new List<DeliveryPoint>()
            {
                new DeliveryPoint()
                {
                }
            };

            List<PostalAddress> postalAddresses = new List<PostalAddress>()
            {
                new PostalAddress()
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
                    DeliveryPoints = deliveryPoint
            }
            };

            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO()
            {
                ID = Guid.NewGuid()
            };

            addDeliveryPointDTO1 = new AddDeliveryPointDTO()
            {
                PostalAddressDTO = postalAddressesDTO[0],
                DeliveryPointDTO = deliveryPointDTO
            };
            addDeliveryPointDTO2 = new AddDeliveryPointDTO()
            {
                PostalAddressDTO = postalAddressesDTO[1],
                DeliveryPointDTO = deliveryPointDTO
            };
            addDeliveryPointDTO3 = new AddDeliveryPointDTO()
            {
                PostalAddressDTO = null,
                DeliveryPointDTO = null
            };

            List<AddressLocation> addressLocation = new List<AddressLocation>()
            {
                new AddressLocation()
                {
                    UDPRN = 12345
                }
            };

            var mockPostalAddressDBSet = MockDbSet(postalAddresses);
            var mockDeliveryPointsDBSet = MockDbSet(deliveryPoint);
            var mockAddressLocationsDBSet = MockDbSet(addressLocation);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogRepository>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockPostCodeRepository = CreateMock<IPostCodeRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockAddressRepository = CreateMock<IAddressRepository>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointsDBSet.Object);
            mockDeliveryPointsDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointsDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointsDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointsDBSet.Object);

            mockFmoDbContext.Setup(x => x.Set<AddressLocation>()).Returns(mockAddressLocationsDBSet.Object);
            mockDeliveryPointsDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockDeliveryPointsDBSet.Object);
            mockFmoDbContext.Setup(x => x.AddressLocations).Returns(mockAddressLocationsDBSet.Object);

            mockPostCodeRepository.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Guid.NewGuid);
            mockAddressRepository.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddressesDTO[0]);

            mockReferenceDataCategoryRepository.Setup(x => x.GetReferenceDataId("Postal Address Type", "Nyb")).Returns(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));

            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }

        private void SetUpdataWithDeliverypoints()
        {
            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    UDPRN = 14856,
                    DeliveryPoints = new List<DeliveryPoint>()
               {
                new DeliveryPoint() { ID = Guid.NewGuid(), Address_GUID = Guid.NewGuid(), UDPRN = 14856 },
                new DeliveryPoint() { ID = Guid.NewGuid(), Address_GUID = Guid.NewGuid(), UDPRN = 14856 }
            }
                }
            };

            PostalAddressDTO postalAddress = new PostalAddressDTO()
            {
                UDPRN = 14856,
                BuildingName = "Building one",
                BuildingNumber = 123,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
            };

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogRepository>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockPostCodeRepository = CreateMock<IPostCodeRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockAddressRepository = CreateMock<IAddressRepository>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            mockPostCodeRepository.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Guid.NewGuid);
            mockAddressRepository.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddress);

            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }

        private void SetUpdataWithOutDeliverypoints()
        {
            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
            {
                new PostalAddress()
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

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogRepository>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockPostCodeRepository = CreateMock<IPostCodeRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostCodeRepository.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Guid.NewGuid);
            mockAddressRepository.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddress);

            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }

        private void SetUpdataPostalAddressDetails()
        {
            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    ID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                    UDPRN = 14856,
                    DeliveryPoints = new List<DeliveryPoint>()
               {
                new DeliveryPoint() { ID = Guid.NewGuid(), Address_GUID = Guid.NewGuid(), UDPRN = 14856 },
                new DeliveryPoint() { ID = Guid.NewGuid(), Address_GUID = Guid.NewGuid(), UDPRN = 14856 }
            }
                }
            };

            PostalAddressDTO postalAddress = new PostalAddressDTO()
            {
                UDPRN = 14856,
                BuildingName = "Building one",
                BuildingNumber = 123,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
            };

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogRepository>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockPostCodeRepository = CreateMock<IPostCodeRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockAddressRepository = CreateMock<IAddressRepository>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);

            mockPostCodeRepository.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Guid.NewGuid);
            mockAddressRepository.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddress);

            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }

        private void SetupDataForSearch()
        {
            var deliveryRoutePostcode1 = new DeliveryRoutePostcode();
            var deliveryRoutePostcode2 = new DeliveryRoutePostcode();

            var deliveryRoute1 = new DeliveryRoute()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000001"),
                OperationalStatus_GUID = new Guid("00000000-0000-0000-0000-000000000001"),
                RouteMethodType_GUID = new Guid("00000000-0000-0000-0000-000000000001"),
                RouteName = "DeliveryRoute1"
            };

            var deliveryRoute2 = new DeliveryRoute()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000002"),
                OperationalStatus_GUID = new Guid("00000000-0000-0000-0000-000000000002"),
                RouteMethodType_GUID = new Guid("00000000-0000-0000-0000-000000000002"),
                RouteName = "DeliveryRoute2"
            };

            var unitLocationPostcode1 = new UnitLocationPostcode()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000000"),
                PostcodeUnit = "test",
                Unit_GUID = new Guid("00000000-0000-0000-0000-000000000000"),
                PoscodeUnit_GUID = new Guid("00000000-0000-0000-0000-000000000002")
            };

            var unitLocationPostcode2 = new UnitLocationPostcode()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000001"),
                PostcodeUnit = "test",
                Unit_GUID = new Guid("00000000-0000-0000-0000-000000000000"),
                PoscodeUnit_GUID = new Guid("00000000-0000-0000-0000-000000000001")
            };

            var postCode1 = new Postcode()
            {
                PostcodeUnit = "Postcode1",
                OutwardCode = "test",
                InwardCode = "tes",
                Sector = "test",
                ID = new Guid("00000000-0000-0000-0000-000000000002"),
                SectorGUID = new Guid("00000000-0000-0000-0000-000000000000"),
                UnitLocationPostcodes = new List<UnitLocationPostcode>()
                {
                   unitLocationPostcode1
                },
                DeliveryRoutePostcodes = new List<DeliveryRoutePostcode>()
                {
                    deliveryRoutePostcode1
                }
            };

            var postCode2 = new Postcode()
            {
                PostcodeUnit = "Postcode2",
                OutwardCode = "test",
                InwardCode = "tes",
                Sector = "test",
                ID = new Guid("00000000-0000-0000-0000-000000000001"),
                SectorGUID = new Guid("00000000-0000-0000-0000-000000000001"),
                UnitLocationPostcodes = new List<UnitLocationPostcode>()
                {
                    unitLocationPostcode2
                },
                DeliveryRoutePostcodes = new List<DeliveryRoutePostcode>()
                {
                    deliveryRoutePostcode2
                }
            };

            deliveryRoutePostcode1.ID = new Guid("00000000-0000-0000-0000-000000000001");
            deliveryRoutePostcode1.DeliveryRoute_GUID = new Guid("00000000-0000-0000-0000-000000000001");
            deliveryRoutePostcode1.Postcode_GUID = new Guid("00000000-0000-0000-0000-000000000002");
            deliveryRoutePostcode1.IsPrimaryRoute = false;
            deliveryRoutePostcode1.Postcode = postCode1;
            deliveryRoutePostcode1.DeliveryRoute = deliveryRoute1;

            deliveryRoutePostcode2.ID = new Guid("00000000-0000-0000-0000-000000000002");
            deliveryRoutePostcode2.DeliveryRoute_GUID = new Guid("00000000-0000-0000-0000-000000000002");
            deliveryRoutePostcode2.Postcode_GUID = new Guid("00000000-0000-0000-0000-000000000001");
            deliveryRoutePostcode2.IsPrimaryRoute = true;
            deliveryRoutePostcode2.Postcode = postCode2;
            deliveryRoutePostcode2.DeliveryRoute = deliveryRoute2;

            List<Postcode> postcodes = new List<Postcode>(new Postcode[] { postCode1, postCode2 });

            List<PostalAddress> postalAddresses = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    Postcode = "Postcode1",
                    Thoroughfare = "ThoroughFare1",
                    DependentThoroughfare = "DependentThoroughFare1",
                    AddressType_GUID = new Guid("222C68A4-D959-4B37-B468-4B1855950A81"),
                    PostCodeGUID = new Guid("00000000-0000-0000-0000-000000000002"),
                    UDPRN = 14856,
                    PostcodeType = "S",
                    PostTown = "test",
                    ID = new Guid("00000000-0000-0000-0000-000000000000"),
                    AddressStatus_GUID = new Guid("00000000-0000-0000-0000-000000000000"),
                    Postcode1 = postCode1
                },
                new PostalAddress()
                {
                    Postcode = "Postcode2",
                    Thoroughfare = "ThoroughFare2",
                    DependentThoroughfare = "DependentThoroughFare2",
                    AddressType_GUID = new Guid("A21F3E46-2D0D-4989-A5D5-872D23B479A2"),
                    PostCodeGUID = new Guid("00000000-0000-0000-0000-000000000001"),
                    UDPRN = 14856,
                    PostcodeType = "S",
                    PostTown = "test",
                    ID = new Guid("00000000-0000-0000-0000-000000000001"),
                    AddressStatus_GUID = new Guid("00000000-0000-0000-0000-000000000001"),
                    Postcode1 = postCode2
                }
            };

            List<DeliveryRoutePostcode> deliveryRoutePostcodes = new List<DeliveryRoutePostcode>()
            {
               deliveryRoutePostcode1,
               deliveryRoutePostcode2
            };

            List<DeliveryRoute> deliveryRoutes = new List<DeliveryRoute>() { deliveryRoute1, deliveryRoute2 };

            List<UnitLocationPostcode> unitLocationPostcodes = new List<UnitLocationPostcode>(new UnitLocationPostcode[] { unitLocationPostcode1, unitLocationPostcode2 });

            var mockPostalAddressEnumerable = new DbAsyncEnumerable<PostalAddress>(postalAddresses);
            var mockPostalAddress = MockDbSet(postalAddresses);

            var mockPostcodeEnumerable = new DbAsyncEnumerable<Postcode>(postcodes);
            var mockPostcode = MockDbSet(postcodes);

            var mockUnitLocationPostcodesEnumerable = new DbAsyncEnumerable<UnitLocationPostcode>(unitLocationPostcodes);
            var mockUnitLocationPostcodes = MockDbSet(unitLocationPostcodes);

            var mockDeliveryRoutePostcodeEnumerable = new DbAsyncEnumerable<DeliveryRoutePostcode>(deliveryRoutePostcodes);
            var mockDeliveryRoutePostcode = MockDbSet(deliveryRoutePostcodes);

            var mockDeliveryRouteEnumerable = new DbAsyncEnumerable<DeliveryRoute>(deliveryRoutes);
            var mockDeliveryRoute = MockDbSet(deliveryRoutes);

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogRepository>();
            mockPostCodeRepository = CreateMock<IPostCodeRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));

            mockDeliveryRoutePostcode.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockDeliveryRoutePostcodeEnumerable.AsQueryable().Provider);
            mockDeliveryRoutePostcode.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockDeliveryRoutePostcodeEnumerable.AsQueryable().Expression);
            mockDeliveryRoutePostcode.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockDeliveryRoutePostcodeEnumerable.AsQueryable().ElementType);
            mockDeliveryRoutePostcode.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryRoutePostcode>)mockDeliveryRoutePostcodeEnumerable).GetAsyncEnumerator());

            mockPostalAddress.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddress.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());
            mockPostalAddress.Setup(x => x.Include("DeliveryRoutePostcode"));

            mockPostcode.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostcodeEnumerable.AsQueryable().Provider);
            mockPostcode.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostcodeEnumerable.AsQueryable().Expression);
            mockPostcode.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostcodeEnumerable.AsQueryable().ElementType);
            mockPostcode.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Postcode>)mockPostcodeEnumerable).GetAsyncEnumerator());

            mockUnitLocationPostcodes.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockUnitLocationPostcodesEnumerable.AsQueryable().Provider);
            mockUnitLocationPostcodes.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockUnitLocationPostcodesEnumerable.AsQueryable().Expression);
            mockUnitLocationPostcodes.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockUnitLocationPostcodesEnumerable.AsQueryable().ElementType);
            mockUnitLocationPostcodes.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UnitLocationPostcode>)mockUnitLocationPostcodesEnumerable).GetAsyncEnumerator());

            mockDeliveryRoute.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockDeliveryRouteEnumerable.AsQueryable().Provider);
            mockDeliveryRoute.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockDeliveryRouteEnumerable.AsQueryable().Expression);
            mockDeliveryRoute.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockDeliveryRouteEnumerable.AsQueryable().ElementType);
            mockDeliveryRoute.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryRoute>)mockDeliveryRouteEnumerable).GetAsyncEnumerator());

            mockFmoDbContext = CreateMock<FMODBContext>();

            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddress.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddress.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddress.Object);

            mockFmoDbContext.Setup(x => x.Set<Postcode>()).Returns(mockPostcode.Object);
            mockFmoDbContext.Setup(x => x.Postcodes).Returns(mockPostcode.Object);
            mockFmoDbContext.Setup(x => x.Postcodes.AsNoTracking()).Returns(mockPostcode.Object);

            mockFmoDbContext.Setup(x => x.Set<UnitLocationPostcode>()).Returns(mockUnitLocationPostcodes.Object);
            mockFmoDbContext.Setup(x => x.UnitLocationPostcodes).Returns(mockUnitLocationPostcodes.Object);
            mockFmoDbContext.Setup(x => x.UnitLocationPostcodes.AsNoTracking()).Returns(mockUnitLocationPostcodes.Object);

            mockFmoDbContext.Setup(x => x.Set<DeliveryRoutePostcode>()).Returns(mockDeliveryRoutePostcode.Object);
            mockFmoDbContext.Setup(x => x.DeliveryRoutePostcodes).Returns(mockDeliveryRoutePostcode.Object);
            mockFmoDbContext.Setup(x => x.DeliveryRoutePostcodes.AsNoTracking()).Returns(mockDeliveryRoutePostcode.Object);

            mockFmoDbContext.Setup(x => x.Set<DeliveryRoute>()).Returns(mockDeliveryRoute.Object);
            mockFmoDbContext.Setup(x => x.DeliveryRoutes).Returns(mockDeliveryRoute.Object);
            mockFmoDbContext.Setup(x => x.DeliveryRoutes.AsNoTracking()).Returns(mockDeliveryRoute.Object);

            mockReferenceDataCategoryRepository.Setup(x => x.GetReferenceDataIds(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(new List<Guid>() { new Guid("222C68A4-D959-4B37-B468-4B1855950A81"), new Guid("A21F3E46-2D0D-4989-A5D5-872D23B479A2") });
            mockReferenceDataCategoryRepository.Setup(x => x.GetReferenceDataId("Postal Address Type", "Nyb")).Returns(new Guid("222C68A4-D959-4B37-B468-4B1855950A81"));

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }
    }
}