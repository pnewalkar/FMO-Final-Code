using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Implementation;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.Entities;

namespace RM.Data.PostalAddress.WebAPI.Test.DataService
{
    [TestFixture]
    public class AddressDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<PostalAddressDBContext> mockFmoDbContext;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IFileProcessingLogDataService> mockFileProcessingLog;
        private Mock<IDatabaseFactory<PostalAddressDBContext>> mockDatabaseFactory;
        private Mock<CommonLibrary.EntityFramework.DataService.Interfaces.IPostCodeDataService> mockPostCodeDataService;
        private Mock<CommonLibrary.EntityFramework.DataService.Interfaces.IReferenceDataCategoryDataService> mockReferenceDataCategoryDataService;
        private Mock<IPostalAddressDataService> mockAddressDataService;
        private IPostalAddressDataService testCandidate;
        private List<PostalAddressDBDTO> postalAddressesDTO;
        private AddDeliveryPointDTO addDeliveryPointDTO1;
        private AddDeliveryPointDTO addDeliveryPointDTO2;
        private AddDeliveryPointDTO addDeliveryPointDTO3;
        private PostalAddressDBDTO dtoPostalAddresses;
        private List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> lstPostCodeDTO;
        private PostalAddressDBDTO testObject;
        private Guid id1 = new Guid("00000000-0000-0000-0000-000000000002");
        private Guid id2 = new Guid("00000000-0000-0000-0000-000000000001");

