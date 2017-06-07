using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.DataManagement.DeliveryRoute.WebAPI.BusinessService;
using RM.DataManagement.DeliveryRoute.WebAPI.BusinessService.Implementation;
using RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService;

namespace RM.Data.DeliveryRoute.WebAPI.Test
{
    [TestFixture]
    public class DeliveryRouteBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryRouteDataService> mockDeliveryRouteRepository;
        private Mock<IScenarioDataService> mockScenarioRepository;    
        private IDeliveryRouteBusinessService testCandidate;
        private Mock<IDeliveryRouteIntegrationService> mockDeliveryRouteIntegrationService;
        private Mock<IBlockSequenceDataService> mockBlockSequenceDataService;
        private List<DeliveryRouteDTO> actualDeliveryRouteResult = null;
        private List<ReferenceDataDTO> actualReferenceDataCategoryResult = null;
        private List<ScenarioDTO> actualScenarioResult = null;

        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid operationalStateID = System.Guid.NewGuid();
        private Guid deliveryScenarioID = System.Guid.NewGuid();

        // [Test]
        // public void TestRouteLogStatus()
        // {
        //    List<ReferenceDataDTO> expectedReferenceDataResult = testCandidate.FetchRouteLogStatus();
        //    Assert.NotNull(expectedReferenceDataResult);
        //    Assert.NotNull(actualReferenceDataCategoryResult);
        //    Assert.AreEqual(expectedReferenceDataResult, actualReferenceDataCategoryResult);
        // }
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

        [Test]
        public void TestGetDeliveryRouteDetailsforPdfGeneration()
        {
            var deliveryRouteGuid = System.Guid.Parse("B13D545D-2DE7-4E62-8DAD-00EC2B7FF8B8");
            var unitGuid = Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            var expectedDeliveryRouteResult = testCandidate.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteGuid, unitGuid);
            Assert.NotNull(expectedDeliveryRouteResult);
        }

        protected override void OnSetup()
        {
            mockScenarioRepository = CreateMock<IScenarioDataService>();
            mockDeliveryRouteIntegrationService = CreateMock<IDeliveryRouteIntegrationService>();
            mockBlockSequenceDataService = CreateMock<IBlockSequenceDataService>();
            deliveryUnitID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            operationalStateID = System.Guid.Parse("9C1E56D7-5397-4984-9CF0-CD9EE7093C88");

            actualDeliveryRouteResult = new List<DeliveryRouteDTO>() { new DeliveryRouteDTO() { DeliveryRouteBarcode = "D0001", ID = Guid.NewGuid(), DeliveryScenario_Id = 1, ExternalId = 1, OperationalStatus_Id = 1, RouteMethodType_Id = 1, RouteName = "RouteOne", RouteNumber = "R004341" } };
            mockDeliveryRouteRepository = CreateMock<IDeliveryRouteDataService>();
            mockDeliveryRouteRepository.Setup(n => n.FetchDeliveryRoute(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(actualDeliveryRouteResult);

            actualReferenceDataCategoryResult = new List<ReferenceDataDTO>() { new ReferenceDataDTO() { DataDescription = "Live", DisplayText = "Live", ReferenceDataName = "Live" } };
          //  mockReferenceDataBusinessService = CreateMock<IReferenceDataBusinessService>();
           // mockReferenceDataBusinessService.Setup(n => n.FetchRouteLogStatus()).Returns(actualReferenceDataCategoryResult);

            actualScenarioResult = new List<ScenarioDTO>() { new ScenarioDTO() { ScenarioName = "ScenarioOne", DeliveryScenario_Id = 1, DeliveryUnit_Id = 1, OperationalState_Id = 1 } };
            mockScenarioRepository = CreateMock<IScenarioDataService>();
            mockScenarioRepository.Setup(n => n.FetchScenario(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(actualScenarioResult);

            mockDeliveryRouteRepository.Setup(n => n.GetDeliveryRouteDetailsforPdfGeneration(It.IsAny<Guid>(), It.IsAny<List<ReferenceDataCategoryDTO>>(), It.IsAny<Guid>())).Returns(Task.FromResult(new DeliveryRouteDTO() { }));

            testCandidate = new DeliveryRouteBusinessService(mockDeliveryRouteRepository.Object,mockScenarioRepository.Object,mockDeliveryRouteIntegrationService.Object,mockBlockSequenceDataService.Object);
        }
    }
}
