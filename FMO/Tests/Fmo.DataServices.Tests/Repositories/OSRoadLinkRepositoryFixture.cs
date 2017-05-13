using System.Collections.Generic;
using Fmo.Common.TestSupport;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;
using Moq;
using NUnit.Framework;

namespace Fmo.DataServices.Tests.Repositories
{
    [TestFixture]
    public class OSRoadLinkRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IOSRoadLinkRepository testCandidate;

        [Test]
        public void Test_GetDeliveryPointRowVersion()
        {
            var actualResult = testCandidate.GetOSRoadLink("123");
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public void Test_GetDeliveryPointRowVersion_Null()
        {
            var actualResult = testCandidate.GetOSRoadLink(null);
            Assert.IsNotNull(actualResult);
        }

        protected override void OnSetup()
        {
            List<OSRoadLink> oSRoadLink = new List<OSRoadLink>() { new OSRoadLink() { TOID = "123", RouteHierarchy = "1" } };
            var mockOSRoadLinkRepository = MockDbSet(oSRoadLink);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<OSRoadLink>()).Returns(mockOSRoadLinkRepository.Object);
            mockFmoDbContext.Setup(x => x.OSRoadLinks).Returns(mockOSRoadLinkRepository.Object);
            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);
            testCandidate = new OSRoadLinkRepository(mockDatabaseFactory.Object);
        }
    }
}