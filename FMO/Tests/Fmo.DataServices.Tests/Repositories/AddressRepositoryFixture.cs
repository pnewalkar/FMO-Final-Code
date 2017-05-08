using System;
using System.Collections.Generic;
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
using System.Linq;
using System.Data.Entity.Infrastructure;
using Fmo.Common.AsyncEnumerator;
using System.Threading.Tasks;

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

        [Test]
        public void Test_UpdateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = new PostalAddressDTO() { Address_Id = 28, UDPRN = 14856 };
            var result = testCandidate.SaveAddress(objstPostalAddressDTO, "NYB.CSV");
            mockFmoDbContext.Verify(n => n.SaveChanges(), Times.Once);
            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_CreateAddressValidTestCase()
        {
            SetUpdataWithDeliverypoints();
            PostalAddressDTO objstPostalAddressDTO = new PostalAddressDTO() { Address_Id = 326, UDPRN = 15862 };
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
            SetUpdataWithDeliverypoints();
            var result = testCandidate.GetPostalAddressDetails(new Guid("019DBBBB-03FB-489C-8C8D-F1085E0D2A15"));

           // Assert.NotNull(result);
        }

        protected override void OnSetup()
        {
        }

        [Test]
        public async Task Test_SearchByPostcode()
        {
            SetupDataForSearch();
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("Postcode1", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare1, Postcode1");
        }

        [Test]
        public async Task Test_SearchByThoroughFare()
        {
            SetupDataForSearch();
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("ThoroughFare2", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare2, Postcode2");
        }

        [Test]
        public async Task Test_SearchByDependentThoroughFare()
        {
            SetupDataForSearch();
            List<string> results = await testCandidate.GetPostalAddressSearchDetails("DependentThoroughFare1", new Guid("00000000-0000-0000-0000-000000000000"));
            Assert.NotNull(results);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0] == "ThoroughFare1, Postcode1");
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

        private void SetUpdataWithDeliverypoints()
        {
            List<PostalAddress> lstPostalAddress = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    ID= Guid.NewGuid(),
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
                Address_Id = 10,
                AddressType_Id = 2,
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
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>()));
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
                Address_Id = 10,
                AddressType_Id = 2,
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
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>()));
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            mockFmoDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockPostalAddressDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressDBSet.Object);
            mockFmoDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
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
                DeliveryRoute_Id = 10,
                OperationalStatus_Id = 5,
                RouteMethodType_Id = 5,
                ID = new Guid("00000000-0000-0000-0000-000000000001"),
                OperationalStatus_GUID = new Guid("00000000-0000-0000-0000-000000000001"),
                RouteMethodType_GUID = new Guid("00000000-0000-0000-0000-000000000001"),
                RouteName = "DeliveryRoute1"
            };

            var deliveryRoute2 = new DeliveryRoute()
            {
                DeliveryRoute_Id = 10,
                OperationalStatus_Id = 5,
                RouteMethodType_Id = 5,
                ID = new Guid("00000000-0000-0000-0000-000000000002"),
                OperationalStatus_GUID = new Guid("00000000-0000-0000-0000-000000000002"),
                RouteMethodType_GUID = new Guid("00000000-0000-0000-0000-000000000002"),
                RouteName = "DeliveryRoute2"
            };

            var unitLocationPostcode1 = new UnitLocationPostcode()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000000"),
                UnitLocationPostcodeId = 10,
                PostcodeUnit = "test",
                Unit_Id = 10,
                Unit_GUID = new Guid("00000000-0000-0000-0000-000000000000"),
                PoscodeUnit_GUID = new Guid("00000000-0000-0000-0000-000000000002")
            };

            var unitLocationPostcode2 = new UnitLocationPostcode()
            {
                ID = new Guid("00000000-0000-0000-0000-000000000001"),
                UnitLocationPostcodeId = 10,
                PostcodeUnit = "test",
                Unit_Id = 10,
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
                    Address_Id = 10,
                    AddressType_Id = 2,
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
                    Address_Id = 10,
                    AddressType_Id = 2,
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
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>()));

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

            mockReferenceDataCategoryRepository.Setup(x => x.GetReferenceDataId("Postal Address Type", "Paf")).Returns(new Guid("A21F3E46-2D0D-4989-A5D5-872D23B479A2"));
            mockReferenceDataCategoryRepository.Setup(x => x.GetReferenceDataId("Postal Address Type", "Nyb")).Returns(new Guid("222C68A4-D959-4B37-B468-4B1855950A81"));

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new AddressRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockFileProcessingLog.Object, mockPostCodeRepository.Object, mockReferenceDataCategoryRepository.Object);
        }
    }
}