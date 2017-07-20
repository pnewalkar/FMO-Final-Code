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
    //TODO: Nunits to be fixed

    [TestFixture]
    public class PostCodeDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<UnitManagerDbContext> mockUnitManagerDbContext;
        private Mock<IDatabaseFactory<UnitManagerDbContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockILoggingHelper;
        private IPostcodeDataService testCandidate;
        private Guid deliveryUnitID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");
        private Guid postcodeTypeGUID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");
        private SearchInputDataDto searchInputDataDto;
        
        [Test]
        public async Task Test_FetchPostCodeUnitForBasicSearchValid()
        {
            var actualResult = await testCandidate.GetPostcodeUnitForBasicSearch(searchInputDataDto);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.ToList().Count == 1);
        }

        [Test]
        public async Task Test_GetPostcodeUnitCountValid()
        {
            var actualResult = await testCandidate.GetPostcodeUnitCount(searchInputDataDto);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult == 1);
        }

        [Test]
        public async Task Test_GetPostcodeUnitForAdvanceSearchValid()
        {
            var actualResult = await testCandidate.GetPostcodeUnitForAdvanceSearch(searchInputDataDto);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.ToList().Count == 1);
        }

        [Test]
        public async Task Test_GetPostcodeID()
        {
            var actualResult = await testCandidate.GetPostcodeID("123");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(actualResult.ID, new Guid("3534aa41-391f-4579-a18d-d7edf5b5f918"));
        }

        [Test]
        public async Task Test_GetApproxLocation()
        {
            var actualResult = await testCandidate.GetApproxLocation("123", new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"));
            Assert.IsNotNull(actualResult);
           // Assert.AreEqual(actualResult.ID, new Guid("3534aa41-391f-4579-a18d-d7edf5b5f918"));
        }


        /*
        [Test]
        public async Task TestFetchPostCodeUnitForBasicSearchInvalid()
        {
            var actualResult = await testCandidate.FetchPostCodeUnitForBasicSearch("invalid_searchtest", deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestFetchPostCodeUnitForBasicSearchNull()
        {
            var actualResult = await testCandidate.FetchPostCodeUnitForBasicSearch(null, deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 1);
        }

        [Test]
        public async Task TestGetPostCodeUnitCountValid()
        {
            var actualResultCount = await testCandidate.GetPostCodeUnitCount("search", deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 1);
        }

        [Test]
        public async Task TestGetPostCodeUnitCountInvalid()
        {
            var actualResultCount = await testCandidate.GetPostCodeUnitCount("searchtest", deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 0);
        }

        [Test]
        public async Task TestGetPostCodeUnitCountNull()
        {
            var actualResultCount = await testCandidate.GetPostCodeUnitCount(null, deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 1);
        }*/

        protected override void OnSetup()
        {

            List<Postcode> postcodeList = new List<Postcode>()
            {
                new Postcode()
                {
                    PostcodeUnit = "123",
                    InwardCode = "1",
                    OutwardCode = "2",
                    ID = new Guid("3534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

           List<PostcodeHierarchy> postcodeHierarchyList = new List<PostcodeHierarchy>()
            {
                new PostcodeHierarchy()
                {
                    ID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"),
                    PostcodeTypeGUID = postcodeTypeGUID,
                    ParentPostcode = "111",
                    Postcode = "123"
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

            List<DeliveryPoint> deliveryPointList = new List<DeliveryPoint>()
            {
                new DeliveryPoint()
                {
                    PostalAddress = new PostalAddress() { }
                }
            };

            searchInputDataDto = new SearchInputDataDto()
            {
                SearchText = "12",
                PostcodeTypeGUID = postcodeTypeGUID,
                SearchResultCount = 2,
                UserUnitLocationId = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
            };

            List<PostalAddressIdentifier> postalAddressIdentifierList = new List<PostalAddressIdentifier>()
            {
                new PostalAddressIdentifier()
                {
                    ID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            mockUnitManagerDbContext = CreateMock<UnitManagerDbContext>();
            mockILoggingHelper = CreateMock<ILoggingHelper>();

            //Setup for PostcodeHierarchy
            var mockAsynEnumerable = new DbAsyncEnumerable<PostcodeHierarchy>(postcodeHierarchyList);
            var mockPostcodeHierarchy = MockDbSet(postcodeHierarchyList);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostcodeHierarchy>)mockAsynEnumerable).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostcodeHierarchy>()).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.PostcodeHierarchies).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(c => c.PostcodeHierarchies.AsNoTracking()).Returns(mockPostcodeHierarchy.Object);

            //Setup for PostalAddressIdentifier
            var mockAsynEnumerable1 = new DbAsyncEnumerable<PostalAddressIdentifier>(postalAddressIdentifierList);
            var mockPostalAddressIdentifier = MockDbSet(postalAddressIdentifierList);
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockPostalAddressIdentifier.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddressIdentifier>)mockAsynEnumerable1).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostalAddressIdentifier>()).Returns(mockPostalAddressIdentifier.Object);
            mockUnitManagerDbContext.Setup(x => x.PostalAddressIdentifiers).Returns(mockPostalAddressIdentifier.Object);
            mockUnitManagerDbContext.Setup(c => c.PostalAddressIdentifiers.AsNoTracking()).Returns(mockPostalAddressIdentifier.Object);

            //Setup for LocationPostcodeHierarchy
            var mockAsynEnumerable2 = new DbAsyncEnumerable<LocationPostcodeHierarchy>(locationPostcodeHierarchyList);
            var mockLocationPostcodeHierarchy = MockDbSet(locationPostcodeHierarchyList);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockLocationPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<LocationPostcodeHierarchy>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<LocationPostcodeHierarchy>()).Returns(mockLocationPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.LocationPostcodeHierarchies).Returns(mockLocationPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(c => c.LocationPostcodeHierarchies.AsNoTracking()).Returns(mockLocationPostcodeHierarchy.Object);

            //Setup for Location
            var mockAsynEnumerable3 = new DbAsyncEnumerable<Location>(locationList);
            var mockLocation = MockDbSet(locationList);
            mockLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Location>)mockAsynEnumerable3).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<Location>()).Returns(mockLocation.Object);
            mockUnitManagerDbContext.Setup(x => x.Locations).Returns(mockLocation.Object);
            mockUnitManagerDbContext.Setup(c => c.Locations.AsNoTracking()).Returns(mockLocation.Object);

            //Setup for Postcode
            var mockAsynEnumerable4 = new DbAsyncEnumerable<Postcode>(postcodeList);
            var mockPostcode = MockDbSet(postcodeList);
            mockPostcode.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable4.AsQueryable().Provider);
            mockPostcode.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable4.AsQueryable().Expression);
            mockPostcode.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable4.AsQueryable().ElementType);
            mockPostcode.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Postcode>)mockAsynEnumerable4).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<Postcode>()).Returns(mockPostcode.Object);
            mockUnitManagerDbContext.Setup(x => x.Postcodes).Returns(mockPostcode.Object);

            //Setup for DeliveryPoint
            var mockAsynEnumerable5 = new DbAsyncEnumerable<DeliveryPoint>(deliveryPointList);
            var mockDeliveryPoint = MockDbSet(deliveryPointList);
            mockDeliveryPoint.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable5.AsQueryable().Provider);
            mockDeliveryPoint.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable5.AsQueryable().Expression);
            mockDeliveryPoint.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable5.AsQueryable().ElementType);
            mockDeliveryPoint.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPoint>)mockAsynEnumerable5).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPoint.Object);
            mockUnitManagerDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPoint.Object);
            mockUnitManagerDbContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPoint.Object);
            

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<UnitManagerDbContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockUnitManagerDbContext.Object);
            testCandidate = new PostcodeDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object);
        }
    }

}
