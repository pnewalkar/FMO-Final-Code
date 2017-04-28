using System.Collections.Generic;
using Fmo.Common.Interface;
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
    public class SerarchDeliveryPointsRepositoryFixture : RepositoryFixtureBase
    {
        private Mock<FMODBContext> mockFmoDbContext;
        private Mock<IDatabaseFactory<FMODBContext>> mockDatabaseFactory;
        private IDeliveryPointsRepository testCandidate;
        private Mock<ILoggingHelper> mockLoggingHelper;

        protected override void OnSetup()
        {
            var deliveryPoint = new List<DeliveryPoint>()
            {
                new DeliveryPoint() { DeliveryPoint_Id = 1 },
                new DeliveryPoint() { DeliveryPoint_Id = 2 }
            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>()));

            var mockDeliveryPointDBSet = MockDbSet(deliveryPoint);

            mockFmoDbContext = CreateMock<FMODBContext>();
            mockFmoDbContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDBSet.Object);
            mockFmoDbContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<FMODBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockFmoDbContext.Object);

            testCandidate = new DeliveryPointsRepository(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}