using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.DataMiddleware;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Entities;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DataService;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;


namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Test
{
    [TestFixture]
    public class ThirdPartyAddressLocationDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<AddressLocationDBContext> mockAddressLocationDBContext;
        private Mock<IDatabaseFactory<AddressLocationDBContext>> mockAddressLocationDBFactory;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private IAddressLocationDataService testCandidate;
        private AddressLocationDataDTO addressLocationDataDTO;

        [Test]
        public void Test_UpdateExistingAddressLocationByUDPRN()
        {
            var result = testCandidate.UpdateExistingAddressLocationByUDPRN(addressLocationDataDTO);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
        }

        /// <summary>
        /// Setup for Nunit Tests.
        /// </summary>
        protected override void OnSetup()
        {
            string sbLocationXY = string.Format("POINT({0} {1})", "1234", "4567");
            addressLocationDataDTO = new AddressLocationDataDTO
            {
                ID = new Guid(),
                UDPRN = 158642,
                LocationXY = DbGeometry.FromText(sbLocationXY.ToString(), 27700),
                Lattitude = 51.64m,
                Longitude = -0.71m
            };

            var addressLocation = new List<AddressLocation>
            {
                new AddressLocation()
                {
                    ID = new Guid(),
                    UDPRN = 158642,
                    LocationXY = DbGeometry.FromText(sbLocationXY.ToString(), 27700),
                    Lattitude = 51.64m,
                    Longitude = -0.71m
                }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<AddressLocation>(addressLocation);
            var mockAddressLocationDataService = MockDbSet(addressLocation);
            mockAddressLocationDataService.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockAddressLocationDataService.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockAddressLocationDataService.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockAddressLocationDataService.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<AddressLocation>)mockAsynEnumerable).GetAsyncEnumerator());
            mockAddressLocationDataService.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAddressLocationDataService.Object);
            mockAddressLocationDataService.Setup(x => x.AsNoTracking()).Returns(mockAddressLocationDataService.Object);

            mockAddressLocationDBContext = CreateMock<AddressLocationDBContext>();
            mockAddressLocationDBContext.Setup(x => x.Set<AddressLocation>()).Returns(mockAddressLocationDataService.Object);
            mockAddressLocationDBContext.Setup(x => x.AddressLocations).Returns(mockAddressLocationDataService.Object);

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockAddressLocationDBFactory = CreateMock<IDatabaseFactory<AddressLocationDBContext>>();
            mockAddressLocationDBFactory.Setup(x => x.Get()).Returns(mockAddressLocationDBContext.Object);
            mockAddressLocationDBContext.Setup(n => n.SaveChangesAsync()).ReturnsAsync(1);
            testCandidate = new AddressLocationDataService(mockAddressLocationDBFactory.Object, mockLoggingHelper.Object);

        }
    }
}