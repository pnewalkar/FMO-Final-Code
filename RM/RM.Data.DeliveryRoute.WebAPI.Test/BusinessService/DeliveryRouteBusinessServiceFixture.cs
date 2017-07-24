using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.DeliveryRoute.WebAPI.BusinessService;
using RM.DataManagement.DeliveryRoute.WebAPI.BusinessService.Implementation;
using RM.DataManagement.DeliveryRoute.WebAPI.DataDTO;
using RM.DataManagement.DeliveryRoute.WebAPI.DataService;
using RM.DataManagement.DeliveryRoute.WebAPI.DTO;
using RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService;
using Common = RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Data.DeliveryRoute.WebAPI.Test
{
    [TestFixture]
    public class DeliveryRouteBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IDeliveryRouteDataService> mockDeliveryRouteDataService;
        private IDeliveryRouteBusinessService testCandidate;
        private Mock<IDeliveryRouteIntegrationService> mockDeliveryRouteIntegrationService;
        private Mock<IPostcodeDataService> mockPostCodedataService;
        private Mock<IBlockSequenceDataService> mockBlockSequenceDataService;
        private List<RouteDataDTO> actualDeliveryRouteResult = null;
        private int actualRoueCount = 0;

        // private RouteDTO actualRouteDTO = null;
        private RouteDataDTO actualRouteDataDTO = null;

        // private List<RouteDataDTO> actualRoutesResult = null;
        private List<RouteLogSequencedPointsDataDTO> actualRouteLogSequencedPointsDataDTO = null;

        private List<Common.ReferenceDataDTO> actualReferenceDataCategoryResult = null;
        private Mock<ILoggingHelper> mockLoggingHelper;

        private Guid deliveryUnitID = Guid.NewGuid();
        private Guid unitLocationTypeId = new Guid("97FE320A-AFEE-4E68-980D-3A70F418E46D");
        private Guid operationalStateID = Guid.NewGuid();
        private Guid deliveryScenarioID = Guid.NewGuid();
        private Guid routeId = Guid.NewGuid();
        private Guid deliveryPointId = Guid.NewGuid();
        private string searchText = "road";
        private string postcodeUnit = "PO001";
        private RouteDTO deliveryRouteDto;
        private List<Common.ReferenceDataCategoryDTO> referenceDataCategoryDTOList;

        [Test]
        public async Task TestGetScenarioRoutes_PostiveScenario()
        {
            List<RouteDTO> expectedScenarioResult = await testCandidate.GetScenarioRoutes(deliveryScenarioID);
            Assert.IsNotNull(expectedScenarioResult);
            Assert.IsTrue(expectedScenarioResult.Count == 2);
        }

        [Test]
        public async Task TestGetScenarioRoutes_NegativeScenario()
        {
            try
            {
                List<RouteDTO> expectedScenarioResult = await testCandidate.GetScenarioRoutes(Guid.Empty);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: scenarioID", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        [Test]
        public async Task TestGetRoutesForAdvanceSearch_PostiveScenario()
        {
            List<RouteDTO> expectedRoutesForAdvancedSearch = await testCandidate.GetRoutesForAdvanceSearch(searchText, deliveryScenarioID);
            Assert.NotNull(expectedRoutesForAdvancedSearch);
            Assert.IsTrue(expectedRoutesForAdvancedSearch.Count == 2);
        }

        [Test]
        public async Task TestGetRoutesForAdvanceSearch_NegativeScenario()
        {
            try
            {
                List<RouteDTO> expectedRoutesForAdvancedSearch = await testCandidate.GetRoutesForAdvanceSearch(string.Empty, Guid.Empty);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: searchText", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        [Test]
        public async Task TestGetRoutesForBasicSearch_PostiveScenario()
        {
            List<RouteDTO> expectedRoutesForBasicSearch = await testCandidate.GetRoutesForBasicSearch(searchText, deliveryScenarioID);
            Assert.NotNull(expectedRoutesForBasicSearch);
            Assert.IsTrue(expectedRoutesForBasicSearch.Count == 2);
        }

        [Test]
        public async Task TestGetRoutesForBasicSearch_NegativeScenario()
        {
            try
            {
                List<RouteDTO> expectedRoutesForBasicSearch = await testCandidate.GetRoutesForBasicSearch(string.Empty, Guid.Empty);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: searchText", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        [Test]
        public async Task TestGetRouteCount_PostiveScenario()
        {
            int expectedRouteCount = await testCandidate.GetRouteCount(searchText, unitLocationTypeId);
            Assert.NotNull(expectedRouteCount);
            Assert.IsTrue(expectedRouteCount == 5);
        }

        [Test]
        public async Task TestGetRouteCount_NegativeScenario()
        {
            try
            {
                int expectedRouteCount = await testCandidate.GetRouteCount(string.Empty, Guid.Empty);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: searchText", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        [Test]
        public async Task TestGetRouteSummary_PostiveScenario()
        {
            RouteDTO expectedRouteSummaryResult = await testCandidate.GetRouteSummary(routeId);
            Assert.NotNull(expectedRouteSummaryResult);
        }

        [Test]
        public async Task TestGetRouteSummary_NegativeScenario()
        {
            try
            {
                RouteDTO expectedRouteSummaryResult = await testCandidate.GetRouteSummary(Guid.Empty);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: routeId", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        [Test]
        public async Task TestGenerateRouteLog_PositiveScenario()
        {
            RouteLogSummaryDTO expectedRouteLogSummaryResult = await testCandidate.GenerateRouteLog(deliveryRouteDto);
            Assert.NotNull(expectedRouteLogSummaryResult);
            Assert.IsTrue(expectedRouteLogSummaryResult.RouteLogSequencedPoints.Count == 2);
        }

        [Test]
        public async Task TestGenerateRouteLog_NegativeScenario()
        {
            try
            {
                RouteLogSummaryDTO expectedRouteLogSummaryResult = await testCandidate.GenerateRouteLog(null);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: routeDetails", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        [Test]
        public async Task TestGetPostcodeSpecificRoutes_PositiveScenario()
        {
            List<RouteDTO> expectedRouteResult = await testCandidate.GetPostcodeSpecificRoutes(postcodeUnit, unitLocationTypeId);
            Assert.NotNull(expectedRouteResult);
            Assert.IsTrue(expectedRouteResult.Count == 2);
        }

        [Test]
        public async Task TestGetPostcodeSpecificRoutes_NegativeScenario()
        {
            try
            {
                List<RouteDTO> expectedRouteResult = await testCandidate.GetPostcodeSpecificRoutes(string.Empty, Guid.Empty);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: postcodeUnit", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        [Test]
        public async Task TestGetRouteByDeliveryPoint_PositiveScenario()
        {
            RouteDTO expectedRouteResult = await testCandidate.GetRouteByDeliveryPoint(deliveryPointId);
            Assert.NotNull(expectedRouteResult);
        }

        [Test]
        public async Task TestGetRouteByDeliveryPoint_NegativeScenario()
        {
            try
            {
                RouteDTO expectedRouteResult = await testCandidate.GetRouteByDeliveryPoint(Guid.Empty);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.Parameter name: deliveryPointId", ae.Message.Replace("\r\n", string.Empty));
            }
        }

        protected override void OnSetup()
        {
            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockDeliveryRouteIntegrationService = CreateMock<IDeliveryRouteIntegrationService>();
            mockBlockSequenceDataService = CreateMock<IBlockSequenceDataService>();
            mockPostCodedataService = CreateMock<IPostcodeDataService>();
            deliveryUnitID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            operationalStateID = System.Guid.Parse("9C1E56D7-5397-4984-9CF0-CD9EE7093C88");
            mockDeliveryRouteDataService = CreateMock<IDeliveryRouteDataService>();

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            referenceDataCategoryDTOList = new List<Common.ReferenceDataCategoryDTO>()
            {
                new Common.ReferenceDataCategoryDTO()
                {
                    CategoryName = ReferenceDataCategoryNames.OperationalObjectType,
                    ReferenceDatas = new List<Common.ReferenceDataDTO>()
                    {
                        new Common.ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.OperationalObjectTypeDP,
                            ID = Guid.Parse("4DBA7B39-D23E-493A-9B8F-B94D181A082F")
                        },
                         new Common.ReferenceDataDTO()
                        {
                            ReferenceDataName = null,
                            ReferenceDataValue = ReferenceDataValues.DeliveryUnit,
                            ID = unitLocationTypeId
                        }
                    }
                }
            };

            deliveryRouteDto = new RouteDTO() { };

            actualDeliveryRouteResult = new List<RouteDataDTO>()
            {
                new RouteDataDTO() { ID = Guid.NewGuid(), RouteName = "RouteOne", RouteNumber = "R004341" },
                 new RouteDataDTO() { ID = Guid.NewGuid(), RouteName = "RouteTwo", RouteNumber = "R003414" },
            };

            actualRouteDataDTO = new RouteDataDTO()
            {
                RouteName = "Route 001",
                RouteNumber = "R003456"
            };

            actualRouteLogSequencedPointsDataDTO = new List<RouteLogSequencedPointsDataDTO>()
            {
               new RouteLogSequencedPointsDataDTO() { StreetName ="Street001", BuildingNumber =001},
                new RouteLogSequencedPointsDataDTO() { StreetName ="Street001",BuildingNumber =002 },
            };

            actualRoueCount = new int();
            actualRoueCount = 5;

            actualReferenceDataCategoryResult = new List<Common.ReferenceDataDTO>() { new Common.ReferenceDataDTO() { DataDescription = "Live", DisplayText = "Live", ReferenceDataName = "Live" } };
            mockDeliveryRouteDataService.Setup(n => n.GetScenarioRoutes(It.IsAny<Guid>())).Returns(Task.FromResult(actualDeliveryRouteResult));
            mockDeliveryRouteDataService.Setup(n => n.GetRoutesForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(actualDeliveryRouteResult));
            mockDeliveryRouteDataService.Setup(n => n.GetRoutesForBasicSearch(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(actualDeliveryRouteResult));
            mockDeliveryRouteDataService.Setup(n => n.GetRouteCount(It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult(actualRoueCount));
            mockDeliveryRouteDataService.Setup(n => n.GetRouteSummary(It.IsAny<Guid>(), It.IsAny<List<Common.ReferenceDataCategoryDTO>>())).Returns(Task.FromResult(actualRouteDataDTO));
            mockDeliveryRouteDataService.Setup(n => n.GetSequencedRouteDetails(It.IsAny<Guid>())).Returns(Task.FromResult(actualRouteLogSequencedPointsDataDTO));
            mockDeliveryRouteDataService.Setup(n => n.GetRoutesByLocation(It.IsAny<Guid>())).Returns(Task.FromResult(actualDeliveryRouteResult));
            mockBlockSequenceDataService.Setup(n => n.SaveDeliveryPointRouteMapping(It.IsAny<Guid>(), It.IsAny<Guid>()));
            mockDeliveryRouteDataService.Setup(n => n.GetRouteByDeliverypoint(It.IsAny<Guid>())).Returns(Task.FromResult(actualRouteDataDTO));

            mockDeliveryRouteIntegrationService.Setup(n => n.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryDTOList));

            testCandidate = new DeliveryRouteBusinessService(mockDeliveryRouteDataService.Object, mockDeliveryRouteIntegrationService.Object, mockLoggingHelper.Object, mockBlockSequenceDataService.Object, mockPostCodedataService.Object);
        }
    }
}