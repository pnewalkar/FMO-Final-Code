using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.DataMiddleware;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Entities;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DataService;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using System.Data.Entity.Spatial;

namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Test
{
    [TestFixture]
    public class ThirdPartyAddressLocationDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<IDatabaseFactory<AddressLocationDBContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private IAddressLocationDataService testCandidate;
        private AddressLocationDataDTO addressLocationDataDTO;

        [Test]
        public void Test_UpdateExistingAddressLocationByUDPRN()
        {
            var result = testCandidate.UpdateExistingAddressLocationByUDPRN(addressLocationDataDTO);
            Assert.IsNotNull(result);
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

            mockDatabaseFactory = CreateMock<IDatabaseFactory<AddressLocationDBContext>>();
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new AddressLocationDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);

        }
    }
}
