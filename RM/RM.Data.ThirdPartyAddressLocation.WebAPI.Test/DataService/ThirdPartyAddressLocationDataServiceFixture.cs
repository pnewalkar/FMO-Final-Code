using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DataService;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Entities;

namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Test
{
    [TestFixture]
    public class ThirdPartyAddressLocationDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<IDatabaseFactory<AddressLocationDBContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private IAddressLocationDataService testCandidate;
        private AddressLocationDataDTO addressLocationDataDTO;
        private Mock<AddressLocationDBContext> mockAddressLocationDbContext;

        [Test]
        public void Test_UpdateExistingAddressLocationByUDPRN()
        {
            var result = testCandidate.UpdateExistingAddressLocationByUDPRN(addressLocationDataDTO);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Testcase for deleting an address location
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_DeleteAddressLocation()
        {
            var actualResult = await testCandidate.DeleteAddressLocation(addressLocationDataDTO);
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(1, actualResult);
        }

        /// <summary>
        /// Setup for Nunit Tests.
        /// </summary>
        protected override void OnSetup()
        {
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockAddressLocationDbContext = CreateMock<AddressLocationDBContext>();

            // Data Setup
            AddressLocation addressLocation = new AddressLocation()
            {
                ID = new Guid("5B9C7207-3D20-E711-9F8C-28D244AEF9ED"),
                UDPRN = 1234
            };

            string sbLocationXY = string.Format("POINT({0} {1})", "1234", "4567");
            addressLocationDataDTO = new AddressLocationDataDTO
            {
                ID = Guid.Empty,
                UDPRN = 1234,
                LocationXY = DbGeometry.FromText(sbLocationXY.ToString(), 27700),
                Lattitude = 10,
                Longitude = 45
            };

            // Setup for AddressLocation
            var mockAsynEnumerable1 = new DbAsyncEnumerable<AddressLocation>(new List<AddressLocation>() { addressLocation });
            var mockAddressLocation = MockDbSet(new List<AddressLocation>() { addressLocation });
            mockAddressLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockAddressLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockAddressLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockAddressLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<AddressLocation>)mockAsynEnumerable1).GetAsyncEnumerator());

            mockAddressLocationDbContext.Setup(x => x.Set<AddressLocation>()).Returns(mockAddressLocation.Object);
            mockAddressLocationDbContext.Setup(x => x.AddressLocations).Returns(mockAddressLocation.Object);
            mockAddressLocation.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAddressLocation.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<AddressLocationDBContext>>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();

            // Setup for Logging
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<AddressLocationDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockAddressLocationDbContext.Object);
            mockAddressLocationDbContext.Setup(n => n.SaveChangesAsync()).ReturnsAsync(1);
            testCandidate = new AddressLocationDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}