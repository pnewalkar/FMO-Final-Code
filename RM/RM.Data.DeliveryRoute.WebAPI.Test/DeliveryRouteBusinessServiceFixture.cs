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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private RouteDTO actualRouteDTO = null;
        private RouteDataDTO actualRouteDataDTO = null;
        private List<RouteDataDTO> actualRoutesResult = null;
        private List<RouteLogSequencedPointsDataDTO> actualRouteLogSequencedPointsDataDTO = null;
        private List<Common.ReferenceDataDTO> actualReferenceDataCategoryResult = null;
        private Mock<ILoggingHelper> mockLoggingHelper;

        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid unitLocationTypeId = new Guid("97FE320A-AFEE-4E68-980D-3A70F418E46D");
        private Guid operationalStateID = System.Guid.NewGuid();
        private Guid deliveryScenarioID = System.Guid.NewGuid();
        private Guid routeId = System.Guid.NewGuid();
        private Guid deliveryPointId = System.Guid.NewGuid();
        private string searchText = "road";
        private string postcodeUnit = "PO001";
        private RouteDTO deliveryRouteDto;
        private List<Common.ReferenceDataCategoryDTO> referenceDataCategoryDTOList;

        [Test]
        public async Task TestGetScenarioRoutes_PostiveScenario()
        {
            List<RouteDTO> expectedScenarioResult = await testCandidate.GetScenarioRoutes(deliveryScenarioID);
            Assert.NotNull(expectedScenarioResult);
            Assert.IsTrue(expectedScenarioResult.Count == 2);
        }

        [Test]
        public async Task TestGetScenarioRoutes_NegativeScenario()
        {
            try
            {
                deliveryScenarioID = Guid.Empty;
                List<RouteDTO> expectedScenarioResult = await testCandidate.GetScenarioRoutes(deliveryScenarioID);
            }
            catch (Exception e)
            {
                Assert.AreEqual("ArgumentNullException", e);
            }
            //  Assert.Throws<ExpectedException>(() =>  testCandidate.GetScenarioRoutes(deliveryScenarioID));
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
            List<RouteDTO> expectedRoutesForAdvancedSearch = await testCandidate.GetRoutesForAdvanceSearch(searchText, deliveryScenarioID);
            Assert.NotNull(expectedRoutesForAdvancedSearch);
            Assert.IsTrue(expectedRoutesForAdvancedSearch.Count == 2);
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
            List<RouteDTO> expectedRoutesForBasicSearch = await testCandidate.GetRoutesForBasicSearch(searchText, deliveryScenarioID);
            Assert.NotNull(expectedRoutesForBasicSearch);
            Assert.IsTrue(expectedRoutesForBasicSearch.Count == 2);
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
            int expectedRouteCount = await testCandidate.GetRouteCount(searchText, unitLocationTypeId);
            Assert.NotNull(expectedRouteCount);
            Assert.IsTrue(expectedRouteCount == 5);
        }

        [Test]
        public async Task TestGetRouteSummary_PostiveScenario()
        {
            RouteDTO expectedRouteSummaryResult = await testCandidate.GetRouteSummary(routeId);
            Assert.NotNull(expectedRouteSummaryResult);
            //Assert.IsTrue(expectedRouteSummaryResult == 5);
        }

        [Test]
        public async Task TestGetRouteSummary_NegativeScenario()
        {
            RouteDTO expectedRouteSummaryResult = await testCandidate.GetRouteSummary(routeId);
            Assert.NotNull(expectedRouteSummaryResult);
            //Assert.IsTrue(expectedRouteSummaryResult == 5);
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
            RouteLogSummaryDTO expectedRouteLogSummaryResult = await testCandidate.GenerateRouteLog(deliveryRouteDto);
            Assert.NotNull(expectedRouteLogSummaryResult);
            Assert.IsTrue(expectedRouteLogSummaryResult.RouteLogSequencedPoints.Count == 2);
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
            List<RouteDTO> expectedRouteResult = await testCandidate.GetPostcodeSpecificRoutes(postcodeUnit, unitLocationTypeId);
            Assert.NotNull(expectedRouteResult);
            Assert.IsTrue(expectedRouteResult.Count == 2);
        }

        [Test]
        public void TestSaveDeliveryPointRouteMapping_PositiveScenario()
        {
            testCandidate.SaveDeliveryPointRouteMapping(routeId, deliveryPointId);
            //Assert.NotNull(expectedRouteResult);
            //Assert.IsTrue(expectedRouteResult.Count == 2);
        }

        [Test]
        public void TestSaveDeliveryPointRouteMapping_NegativeScenario()
        {
            testCandidate.SaveDeliveryPointRouteMapping(routeId, deliveryPointId);
            //Assert.NotNull(expectedRouteResult);
            //Assert.IsTrue(expectedRouteResult.Count == 2);
        }

        [Test]
        public async Task TestGetRouteByDeliveryPoint_PositiveScenario()
        {
            RouteDTO expectedRouteResult = await testCandidate.GetRouteByDeliveryPoint(deliveryPointId);
            Assert.NotNull(expectedRouteResult);
        }

        //[Test]
        //public void Test_FetchDeliveryRoute()
        //{
        //    List<RouteDTO> expectedDeliveryRouteResult = testCandidate.FetchRoutes(operationalStateID, deliveryScenarioID, deliveryUnitID);
        //    Assert.NotNull(expectedDeliveryRouteResult);
        //    Assert.NotNull(actualDeliveryRouteResult);
        //    Assert.AreEqual(expectedDeliveryRouteResult, actualDeliveryRouteResult);
        //}

        //[Test]
        //public void Test_GetDeliveryRouteDetailsforPdfGeneration()
        //{
        //    var deliveryRouteGuid = System.Guid.Parse("B13D545D-2DE7-4E62-8DAD-00EC2B7FF8B8");
        //    var unitGuid = Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
        //    var expectedDeliveryRouteResult = testCandidate.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteGuid, unitGuid);
        //    Assert.NotNull(expectedDeliveryRouteResult);
        //}

        //[Test]
        //public void Test_FetchDeliveryRouteForBasicSearch()
        //{
        //    mockDeliveryRouteDataService.Setup(n => n.FetchDeliveryRouteForBasicSearch(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Unit")).ReturnsAsync(new List<RouteDTO>() { });
        //    var expectedDeliveryRouteResult = testCandidate.FetchDeliveryRouteForBasicSearch("abc", Guid.NewGuid());
        //    Assert.NotNull(expectedDeliveryRouteResult);
        //}

        //[Test]
        //public void Test_GetDeliveryRouteCount()
        //{
        //    mockDeliveryRouteDataService.Setup(n => n.GetDeliveryRouteCount(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Unit")).ReturnsAsync(5);
        //    var actualResult = testCandidate.GetDeliveryRouteCount("abc", Guid.NewGuid());
        //    Assert.NotNull(actualResult);
        //    Assert.AreEqual(actualResult.Result, 5);
        //}

        //[Test]
        //public void Test_FetchDeliveryRouteForAdvanceSearch()
        //{
        //    mockDeliveryRouteDataService.Setup(n => n.FetchDeliveryRouteForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>(), "Delivery Unit")).ReturnsAsync(new List<RouteDTO>() { });
        //    var actualResult = testCandidate.FetchDeliveryRouteForAdvanceSearch("abc", Guid.NewGuid());
        //    Assert.NotNull(actualResult);
        //}

        //[Test]
        //public void Test_GenerateRouteLog()
        //{
        //    mockDeliveryRouteIntegrationService.Setup(n => n.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Guid.NewGuid());
        //    mockDeliveryRouteDataService.Setup(n => n.GenerateRouteLog(It.IsAny<RouteDTO>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(new RouteLogSummaryModelDTO() { });
        //    var expectedDeliveryRouteResult = testCandidate.GenerateRouteLog(deliveryRouteDto, Guid.NewGuid());
        //    Assert.NotNull(actualDeliveryRouteResult);
        //}

        //[Test]
        //public void Test_CreateBlockSequenceForDeliveryPoint()
        //{
        //    mockDeliveryRouteIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).ReturnsAsync(referenceDataCategoryDTOList);
        //    mockBlockSequenceDataService.Setup(n => n.AddBlockSequence(It.IsAny<BlockSequenceDTO>(), It.IsAny<Guid>())).ReturnsAsync(true);
        //    var actualDeliveryRouteResult = testCandidate.CreateBlockSequenceForDeliveryPoint(Guid.NewGuid(), Guid.NewGuid());
        //    Assert.NotNull(actualDeliveryRouteResult);
        //    Assert.IsTrue(actualDeliveryRouteResult.Result);
        //}

        //[Test]
        //public void TestGenerateRouteLog()
        //{
        //    RouteDTO deliveryRouteDTO = new RouteDTO() { DeliveryRouteBarcode = "D0001234", ID = Guid.NewGuid(), DeliveryScenario_Id = 1, ExternalId = 1, OperationalStatus_Id = 1, RouteMethodType_Id = 1, RouteName = "RouteOneTwothree", RouteNumber = "R004341566" };
        //    var unitGuid = Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
        //    var expectedDeliveryRouteResult = testCandidate.GenerateRouteLog(deliveryRouteDTO, unitGuid);
        //    Assert.NotNull(expectedDeliveryRouteResult);
        //}
        //public static void Throws<T>(Action task, string expectedMessage, ExceptionMessageCompareOptions options) where T : Exception
        //{
        //    try
        //    {
        //        task();
        //    }
        //    catch (Exception ex)
        //    {
        //        AssertExceptionType<T>(ex);
        //        AssertExceptionMessage(ex, expectedMessage, options);
        //        return;
        //    }

        //    if (typeof(T).Equals(new Exception().GetType()))
        //    {
        //        Assert.Fail("Expected exception but no exception was thrown.");
        //    }
        //    else
        //    {
        //        Assert.Fail(string.Format("Expected exception of type {0} but no exception was thrown.", typeof(T)));
        //    }
        //}
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