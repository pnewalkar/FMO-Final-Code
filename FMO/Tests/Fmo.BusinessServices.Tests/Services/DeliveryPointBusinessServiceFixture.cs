using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interface;
using Moq;
using NUnit.Framework;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class DeliveryPointBusinessServiceFixture : TestFixtureBase
    {
        private Mock<ISearchDeliveryPointsRepository> mockDeliveryPointsRepository;
        private IDeliveryPointBussinessService testCandidate;

        protected override void OnSetup()
        {
            mockDeliveryPointsRepository = CreateMock<ISearchDeliveryPointsRepository>();
            testCandidate = new DeliveryPointBusinessService(mockDeliveryPointsRepository.Object);
        }

        [Test]
        public void Test_SearchDeliveryPoints()
        {
            testCandidate.SearchDelievryPoints();
            mockDeliveryPointsRepository.Verify(x => x.SearchDelievryPoints(), Times.Once());
        }
    }
}