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
using RM.DataManagement.PostalAddress.WebAPI.DataDTO;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Implementation;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.Entities;

namespace RM.Data.PostalAddress.WebAPI.Test.DataService
{
    [TestFixture]
    public class AddressDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<PostalAddressDBContext> mockPostalAddressDbContext;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IFileProcessingLogDataService> mockFileProcessingLog;
        private Mock<IDatabaseFactory<PostalAddressDBContext>> mockDatabaseFactory;

        // private Mock<CommonLibrary.EntityFramework.DataService.Interfaces.IPostCodeDataService> mockPostCodeDataService;
        // private Mock<CommonLibrary.EntityFramework.DataService.Interfaces.IReferenceDataCategoryDataService> mockReferenceDataCategoryDataService;
        private Mock<IPostalAddressDataService> mockAddressDataService;

        private IPostalAddressDataService testCandidate;
        private List<PostalAddressDataDTO> postalAddressesDTO;
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
            string results = testCandidate.CheckForDuplicateNybRecords(postalAddressesDTO[0], new Guid("222C68A4-D959-4B37-B468-4B1855950A81"));
            Assert.NotNull(results);
            Assert.IsEmpty(results.ToString());            
        }

        [Test]
        public void Test_CheckForDuplicateNybRecords_NotDuplicate()
        {
            SetUpDataForDeliveryPoints();
            string results = testCandidate.CheckForDuplicateNybRecords(postalAddressesDTO[1], new Guid("222C68A4-D959-4B37-B468-4B1855950A81"));
            Assert.NotNull(results);
            Assert.IsTrue(results == string.Empty);
        }

        [Test]
        public void Test_CreateAddressForDeliveryPoint()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var result = testCandidate.CreateAddressForDeliveryPoint(postalAddressesDTO[0]);
            Assert.NotNull(result);
            Assert.IsTrue(result != Guid.Empty);
        }

        [Test]
        public void Test_CheckForDuplicateAddressWithDeliveryPoints()
        {
            SetUpDataForCreateAddressAndDeliveryPoint();
            var result = testCandidate.CheckForDuplicateAddressWithDeliveryPoints(postalAddressesDTO[0]);
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
            postalAddressesDTO = new List<PostalAddressDataDTO>()
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

            // List<RM.DataManagement.PostalAddress.WebAPI.Entities.deli> deliveryPoint = new List<DeliveryPoint>()
            // {
            //    new DeliveryPoint()
            //    {
            //    }
            // };
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
                ID = new Guid()
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
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockAddressDataService = CreateMock<IPostalAddressDataService>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);

            mockAddressDataService.Setup(n => n.CheckForDuplicateNybRecords(It.IsAny<PostalAddressDataDTO>(), It.IsAny<Guid>())).Returns(("Postcode"));
            mockAddressDataService.Setup(n => n.CheckForDuplicateAddressWithDeliveryPoints(It.IsAny<PostalAddressDataDTO>())).Returns(Task.FromResult(true));
            mockAddressDataService.Setup(n => n.GetPAFAddress(It.IsAny<int>(), It.IsAny<Guid>())).Returns(Task.FromResult(postalAddressDTO));

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
        }

        private void SetUpDataForCreateAddressAndDeliveryPoint()
        {
            postalAddressesDTO = new List<PostalAddressDataDTO>()
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

            // List<DeliveryPoint> deliveryPoint = new List<DeliveryPoint>()
            // {
            //    new DeliveryPoint()
            //    {
            //    }
            // };
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
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockAddressDataService = CreateMock<IPostalAddressDataService>();
            

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);

            var mockPostalAddressDTO = CreateMock<PostalAddressDataDTO>();
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            // mockPostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));
            mockAddressDataService.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddressesDTO[0]);
            mockAddressDataService.Setup(x => x.CreateAddressForDeliveryPoint(mockPostalAddressDTO.Object)).Returns(It.IsAny<Guid>());

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

                // PostCodeGUID = new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"),
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

            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            
            mockAddressDataService = CreateMock<IPostalAddressDataService>();

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);


            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);          
                                    
            // mockPostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));
            //mockAddressDataService.Setup(x => x.GetPostalAddressDetails(It.IsAny<Guid>())).Returns(postalAddress);

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
            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();

            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);

            // mockPostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).Returns(Task.FromResult(Guid.NewGuid()));
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
            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();
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

            var mockPostalAddressEnumerable = new DbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>(lstPostalAddress);
            var mockPostalAddressDBSet = MockDbSet(lstPostalAddress);
            mockPostalAddressDBSet.Setup(x => x.Include("PostalAddressStatus"));
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);

            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockPostalAddressEnumerable.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockPostalAddressEnumerable.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockPostalAddressEnumerable.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>)mockPostalAddressEnumerable).GetAsyncEnumerator());

            mockPostalAddressDbContext = CreateMock<PostalAddressDBContext>();
            mockPostalAddressDbContext.Setup(x => x.Set<RM.DataManagement.PostalAddress.WebAPI.Entities.PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockLoggingHelper = CreateMock<ILoggingHelper>();

            mockFileProcessingLog = CreateMock<IFileProcessingLogDataService>();

            mockDatabaseFactory = CreateMock<IDatabaseFactory<PostalAddressDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockPostalAddressDbContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object);
        }
    }
}