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
using RM.DataManagement.UnitManager.WebAPI.DataService;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.Entity;

namespace RM.Data.UnitManager.WebAPI.Test.DataService
{
    /// <summary>
    /// This class contains test methods for UnitLocationDataService
    /// </summary>
    [TestFixture]
    public class UnitLocationDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<UnitManagerDbContext> mockUnitManagerDbContext;
        private Mock<IDatabaseFactory<UnitManagerDbContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockILoggingHelper;
        private IUnitLocationDataService testCandidate;
        private Guid postcodeTypeGUID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");
        private Guid userId = new Guid("2534AA41-391F-4579-A18D-D7EDF5B5F918");
        private List<PostcodeHierarchy> postcodeHierarchyList;
        private List<Location> locationList;

        /// <summary>
        /// Passed all correct values
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetUnitsByUser_PositiveScenario()
        {
            OnSetup();
            var result = await testCandidate.GetUnitsByUser(userId, postcodeTypeGUID);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ToList().Count, 1);
            Assert.AreEqual(result.ToList()[0].LocationId, new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"));
            Assert.AreEqual(result.ToList()[0].Area, "123");
        }

        /// <summary>
        /// Passed invalid userid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetUnitsByUser_NegativeScenario()
        {
            var result = await testCandidate.GetUnitsByUser(Guid.Empty, postcodeTypeGUID);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ToList().Count, 0);
        }

        /// <summary>
        /// Passed all correct values
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetPostcodes_PositiveScenario()
        {
            // Setup for PostcodeHierarchy with mocked AsNoTracking()
            var mockAsynEnumerable4 = new DbAsyncEnumerable<PostcodeHierarchy>(postcodeHierarchyList);
            var mockPostcodeHierarchy = MockDbSet(postcodeHierarchyList);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable4.AsQueryable().Provider);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable4.AsQueryable().Expression);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable4.AsQueryable().ElementType);
            mockPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostcodeHierarchy>)mockAsynEnumerable4).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostcodeHierarchy>()).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.PostcodeHierarchies).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(c => c.PostcodeHierarchies.AsNoTracking()).Returns(mockPostcodeHierarchy.Object);

            var result = await testCandidate.GetPostcodes(new List<Guid>() { new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918") }, postcodeTypeGUID);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ToList().Count, 1);
            Assert.AreEqual(result.ToList()[0].ID, new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"));
        }

        /// <summary>
        /// Passed list of empty postcodeGuid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetPostcodes_NegativeScenario()
        {
            // Setup for PostcodeHierarchy with mocked AsNoTracking()
            var mockAsynEnumerable4 = new DbAsyncEnumerable<PostcodeHierarchy>(postcodeHierarchyList);
            var mockPostcodeHierarchy = MockDbSet(postcodeHierarchyList);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable4.AsQueryable().Provider);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable4.AsQueryable().Expression);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable4.AsQueryable().ElementType);
            mockPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostcodeHierarchy>)mockAsynEnumerable4).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostcodeHierarchy>()).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.PostcodeHierarchies).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(c => c.PostcodeHierarchies.AsNoTracking()).Returns(mockPostcodeHierarchy.Object);

            var result = await testCandidate.GetPostcodes(new List<Guid>() { Guid.Empty }, postcodeTypeGUID);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ToList().Count, 0);
        }

        /// <summary>
        /// get the unit id for user above mail center by passing all correct values
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetUnitsByUserForNational()
        {
            OnSetup();
            var result = await testCandidate.GetUnitsByUserForNational(userId, postcodeTypeGUID);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ToList().Count, 1);
            Assert.AreEqual(result.ToList()[0].Area, "123");
            Assert.AreEqual(result.ToList()[0].LocationId, new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"));
        }

        /// <summary>
        /// setup for nunit tests
        /// </summary>
        protected override void OnSetup()
        {
            // Data Setup
            List<PostalAddressIdentifier> postalAddressIdentifierList = new List<PostalAddressIdentifier>()
            {
                new PostalAddressIdentifier()
                {
                    ID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            locationList = new List<Location>()
            {
                new Location()
                {
                     ID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            List<UserRoleLocation> userRoleLocationList = new List<UserRoleLocation>()
            {
                new UserRoleLocation()
                {
                    LocationID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"),
                    UserID = userId
                }
            };

            postcodeHierarchyList = new List<PostcodeHierarchy>()
            {
                new PostcodeHierarchy()
                {
                    ID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"),
                    PostcodeTypeGUID = postcodeTypeGUID,
                    ParentPostcode = "111",
                    Postcode = "123",
                }
            };

            List<Postcode> postcodeList = new List<Postcode>()
            {
                new Postcode()
                {
                    ID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918"),
                    InwardCode = "a",
                    OutwardCode = "b",
                    PostcodeUnit = "unit1",
                }
            };

            List<LocationReferenceData> locationReferenceDataList = new List<LocationReferenceData>()
            {
                new LocationReferenceData()
                {
                    LocationID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918")
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

            mockUnitManagerDbContext = CreateMock<UnitManagerDbContext>();
            mockILoggingHelper = CreateMock<ILoggingHelper>();

            // Setup for PostalAddressIdentifier
            var mockAsynEnumerable1 = new DbAsyncEnumerable<PostalAddressIdentifier>(postalAddressIdentifierList);
            var mockPostalAddressIdentifier = MockDbSet(postalAddressIdentifierList);
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockPostalAddressIdentifier.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddressIdentifier>)mockAsynEnumerable1).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostalAddressIdentifier>()).Returns(mockPostalAddressIdentifier.Object);
            mockUnitManagerDbContext.Setup(x => x.PostalAddressIdentifiers).Returns(mockPostalAddressIdentifier.Object);
            mockUnitManagerDbContext.Setup(c => c.PostalAddressIdentifiers.AsNoTracking()).Returns(mockPostalAddressIdentifier.Object);

            // Setup for Location
            var mockAsynEnumerable2 = new DbAsyncEnumerable<Location>(locationList);
            var mockLocation = MockDbSet(locationList);
            mockLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Location>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<Location>()).Returns(mockLocation.Object);
            mockUnitManagerDbContext.Setup(x => x.Locations).Returns(mockLocation.Object);
            mockUnitManagerDbContext.Setup(c => c.Locations.AsNoTracking()).Returns(mockLocation.Object);

            // Setup for UserRoleLocation
            var mockAsynEnumerable3 = new DbAsyncEnumerable<UserRoleLocation>(userRoleLocationList);
            var mockUserRoleLocation = MockDbSet(userRoleLocationList);
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockUserRoleLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UserRoleLocation>)mockAsynEnumerable3).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<UserRoleLocation>()).Returns(mockUserRoleLocation.Object);
            mockUnitManagerDbContext.Setup(x => x.UserRoleLocations).Returns(mockUserRoleLocation.Object);
            mockUnitManagerDbContext.Setup(c => c.UserRoleLocations.AsNoTracking()).Returns(mockUserRoleLocation.Object);

            // Setup for PostcodeHierarchy
            var mockAsynEnumerable4 = new DbAsyncEnumerable<PostcodeHierarchy>(postcodeHierarchyList);
            var mockPostcodeHierarchy = MockDbSet(postcodeHierarchyList);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable4.AsQueryable().Provider);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable4.AsQueryable().Expression);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable4.AsQueryable().ElementType);
            mockPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostcodeHierarchy>)mockAsynEnumerable4).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostcodeHierarchy>()).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.PostcodeHierarchies).Returns(mockPostcodeHierarchy.Object);

            // Setup for Postcode
            var mockAsynEnumerable5 = new DbAsyncEnumerable<Postcode>(postcodeList);
            var mockPostcode = MockDbSet(postcodeList);
            mockPostcode.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable5.AsQueryable().Provider);
            mockPostcode.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable5.AsQueryable().Expression);
            mockPostcode.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable5.AsQueryable().ElementType);
            mockPostcode.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Postcode>)mockAsynEnumerable5).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<Postcode>()).Returns(mockPostcode.Object);
            mockUnitManagerDbContext.Setup(x => x.Postcodes).Returns(mockPostcode.Object);
            mockUnitManagerDbContext.Setup(c => c.Postcodes.AsNoTracking()).Returns(mockPostcode.Object);

            // Setup for Postcode
            var mockAsynEnumerable6 = new DbAsyncEnumerable<LocationReferenceData>(locationReferenceDataList);
            var mockLocationReferenceData = MockDbSet(locationReferenceDataList);
            mockLocationReferenceData.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable6.AsQueryable().Provider);
            mockLocationReferenceData.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable6.AsQueryable().Expression);
            mockLocationReferenceData.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable6.AsQueryable().ElementType);
            mockLocationReferenceData.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<LocationReferenceData>)mockAsynEnumerable6).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<LocationReferenceData>()).Returns(mockLocationReferenceData.Object);
            mockUnitManagerDbContext.Setup(x => x.LocationReferenceDatas).Returns(mockLocationReferenceData.Object);

            // Setup for LocationPostcodeHierarchy
            var mockAsynEnumerable7 = new DbAsyncEnumerable<LocationPostcodeHierarchy>(locationPostcodeHierarchyList);
            var mockLocationPostcodeHierarchy = MockDbSet(locationPostcodeHierarchyList);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable7.AsQueryable().Provider);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable7.AsQueryable().Expression);
            mockLocationPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable7.AsQueryable().ElementType);
            mockLocationPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<LocationPostcodeHierarchy>)mockAsynEnumerable7).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<LocationPostcodeHierarchy>()).Returns(mockLocationPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.LocationPostcodeHierarchies).Returns(mockLocationPostcodeHierarchy.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<UnitManagerDbContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockUnitManagerDbContext.Object);
            testCandidate = new UnitLocationDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object);
        }
    }
}