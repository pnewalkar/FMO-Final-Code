﻿using System.Collections.Generic;
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
        private List<DeliveryRouteDTO> actualDeliveryRouteResult = null;
        private List<ReferenceDataDTO> actualReferenceDataCategoryResult = null;
        private List<ScenarioDTO> actualScenarioResult = null;
        private List<DeliveryUnitLocationDTO> actualDeliveryUnitResult = null;
        private int deliveryUnitID = 0;
        private int operationalStateID = 0;

        [Test]
        public void Test_RouteLogStatus()
        {
            List<ReferenceDataDTO> expectedReferenceDataResult = testCandidate.FetchRouteLogStatus();
            Assert.NotNull(expectedReferenceDataResult);
            Assert.NotNull(actualReferenceDataCategoryResult);
            Assert.AreEqual(expectedReferenceDataResult, actualReferenceDataCategoryResult);
        }

        [Test]
        public void Test_FetchDeliveryScenario()
        {
            List<ScenarioDTO> expectedScenarioResult = testCandidate.FetchDeliveryScenario(operationalStateID, deliveryUnitID);
            Assert.NotNull(expectedScenarioResult);
            Assert.NotNull(actualScenarioResult);
            Assert.AreEqual(expectedScenarioResult, actualScenarioResult);
        }

        [Test]
        public void Test_FetchDeliveryRoute()
        {
            List<DeliveryRouteDTO> expectedDeliveryRouteResult = testCandidate.FetchDeliveryRoute(operationalStateID, deliveryUnitID);
            Assert.NotNull(expectedDeliveryRouteResult);
            Assert.NotNull(actualDeliveryRouteResult);
            Assert.AreEqual(expectedDeliveryRouteResult, actualDeliveryRouteResult);
        }

        [Test]
        public void Test_FetchDeliveryUnit()
        {
            List<DeliveryUnitLocationDTO> expectedDeliveryUnitResult = testCandidate.FetchDeliveryUnit();
            Assert.NotNull(expectedDeliveryUnitResult);
            Assert.NotNull(actualDeliveryUnitResult);
            Assert.AreEqual(expectedDeliveryUnitResult, actualDeliveryUnitResult);
        }

        protected override void OnSetup()
        {
            deliveryUnitID = 1;
            operationalStateID = 1;

            actualDeliveryRouteResult = new List<DeliveryRouteDTO>() { new DeliveryRouteDTO() { DeliveryRouteBarcode = "D0001", DeliveryRoute_Id = 1, DeliveryScenario_Id = 1, ExternalId = 1, OperationalStatus_Id = 1, RouteMethodType_Id = 1, RouteName = "RouteOne", RouteNumber = "R004341" } };
            mockDeliveryRouteRepository = CreateMock<IDeliveryRouteRepository>();
            mockDeliveryRouteRepository.Setup(n => n.FetchDeliveryRoute(It.IsAny<int>(), It.IsAny<int>())).Returns(actualDeliveryRouteResult);

            actualReferenceDataCategoryResult = new List<ReferenceDataDTO>() { new ReferenceDataDTO() { DataDescription = "Live", DisplayText = "Live", ReferenceDataName = "Live" } };
            mockReferenceDataCategoryRepository = CreateMock<IReferenceDataCategoryRepository>();
            mockReferenceDataCategoryRepository.Setup(n => n.RouteLogStatus()).Returns(actualReferenceDataCategoryResult);

            actualScenarioResult = new List<ScenarioDTO>() { new ScenarioDTO() { ScenarioName = "ScenarioOne", DeliveryScenario_Id = 1, DeliveryUnit_Id = 1, OperationalState_Id = 1, ScenarioUnitType_Id = 1 } };
            mockScenarioRepository = CreateMock<IScenarioRepository>();
            mockScenarioRepository.Setup(n => n.FetchScenario(It.IsAny<int>(), It.IsAny<int>())).Returns(actualScenarioResult);

            actualDeliveryUnitResult = new List<DeliveryUnitLocationDTO>() { new DeliveryUnitLocationDTO() { DeliveryUnit_Id = 1, ExternalId = "DI0001", UnitAddressUDPRN = 1, UnitName = "UnitOne" } };
            mockIDeliveryUnitLocationRepository = CreateMock<IDeliveryUnitLocationRepository>();
            mockIDeliveryUnitLocationRepository.Setup(n => n.FetchDeliveryUnit()).Returns(actualDeliveryUnitResult);

            testCandidate = new DeliveryRouteBusinessService(mockDeliveryRouteRepository.Object, mockReferenceDataCategoryRepository.Object, mockScenarioRepository.Object, mockIDeliveryUnitLocationRepository.Object);
        }
    }
}
