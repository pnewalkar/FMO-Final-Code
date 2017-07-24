using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Implementation;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.Entities;
using System;

namespace RM.DataServices.Tests.DataService
{
    /// <summary>
    /// This class contains test methods for OSRoadLinkDataService.
    /// </summary>
    [TestFixture]
    public class OSRoadLinkDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<NetworkDBContext> mockNetworkDBContext;
        private Mock<IDatabaseFactory<NetworkDBContext>> mockDatabaseFactory;
        private IOSRoadLinkDataService testCandidate;
        private Mock<ILoggingHelper> mockILoggingHelper;

        /// <summary>
        ///Test for fetch data for OSRoadLink.
        /// </summary>
        [Test]
        public void Test_GetOSRoadLink()
        {
            var actualResult = testCandidate.GetOSRoadLink("123");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(actualResult.Result, "1");
        }

        /// <summary>
        /// Setup for Nunit Tests
        /// </summary>
        protected override void OnSetup()
        {
            mockILoggingHelper = CreateMock<ILoggingHelper>();
            List<OSRoadLink> oSRoadLink = new List<OSRoadLink>() { new OSRoadLink() { TOID = "123", RouteHierarchy = "1" } };

            //Setup for OSRoadLink.
            var mockOSRoadLinkEnumerable = new DbAsyncEnumerable<OSRoadLink>(oSRoadLink);
            var mockOSRoadLinkDataService = MockDbSet(oSRoadLink);
            mockOSRoadLinkDataService.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockOSRoadLinkEnumerable.AsQueryable().Provider);
            mockOSRoadLinkDataService.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockOSRoadLinkEnumerable.AsQueryable().Expression);
            mockOSRoadLinkDataService.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockOSRoadLinkEnumerable.AsQueryable().ElementType);
            mockOSRoadLinkDataService.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<OSRoadLink>)mockOSRoadLinkEnumerable).GetAsyncEnumerator());

            mockNetworkDBContext = CreateMock<NetworkDBContext>();
            mockNetworkDBContext.Setup(x => x.Set<OSRoadLink>()).Returns(mockOSRoadLinkDataService.Object);
            mockNetworkDBContext.Setup(x => x.OSRoadLinks).Returns(mockOSRoadLinkDataService.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<NetworkDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockNetworkDBContext.Object);

            //Setup for IRMTraceManager.
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new OSRoadLinkDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object);
        }
    }
}