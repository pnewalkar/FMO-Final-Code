﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.DeliveryRoute.WebAPI.BusinessService;
using RM.DataManagement.DeliveryRoute.WebAPI.BusinessService.Implementation;
using RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService;

namespace RM.Data.DeliveryRoute.WebAPI.Test
{
    [TestFixture]
    public class DeliveryRouteBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryRouteDataService> mockDeliveryRouteDataService;
        private Mock<IScenarioDataService> mockScenarioDataService;
        private IDeliveryRouteBusinessService testCandidate;
        private Mock<IDeliveryRouteIntegrationService> mockDeliveryRouteIntegrationService;
        private Mock<IBlockSequenceDataService> mockBlockSequenceDataService;
        private List<RouteDTO> actualDeliveryRouteResult = null;
        private List<ReferenceDataDTO> actualReferenceDataCategoryResult = null;
        private List<ScenarioDTO> actualScenarioResult = null;
        private Mock<ILoggingHelper> mockLoggingHelper;

        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid unitLocationTypeId = new Guid("97FE320A-AFEE-4E68-980D-3A70F418E46D");
        private Guid operationalStateID = System.Guid.NewGuid();
        private Guid deliveryScenarioID = System.Guid.NewGuid();
        private RouteDTO deliveryRouteDto;
        private List<ReferenceDataCategoryDTO> referenceDataCategoryDTOList;

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
            List<RouteDTO> expectedDeliveryRouteResult = testCandidate.FetchRoutes(operationalStateID, deliveryScenarioID, deliveryUnitID);
            Assert.NotNull(expectedDeliveryRouteResult);
            Assert.NotNull(actualDeliveryRouteResult);
            Assert.AreEqual(expectedDeliveryRouteResult, actualDeliveryRouteResult);
        }

        [Test]
        public void Test_GetDeliveryRouteDetailsforPdfGeneration()
        {
            var deliveryRouteGuid = System.Guid.Parse("B13D545D-2DE7-4E62-8DAD-00EC2B7FF8B8");
            var unitGuid = Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            var expectedDeliveryRouteResult = testCandidate.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteGuid, unitGuid);
            Assert.NotNull(expectedDeliveryRouteResult);
        }

        [Test]
        public void Test_FetchDeliveryRouteForBasicSearch()
        {
            mockDeliveryRouteDataService.Setup(n => n.FetchDeliveryRouteForBasicSearch(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Unit")).ReturnsAsync(new List<RouteDTO>() { });
            var expectedDeliveryRouteResult = testCandidate.FetchDeliveryRouteForBasicSearch("abc", Guid.NewGuid());
            Assert.NotNull(expectedDeliveryRouteResult);
        }

        [Test]
        public void Test_GetDeliveryRouteCount()
        {
            mockDeliveryRouteDataService.Setup(n => n.GetDeliveryRouteCount(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Unit")).ReturnsAsync(5);
            var actualResult = testCandidate.GetDeliveryRouteCount("abc", Guid.NewGuid());
            Assert.NotNull(actualResult);
            Assert.AreEqual(actualResult.Result, 5);
        }

        [Test]
        public void Test_FetchDeliveryRouteForAdvanceSearch()
        {
            mockDeliveryRouteDataService.Setup(n => n.FetchDeliveryRouteForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Unit")).ReturnsAsync(new List<RouteDTO>() { });
            var actualResult = testCandidate.FetchDeliveryRouteForAdvanceSearch("abc", Guid.NewGuid());
            Assert.NotNull(actualResult);
        }

        [Test]
        public void Test_GenerateRouteLog()
        {
            mockDeliveryRouteIntegrationService.Setup(n => n.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Guid.NewGuid());
            mockDeliveryRouteDataService.Setup(n => n.GenerateRouteLog(It.IsAny<RouteDTO>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new RouteLogSummaryModelDTO() { });
            var expectedDeliveryRouteResult = testCandidate.GenerateRouteLog(deliveryRouteDto, Guid.NewGuid());
            Assert.NotNull(actualDeliveryRouteResult);
        }

        [Test]
        public void Test_CreateBlockSequenceForDeliveryPoint()
        {
            mockDeliveryRouteIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).ReturnsAsync(referenceDataCategoryDTOList);
            mockBlockSequenceDataService.Setup(n => n.AddBlockSequence(It.IsAny<BlockSequenceDTO>(), It.IsAny<Guid>())).ReturnsAsync(true);
            var actualDeliveryRouteResult = testCandidate.CreateBlockSequenceForDeliveryPoint(Guid.NewGuid(), Guid.NewGuid());
            Assert.NotNull(actualDeliveryRouteResult);
            Assert.IsTrue(actualDeliveryRouteResult.Result);
        }

        [Test]
        public void TestGenerateRouteLog()
        {
            RouteDTO deliveryRouteDTO = new RouteDTO() { DeliveryRouteBarcode = "D0001234", ID = Guid.NewGuid(), DeliveryScenario_Id = 1, ExternalId = 1, OperationalStatus_Id = 1, RouteMethodType_Id = 1, RouteName = "RouteOneTwothree", RouteNumber = "R004341566" };
            var unitGuid = Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            var expectedDeliveryRouteResult = testCandidate.GenerateRouteLog(deliveryRouteDTO, unitGuid);
            Assert.NotNull(expectedDeliveryRouteResult);
        }

        protected override void OnSetup()
        {
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockScenarioDataService = CreateMock<IScenarioDataService>();
            mockDeliveryRouteIntegrationService = CreateMock<IDeliveryRouteIntegrationService>();
            mockBlockSequenceDataService = CreateMock<IBlockSequenceDataService>();
            deliveryUnitID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            operationalStateID = System.Guid.Parse("9C1E56D7-5397-4984-9CF0-CD9EE7093C88");
            mockScenarioDataService = CreateMock<IScenarioDataService>();
            mockDeliveryRouteDataService = CreateMock<IDeliveryRouteDataService>();

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            referenceDataCategoryDTOList = new List<ReferenceDataCategoryDTO>()
            {
                new ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.OperationalObjectType,
                    ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.OperationalObjectTypeDP,
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },
                         new ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.DeliveryUnit,
                            ID = unitLocationTypeId
                        }
                    }
                }
            };

            deliveryRouteDto = new RouteDTO() { };

            actualDeliveryRouteResult = new List<RouteDTO>() { new RouteDTO() { DeliveryRouteBarcode = "D0001", ID = Guid.NewGuid(), DeliveryScenario_Id = 1, ExternalId = 1, OperationalStatus_Id = 1, RouteMethodType_Id = 1, RouteName = "RouteOne", RouteNumber = "R004341" } };
            actualReferenceDataCategoryResult = new List<ReferenceDataDTO>() { new ReferenceDataDTO() { DataDescription = "Live", DisplayText = "Live", ReferenceDataName = "Live" } };
            actualScenarioResult = new List<ScenarioDTO>() { new ScenarioDTO() { ScenarioName = "ScenarioOne", DeliveryScenario_Id = 1, DeliveryUnit_Id = 1, OperationalState_Id = 1 } };

            mockDeliveryRouteDataService.Setup(n => n.FetchRoutes(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>())).Returns(actualDeliveryRouteResult);
            mockDeliveryRouteDataService.Setup(n => n.GetDeliveryRouteDetailsforPdfGeneration(It.IsAny<Guid>(), It.IsAny<List<ReferenceDataCategoryDTO>>(), It.IsAny<Guid>())).Returns(Task.FromResult(new RouteDTO() { }));
            mockDeliveryRouteDataService.Setup(n => n.GetDeliveryRouteDetailsforPdfGeneration(It.IsAny<Guid>(), It.IsAny<List<ReferenceDataCategoryDTO>>(), It.IsAny<Guid>())).ReturnsAsync(new RouteDTO() { });

            mockScenarioDataService.Setup(n => n.FetchScenario(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(actualScenarioResult);

            mockDeliveryRouteIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryDTOList));
            mockDeliveryRouteIntegrationService.Setup(n => n.GetUnitLocationTypeId(It.IsAny<Guid>())).Returns(Task.FromResult(unitLocationTypeId));

            testCandidate = new DeliveryRouteBusinessService(mockDeliveryRouteDataService.Object, mockScenarioDataService.Object, mockDeliveryRouteIntegrationService.Object, mockBlockSequenceDataService.Object, mockLoggingHelper.Object);
        }
    }
}