        [Test]
        public void Test_UpdateAddressValidTestCase()
        {
            this.SetUpdataWithDeliverypoints();
            PostalAddressDBDTO objstPostalAddressDTO = new PostalAddressDBDTO() { UDPRN = 14856 };
            var result = this.testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChangesAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_CreateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDBDTO objstPostalAddressDTO = new PostalAddressDBDTO() { UDPRN = 15862 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChangesAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_InValidAddress()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDBDTO objstPostalAddressDTO = null;
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChangesAsync(), Times.Never);
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
            List<int> lstUDPRNS = new List<int>() { 158623, 85963 };
            var result = testCandidate.UpdateAddress(dtoPostalAddresses, "abc", new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));
            mockFmoDbContext.Verify(n => n.SaveChangesAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result.Result);
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
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"), new List<Guid>() { new Guid("222C68A4-D959-4B37-B468-4B1855950A81") }, lstPostCodeDTO);
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare1,Postcode1");
        }

        [Test]
        public async Task Test_SearchByThoroughFare()
        {
            SetupDataForSearch();
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("ThoroughFare2", new Guid("00000000-0000-0000-0000-000000000000"), new List<Guid>() { new Guid("A21F3E46-2D0D-4989-A5D5-872D23B479A2") }, lstPostCodeDTO);
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare2,Postcode2");
        }

        [Test]
        public async Task Test_SearchByDependentThoroughFare()
        {
            SetupDataForSearch();
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"), new List<Guid>() { new Guid("222C68A4-D959-4B37-B468-4B1855950A81") }, lstPostCodeDTO);
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare1,Postcode1");
        }

        [Test]
        public async Task Test_SearchByPostcode1()
        {
            SetupDataForSearch();
            var results = await testCandidate.GetPostalAddressDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"), lstPostCodeDTO);
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[0].Postcode == "Postcode1");
        }

        [Test]
        public async Task Test_SearchByPostcode2()
        {
            SetupDataForSearch();
            var results = await testCandidate.GetPostalAddressDetails("Postcode2", new Guid("00000000-0000-0000-0000-000000000000"), lstPostCodeDTO);
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 2);
            Assert.IsTrue(results[1].Postcode == "Postcode2");
        }

        [Test]
        public void Test_CheckForDuplicateNybRecords_Duplicate()
        {
            SetUpDataForDeliveryPoints();
            string results = testCandidate.CheckForDuplicateNybRecords(postalAddressesDTO[0], new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));
            Assert.NotNull(results);
            Assert.IsTrue(results == "Postcode");
        }

        [Test]
        public void Test_CheckForDuplicateNybRecords_NotDuplicate()
        {
            SetUpDataForDeliveryPoints();
            string results = testCandidate.CheckForDuplicateNybRecords(postalAddressesDTO[1], new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));
            Assert.NotNull(results);
            Assert.IsTrue(results == string.Empty);
        }


        [Test]
        public void Test_CreateAddressAndDeliveryPoint_SamePostcode()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var results = testCandidate.CreateAddressAndDeliveryPoint(addDeliveryPointDTO2, Guid.NewGuid());
            Assert.NotNull(results.ID);
            Assert.IsTrue(results.ID != Guid.Empty);
        }

        [Test]
        public void Test_CreateAddressAndDeliveryPoint_DifferentPostcode()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var results = testCandidate.CreateAddressAndDeliveryPoint(addDeliveryPointDTO1, Guid.NewGuid());
            Assert.NotNull(results);
            Assert.IsTrue(results.ID != Guid.Empty);
        }

        [Test]
        public void Test_CreateAddressAndDeliveryPoint_Null()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var results = testCandidate.CreateAddressAndDeliveryPoint(addDeliveryPointDTO3, Guid.NewGuid());
            Assert.NotNull(results);
            Assert.IsTrue(results.ID == Guid.Empty);
        }

        [Test]
        public void Test_InsertAddress()
        {
            OnSetupPaf();
            testObject = new PostalAddressDBDTO()
            {
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                UDPRN = 14856
            };
            var result = testCandidate.InsertAddress(testObject, "PAFTestFile.csv");
            mockFmoDbContext.Verify(n => n.SaveChangesAsync(), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);
        }

        [Test]
        public void Test_GetPostalAddressByUDPRN()
        {
            OnSetupPaf();
            testObject = new PostalAddressDBDTO()
            {
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
                UDPRN = 14856
            };
            var result = testCandidate.GetPostalAddress(testObject.UDPRN);
            Assert.IsNotNull(result);
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
            postalAddressesDTO = new List<PostalAddressDBDTO>()
            {
                new PostalAddressDBDTO()
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
                new PostalAddressDBDTO()
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

            //List<RM.DataManagement.PostalAddress.WebAPI.Entities.deli> deliveryPoint = new List<DeliveryPoint>()
            //{
            //    new DeliveryPoint()
            //    {
            //    }
            //};

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

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockAddressDataService = CreateMock<IPostalAddressDataService>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
        }

        private void SetUpDataForCreateAddressAndDeliveryPoint()
        {
            postalAddressesDTO = new List<PostalAddressDBDTO>()
            {
                new PostalAddressDBDTO()
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
                new PostalAddressDBDTO()
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

            //List<DeliveryPoint> deliveryPoint = new List<DeliveryPoint>()
            //{
            //    new DeliveryPoint()
            //    {
            //    }
            //};

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

            var mockPostalAddressDBSet = MockDbSet(postalAddresses);

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockAddressDataService = CreateMock<IPostalAddressDataService>();

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            //mockPostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));
            mockAddressDataService.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddressesDTO[0]);

           // mockReferenceDataCategoryDataService.Setup(x => x.GetReferenceDataId("Postal Address Type", "Nyb")).Returns(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A11"));

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
        }

        private void SetUpdataWithDeliverypoints()
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

            PostalAddressDBDTO postalAddress = new PostalAddressDBDTO()
            {
                UDPRN = 14856,
                BuildingName = "Building one",
                BuildingNumber = 123,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
            };

            dtoPostalAddresses = new PostalAddressDBDTO()
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
            };

            var mockPostalAddressEnumerable = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(lstPostalAddress);
            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockAddressDataService = CreateMock<IPostalAddressDataService>();

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include("DeliveryPoint"));

           // mockPostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));
            mockAddressDataService.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddress);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
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
            mockFmoDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            //mockPostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
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

            PostalAddressDBDTO postalAddress = new PostalAddressDBDTO()
            {
                UDPRN = 14856,
                BuildingName = "Building one",
                BuildingNumber = 123,
                AddressType_GUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15")
            };

            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFmoDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockAddressDataService = CreateMock<IPostalAddressDataService>();

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);

            mockAddressDataService.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddress);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
        }

        private void SetupDataForSearch()
        {
            List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress> postalAddresses = new List<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()
            {
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
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
                },
                new RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress()
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
                }
            };


            var mockPostalAddressEnumerable = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(postalAddresses);
            var mockPostalAddress = MockDbSet(postalAddresses);

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();

            mockPostalAddress.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddress.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockFmoDbContext = CreateMock<PostalAddressDBContext>();

            mockFmoDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddress.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddress.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddress.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
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
                                PostalAddressStatus = new List<PostalAddressStatus>() { new PostalAddressStatus() {} },
                                UDPRN = 14856
                        }
                    };
            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);

            mockFmoDbContext = CreateMock<PostalAddressDBContext>();
            mockFmoDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockLoggingHelper = CreateMock<ILoggingHelper>();

            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();

            mockPostalAddressDBSet.Setup(x => x.Include("PostalAddressStatus"));

            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
        }
    }
}