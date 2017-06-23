using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.DataServices.Tests.DataService
{
    [TestFixture]
    public class OSRoadLinkDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IOSRoadLinkDataService testCandidate;
        private Mock<ILoggingHelper> loggingHelperMock;

        [Test]
        public void Test_GetOSRoadLink()
        {
            var actualResult = testCandidate.GetOSRoadLink("123");
            Assert.IsNotNull(actualResult);
        }

        protected override void OnSetup()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            loggingHelperMock = CreateMock<ILoggingHelper>();

            List<OSRoadLink> oSRoadLink = new List<OSRoadLink>() { new OSRoadLink() { TOID = "123", RouteHierarchy = "1" } };
            var mockOSRoadLinkEnumerable = new DbAsyncEnumerable<OSRoadLink>(oSRoadLink);
            var mockOSRoadLinkDataService = MockDbSet(oSRoadLink);

            mockOSRoadLinkDataService.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockOSRoadLinkEnumerable.AsQueryable().Provider);
            mockOSRoadLinkDataService.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockOSRoadLinkEnumerable.AsQueryable().Expression);
            mockOSRoadLinkDataService.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockOSRoadLinkEnumerable.AsQueryable().ElementType);
            mockOSRoadLinkDataService.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<OSRoadLink>)mockOSRoadLinkEnumerable).GetAsyncEnumerator());

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<OSRoadLink>()).Returns(mockOSRoadLinkDataService.Object);
            mockRMDBContext.Setup(x => x.OSRoadLinks).Returns(mockOSRoadLinkDataService.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new OSRoadLinkDataService(mockDatabaseFactory.Object, loggingHelperMock.Object);
        }
    }
}