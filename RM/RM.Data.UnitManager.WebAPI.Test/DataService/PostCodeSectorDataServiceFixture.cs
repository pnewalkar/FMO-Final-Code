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
    /// This class contains test methods for PostcodeSectorDataService
    /// </summary>
    [TestFixture]
    public class PostCodeSectorDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<UnitManagerDbContext> mockUnitManagerDbContext;
        private Mock<IDatabaseFactory<UnitManagerDbContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockILoggingHelper;
        private IPostcodeSectorDataService testCandidate;
        private Guid postcodeTypeGUID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");

        /// <summary>
        /// All values passed correctly
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetPostcodeSectorByUdprn_PositiveScenario()
        {
            var result = await testCandidate.GetPostcodeSectorByUdprn(123);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.District, "111");
            Assert.AreEqual(result.Sector, "111");
        }

        /// <summary>
        /// Value of UDPRN is 0
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetPostcodeSectorByUdprn_NegativeScenario1()
        {
            var result = await testCandidate.GetPostcodeSectorByUdprn(0);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.District, null);
            Assert.AreEqual(result.Sector, null);
        }

        /// <summary>
        /// Value of postcodeSectorTypeGuid is empty
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetPostcodeSectorByUdprn_NegativeScenario2()
        {
            var result = await testCandidate.GetPostcodeSectorByUdprn(123);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Sector, null);
            Assert.AreEqual(result.District, "111");
        }

        /// <summary>
        /// Value of postcodeDistrictTypeGuid is empty
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetPostcodeSectorByUdprn_NegativeScenario3()
        {
            var result = await testCandidate.GetPostcodeSectorByUdprn(123);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Sector, "111");
            Assert.AreEqual(result.District, null);
        }

        /// <summary>
        /// Setup for Nunit Tests
        /// </summary>
        protected override void OnSetup()
        {
            // Data Setup
            List<PostcodeHierarchy> postcodeHierarchyList = new List<PostcodeHierarchy>()
            {
                new PostcodeHierarchy()
                {
                    PostcodeTypeGUID = postcodeTypeGUID,
                    ParentPostcode = "111",
                    Postcode = "123"
                }
            };

            List<PostalAddress> postalAddressList = new List<PostalAddress>()
            {
                new PostalAddress()
                {
                    UDPRN = 123,
                    Postcode = "123"
                }
            };

            mockUnitManagerDbContext = CreateMock<UnitManagerDbContext>();
            mockILoggingHelper = CreateMock<ILoggingHelper>();

            // Setup for PostcodeHierarchy
            var mockAsynEnumerable1 = new DbAsyncEnumerable<PostcodeHierarchy>(postcodeHierarchyList);
            var mockPostcodeHierarchy = MockDbSet(postcodeHierarchyList);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockPostcodeHierarchy.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockPostcodeHierarchy.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostcodeHierarchy>)mockAsynEnumerable1).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostcodeHierarchy>()).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(x => x.PostcodeHierarchies).Returns(mockPostcodeHierarchy.Object);
            mockUnitManagerDbContext.Setup(c => c.PostcodeHierarchies.AsNoTracking()).Returns(mockPostcodeHierarchy.Object);

            // Setup for PostalAddress
            var mockAsynEnumerable2 = new DbAsyncEnumerable<PostalAddress>(postalAddressList);
            var mockPostalAddress = MockDbSet(postalAddressList);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockPostalAddress.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockPostalAddress.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddress>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddress.Object);
            mockUnitManagerDbContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddress.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<UnitManagerDbContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockUnitManagerDbContext.Object);
            testCandidate = new PostcodeSectorDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object);
        }
    }
}