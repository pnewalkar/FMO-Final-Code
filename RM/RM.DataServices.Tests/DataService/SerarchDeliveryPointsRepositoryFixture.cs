using System;
using System.Collections.Generic;
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
    public class SerarchDeliveryPointsDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IDeliveryPointsDataService testCandidate;
        private Mock<ILoggingHelper> mockLoggingHelper;

        protected override void OnSetup()
        {
            var deliveryPoint = new List<DeliveryPoint>()
            {
                new DeliveryPoint() { ID = Guid.NewGuid() },
                new DeliveryPoint() { ID = Guid.NewGuid() }
            };

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            // mockLoggingHelper.Setup(n => n.LogInfo(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            var mockDeliveryPointDBSet = MockDbSet(deliveryPoint);

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDBSet.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);

            testCandidate = new DeliveryPointsDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}