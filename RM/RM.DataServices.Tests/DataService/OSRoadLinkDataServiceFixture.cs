using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.DataServices.Tests.DataService
{
    [TestFixture]
    public class OSRoadLinkDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IOSRoadLinkDataService testCandidate;

        [Test]
        public void Test_GetOSRoadLink()
        {
            var actualResult = testCandidate.GetOSRoadLink("123");
            Assert.IsNotNull(actualResult);
        }

        protected override void OnSetup()
        {
            List<OSRoadLink> oSRoadLink = new List<OSRoadLink>() { new OSRoadLink() { TOID = "123", RouteHierarchy = "1" } };
            var mockOSRoadLinkDataService = MockDbSet(oSRoadLink);

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<OSRoadLink>()).Returns(mockOSRoadLinkDataService.Object);
            mockRMDBContext.Setup(x => x.OSRoadLinks).Returns(mockOSRoadLinkDataService.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new OSRoadLinkDataService(mockDatabaseFactory.Object);
        }
    }
}