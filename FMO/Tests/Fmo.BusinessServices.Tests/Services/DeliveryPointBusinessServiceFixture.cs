using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Moq;
using NUnit.Framework;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryPointsRepository> mockDeliveryPointsRepository;
        private IDeliveryPointBusinessService testCandidate;

        protected override void OnSetup()
        {
            mockDeliveryPointsRepository = CreateMock<IDeliveryPointsRepository>();
            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object);
        }
    }
}