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
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataService;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.Entity;

namespace RM.Data.UnitManager.WebAPI.Test.DataService
{
    [TestFixture]
    public class PostalAddressDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<UnitManagerDbContext> mockUnitManagerDbContext;
        private Mock<IDatabaseFactory<UnitManagerDbContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockILoggingHelper;
        private IPostalAddressDataService testCandidate;
        private Guid postcodeTypeGUID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");
        private SearchInputDataDto searchInputDataDto;

        [Test]
        public async Task Test_GetPostcodeUnitForBasicSearch()
        {
            var result = await testCandidate.GetPostcodeUnitForBasicSearch(searchInputDataDto);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ToList().Count == 1);
        }

        [Test]
        public async Task Test_GetPostalAddressSearchDetails()
        {
            var result = await testCandidate.GetPostalAddressSearchDetails("12", new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"), new List<Guid>() { postcodeTypeGUID });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ToList().Count == 1);
            Assert.AreEqual(result.ToList()[0], "12,123");
        }

        [Test]
        public async Task Test_GetPostalAddressDetails_With_PostcodeAndStreet()
        {
            var result = await testCandidate.GetPostalAddressDetails("12,123", new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ToList().Count == 1);
            Assert.AreEqual(result.ToList()[0].Postcode, "123");
            Assert.AreEqual(result.ToList()[0].Thoroughfare, "12");
        }

        [Test]
        public async Task Test_GetPostalAddressDetails_With_Postcode()
        {
            var result = await testCandidate.GetPostalAddressDetails("123", new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ToList().Count == 1);
            Assert.AreEqual(result.ToList()[0].Postcode, "123");
        }

        protected override void OnSetup()
        {
            List<PostcodeHierarchy> postcodeHierarchyList = new List<PostcodeHierarchy>()
            {
                new PostcodeHierarchy()
                {
                    ID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"),
                    PostcodeTypeGUID = postcodeTypeGUID,
                    ParentPostcode = "111",
                    Postcode = "123",
                }
            };

            List<LocationPostcodeHierarchy> locationPostcodeHierarchyList = new List<LocationPostcodeHierarchy>()
            {
                new LocationPostcodeHierarchy()
                {
                    PostcodeHierarchyID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"),
                    LocationID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            List<Location> locationList = new List<Location>()
            {
                new Location()
                {
                     ID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            List<PostalAddress> postalAddressList = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    UDPRN = 123,
                    Postcode = "123",
                    AddressType_GUID = postcodeTypeGUID,
                    Thoroughfare = "12"
                }
            };

            searchInputDataDto = new SearchInputDataDto()
            {
                SearchText = "12",
                PostcodeTypeGUID = postcodeTypeGUID,
                SearchResultCount = 2,
                UserUnitLocationId = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
            };

            mockUnitManagerDbContext = CreateMock<UnitManagerDbContext>();
            mockILoggingHelper = CreateMock<ILoggingHelper>();

            //Setup for PostcodeHierarchy
            var mockAsynEnumerable4 = new DbAsyncEnumerable<PostcodeHierarchy>(postcodeHierarchyList);
            var mockPostcodeHierarchy = MockDbSet(postcodeHierarchyList);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable4.AsQueryable().Provider);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable4.AsQueryable().Expression);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable4.AsQueryable().ElementType);
            mockPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostcodeHierarchy>)mockAsynEnumerable4).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostcodeHierarchy>()).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.PostcodeHierarchies).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(c => c.PostcodeHierarchies.AsNoTracking()).Returns(mockPostcodeHierarchy.Object);

            //Setup for LocationPostcodeHierarchy
            var mockAsynEnumerable7 = new DbAsyncEnumerable<LocationPostcodeHierarchy>(locationPostcodeHierarchyList);
            var mockLocationPostcodeHierarchy = MockDbSet(locationPostcodeHierarchyList);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable7.AsQueryable().Provider);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable7.AsQueryable().Expression);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable7.AsQueryable().ElementType);
            mockLocationPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<LocationPostcodeHierarchy>)mockAsynEnumerable7).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<LocationPostcodeHierarchy>()).Returns(mockLocationPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.LocationPostcodeHierarchies).Returns(mockLocationPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(c => c.LocationPostcodeHierarchies.AsNoTracking()).Returns(mockLocationPostcodeHierarchy.Object);

            //Setup for Location
            var mockAsynEnumerable2 = new DbAsyncEnumerable<Location>(locationList);
            var mockLocation = MockDbSet(locationList);
            mockLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Location>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<Location>()).Returns(mockLocation.Object);
            mockUnitManagerDbContext.Setup(x => x.Locations).Returns(mockLocation.Object);
            mockUnitManagerDbContext.Setup(c => c.Locations.AsNoTracking()).Returns(mockLocation.Object);

            //Setup for PostalAddress
            var mockAsynEnumerable5 = new DbAsyncEnumerable<PostalAddress>(postalAddressList);
            var mockPostalAddress = MockDbSet(postalAddressList);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable5.AsQueryable().Provider);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable5.AsQueryable().Expression);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable5.AsQueryable().ElementType);
            mockPostalAddress.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddress>)mockAsynEnumerable5).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddress.Object);
            mockUnitManagerDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddress.Object);
            mockUnitManagerDbContext.Setup(c => c.PostalAddresses.AsNoTracking()).Returns(mockPostalAddress.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<UnitManagerDbContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockUnitManagerDbContext.Object);
            testCandidate = new PostalAddressDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object);
        }
    }
}