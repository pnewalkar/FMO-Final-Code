using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Moq;
using NUnit.Framework;

namespace Fmo.BusinessServices.Tests.Services
{
    public class DeliveryRouteBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryRouteRepository> mockDeliveryRouteRepository;
        private Mock<IReferenceDataCategoryRepository> mockReferenceDataCategoryRepository;
        private Mock<IScenarioRepository> mockScenarioRepository;
        private Mock<IDeliveryUnitLocationRepository> mockIDeliveryUnitLocationRepository;
        private IDeliveryRouteBusinessService testCandidate;
        private int deliveryUnitID = 0;
        private int operationalStateID = 0;

        [Test]
        public void TestRouteLogStatus()
        {
            List<ReferenceDataDTO> actualResult = testCandidate.FetchRouteLogStatus();
            Assert.NotNull(actualResult);
            mockReferenceDataCategoryRepository.Verify(x => x.RouteLogStatus(), Times.Once());
        }

        [Test]
        public void TestFetchDeliveryScenario()
        {
            List<ScenarioDTO> actualResult = testCandidate.FetchDeliveryScenario(operationalStateID, deliveryUnitID);
            Assert.NotNull(actualResult);
            mockScenarioRepository.Verify(x => x.FetchScenario(operationalStateID, deliveryUnitID), Times.Once());
        }

        [Test]
        public void TestFetchDeliveryRoute()
        {
            List<DeliveryRouteDTO> actualResult = testCandidate.FetchDeliveryRoute(operationalStateID, deliveryUnitID);
            mockDeliveryRouteRepository.Verify(x => x.FetchDeliveryRoute(operationalStateID, deliveryUnitID), Times.Once());
            Assert.NotNull(actualResult);
        }

        [Test]
        public void TestFetchDeliveryUnit()
        {
            List<DeliveryUnitLocationDTO> actualResult = testCandidate.FetchDeliveryUnit();
            Assert.NotNull(actualResult);
            mockIDeliveryUnitLocationRepository.Verify(x => x.FetchDeliveryUnit(), Times.Once());
        }

        protected override void OnSetup()
        {
            mockDeliveryRouteRepository = CreateMock<IDeliveryRouteRepository>();
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockScenarioRepository = CreateMock<IScenarioRepository>();
            mockIDeliveryUnitLocationRepository = CreateMock<IDeliveryUnitLocationRepository>();
            deliveryUnitID = 1;
            operationalStateID = 1;
            testCandidate = new DeliveryRouteBusinessService(mockDeliveryRouteRepository.Object, mockReferenceDataCategoryRepository.Object, mockScenarioRepository.Object, mockIDeliveryUnitLocationRepository.Object);
        }
    }
}
