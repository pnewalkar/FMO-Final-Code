using System.Collections.Generic;
using Fmo.Common.TestSupport;
using Fmo.Entities;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DataServices.DBContext;
using Moq;
using NUnit.Framework;

namespace Fmo.DataServices.Tests.Repositories
{
    [TestFixture]
    public class SerarchDeliveryPointsRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private ISearchDeliveryPointsRepository testCandidate;

        protected override void OnSetup()
        {
            var deliveryPoint = new List<DeliveryPoint>()
            {
                new DeliveryPoint(){ DeliveryPoint_Id = 1 },
                new DeliveryPoint(){ DeliveryPoint_Id = 2 }
            };

            var mockDeliveryPointDBSet = MockDbSet(deliveryPoint);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            testCandidate = new SearchDeliveryPointsRepository(mockDatabaseFactory.Object);
        }

        [Test]
        public void Test_SearchDeliveryPoints()
        {
            var actualResult = testCandidate.SearchDelievryPoints();
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 2);
        }
    }
}