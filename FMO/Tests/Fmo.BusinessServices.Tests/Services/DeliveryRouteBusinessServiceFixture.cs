namespace Fmo.BusinessServices.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.BusinessServices.Services;
    using Fmo.Common.TestSupport;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Moq;
    using NUnit.Framework;

    public class DeliveryRouteBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryRouteRepository> mockDeliveryRouteRepository;
        private Mock<IReferenceDataCategoryRepository> mockReferenceDataCategoryRepository;
        private Mock<IScenarioRepository> mockScenarioRepository;
        private Mock<IReferenceDataBusinessService> mockReferenceDataBusinessService;
        private Mock<FMODBContext> mockFmoDbContext;

        private IDeliveryRouteBusinessService testCandidate;
        private List<DeliveryRouteDTO> actualDeliveryRouteResult = null;
        private List<ReferenceDataDTO> actualReferenceDataCategoryResult = null;
        private List<ScenarioDTO> actualScenarioResult = null;

        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid operationalStateID = System.Guid.NewGuid();
        private Guid deliveryScenarioID = System.Guid.NewGuid();

        //[Test]
        //public void TestRouteLogStatus()
        //{
        //    List<ReferenceDataDTO> expectedReferenceDataResult = testCandidate.FetchRouteLogStatus();
        //    Assert.NotNull(expectedReferenceDataResult);
        //    Assert.NotNull(actualReferenceDataCategoryResult);
        //    Assert.AreEqual(expectedReferenceDataResult, actualReferenceDataCategoryResult);
        //}

        [Test]
        public void TestFetchDeliveryScenario()
        {
            List<ScenarioDTO> expectedScenarioResult = testCandidate.FetchDeliveryScenario(operationalStateID, deliveryUnitID);
            Assert.NotNull(expectedScenarioResult);
            Assert.NotNull(actualScenarioResult);
            Assert.AreEqual(expectedScenarioResult, actualScenarioResult);
        }

        [Test]
        public void TestFetchDeliveryRoute()
        {
            List<DeliveryRouteDTO> expectedDeliveryRouteResult = testCandidate.FetchDeliveryRoute(operationalStateID, deliveryScenarioID, deliveryUnitID);
            Assert.NotNull(expectedDeliveryRouteResult);
            Assert.NotNull(actualDeliveryRouteResult);
            Assert.AreEqual(expectedDeliveryRouteResult, actualDeliveryRouteResult);
        }

        protected override void OnSetup()
        {
            deliveryUnitID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            operationalStateID = System.Guid.Parse("9C1E56D7-5397-4984-9CF0-CD9EE7093C88");

            actualDeliveryRouteResult = new List<DeliveryRouteDTO>() { new DeliveryRouteDTO() { DeliveryRouteBarcode = "D0001", ID = Guid.NewGuid(), DeliveryScenario_Id = 1, ExternalId = 1, OperationalStatus_Id = 1, RouteMethodType_Id = 1, RouteName = "RouteOne", RouteNumber = "R004341" } };
            mockDeliveryRouteRepository = CreateMock<IDeliveryRouteRepository>();
            mockDeliveryRouteRepository.Setup(n => n.FetchDeliveryRoute(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(actualDeliveryRouteResult);

            actualReferenceDataCategoryResult = new List<ReferenceDataDTO>() { new ReferenceDataDTO() { DataDescription = "Live", DisplayText = "Live", ReferenceDataName = "Live" } };
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockReferenceDataCategoryRepository.Setup(n => n.RouteLogStatus()).Returns(actualReferenceDataCategoryResult);

            actualScenarioResult = new List<ScenarioDTO>() { new ScenarioDTO() { ScenarioName = "ScenarioOne", DeliveryScenario_Id = 1, DeliveryUnit_Id = 1, OperationalState_Id = 1 } };
            mockScenarioRepository = CreateMock<IScenarioRepository>();
            mockScenarioRepository.Setup(n => n.FetchScenario(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(actualScenarioResult);

            mockReferenceDataBusinessService = CreateMock<IReferenceDataBusinessService>();

            mockFmoDbContext = CreateMock<FMODBContext>();

            testCandidate = new DeliveryRouteBusinessService(mockDeliveryRouteRepository.Object, mockReferenceDataCategoryRepository.Object, mockScenarioRepository.Object, mockReferenceDataBusinessService.Object);
        }
    }
}