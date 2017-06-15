using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.DataServices.Tests.DataService
{
    [TestFixture]
    public class DeliveryRouteDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RMDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<RMDBContext>> mockDatabaseFactory;
        private IDeliveryRouteDataService testCandidate;
        private Mock<IReferenceDataCategoryDataService> mockReferenceDataCategoryDataService;
        private Guid deliveryUnitID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44");
        private Guid deliveryScenarioID = new Guid("D771E341-8FE8-4980-BE96-A339AE014B4E");
        private Guid operationalStateID = new Guid("1FC7DAB1-D4D7-45BD-BA2E-E8AE6737E1EB");
        private Guid invalidId = new Guid("4A2DE1A4-BC19-4DB8-A8D9-DAA828E1B526");
        private Guid refDataId = new Guid("4A2DE1A4-BC19-4DB8-A8D9-DAA828E1B526");
        private Guid scenarioId = new Guid("4A2DE1A4-BC19-4DB8-A8D9-DAA828E1B527");
        private Guid deliveryRouteId = new Guid("B13D545D-2DE7-4E62-8DAD-00EC2B7FF8B8");
        private Guid operationalObjectTypeForDP = new Guid("9F82733D-C72C-4111-815D-8813790B5CFB");
        private Guid operationalObjectTypeForDPCommercial = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED");
        private Guid operationalObjectTypeForDPResidential = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED");

        private List<ReferenceDataCategoryDTO> referenceDataCategoryDtoList = new List<ReferenceDataCategoryDTO>()
        {
            new ReferenceDataCategoryDTO()
            {
                CategoryName = "DeliveryPoint Use Indicator", CategoryType = 2, Maintainable = false, Id = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED"),
                ReferenceDatas = new List<ReferenceDataDTO>()
                {
                    new ReferenceDataDTO() { DataDescription = "Commercial", ReferenceDataValue = "Commercial", ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"), ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") },
                    new ReferenceDataDTO() { DataDescription = "Residential", ReferenceDataValue = "Residential", ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED")  }
                }
            },
             new ReferenceDataCategoryDTO()
            {
                CategoryName = "Delivery Route Method Type", CategoryType = 2, Maintainable = false, Id = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed"),
                ReferenceDatas = new List<ReferenceDataDTO>()
                {
                    new ReferenceDataDTO() { DataDescription = "High Capacity Trolle", ReferenceDataValue = "High Capacity Trolle", ID = new Guid("c168f46e-561b-e711-9f8c-28d244aef9ed"), ReferenceDataCategory_GUID = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed")},
                    new ReferenceDataDTO() { DataDescription = "RM Van (Shared)", ReferenceDataValue = "RM Van (Shared)", ID = new Guid("e1d25b7f-561b-e711-9f8c-28d244aef9ed"), ReferenceDataCategory_GUID = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed") },
                    new ReferenceDataDTO() { DataDescription = "RM Van", ReferenceDataValue = "RM Van", ID = new Guid("c5b94b88-561b-e711-9f8c-28d244aef9ed"), ReferenceDataCategory_GUID = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed")},
                    new ReferenceDataDTO() { DataDescription = "PO BOX", ReferenceDataValue = "PO BOX", ID = new Guid("492f4394-561b-e711-9f8c-28d244aef9ed"), ReferenceDataCategory_GUID = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed") }
                }
            },
              new ReferenceDataCategoryDTO()
            {
                CategoryName = "Operational Object Type", CategoryType = 2, Maintainable = false, Id = new Guid("bbc205a9-97c4-4345-ae8f-c485d243ecfc"),
                ReferenceDatas = new List<ReferenceDataDTO>()
                {
                    new ReferenceDataDTO() { DataDescription = "Delivery point", ReferenceDataValue = "DP", ID = new Guid("9f82733d-c72c-4111-815d-8813790b5cfb"), ReferenceDataCategory_GUID = new Guid("bbc205a9-97c4-4345-ae8f-c485d243ecfc")},
                    new ReferenceDataDTO() { DataDescription = "RMG Delivery point", ReferenceDataValue = "RMG DP", ID = new Guid("e0b3dbc4-c2e3-40f7-9df0-eb13c6da0cb0"), ReferenceDataCategory_GUID = new Guid("bbc205a9-97c4-4345-ae8f-c485d243ecfc") }
                }
            }
        };

        [Test]
        public void TestFetchDeliveryRoute()
        {
            var actualResult = testCandidate.FetchDeliveryRoute(operationalStateID, deliveryScenarioID, deliveryUnitID);
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public async Task TestFetchDeliveryRouteForBasicSearchValid()
        {
            var actualResult = await testCandidate.FetchDeliveryRouteForBasicSearch("test", deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestFetchDeliveryRouteForBasicSearchInvalid()
        {
            var actualResult = await testCandidate.FetchDeliveryRouteForBasicSearch("invalid_testsearch", invalidId);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestFetchDeliveryRouteForBasicSearchNull()
        {
            var actualResult = await testCandidate.FetchDeliveryRouteForBasicSearch(null, deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 5);
        }

        [Test]
        public async Task TestGetDeliveryRouteCountValid()
        {
            var actualResultCount = await testCandidate.GetDeliveryRouteCount("testsearch", deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public async Task TestGetDeliveryRouteCountInvalid()
        {
            var actualResultCount = await testCandidate.GetDeliveryRouteCount("invalid_testsearch", invalidId);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 0);
        }

        [Test]
        public async Task TestGetDeliveryRouteCountNull()
        {
            var actualResultCount = await testCandidate.GetDeliveryRouteCount(null, deliveryUnitID);
            Assert.IsNotNull(actualResultCount);
            Assert.IsTrue(actualResultCount == 7);
        }

        [Test]
        public async Task TestGetDeliveryRouteDetailsforPdfGenerationValid()
        {
            SetUpNew();
            var actualResult = await testCandidate.GetDeliveryRouteDetailsforPdfGeneration(deliveryRouteId, referenceDataCategoryDtoList, deliveryUnitID);
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Totaltime == "0:05 mins");
            Assert.IsTrue(actualResult.Aliases == 0);
            Assert.IsTrue(actualResult.Blocks == 1);
            Assert.IsTrue(actualResult.DPs == 0);
            Assert.IsTrue(actualResult.BusinessDPs == 0);
            Assert.IsTrue(actualResult.ResidentialDPs == 0);
            Assert.IsEmpty(actualResult.PairedRoute);
        }

        [Test]
        public async Task TestGetDeliveryRouteDetailsforPdfGenerationInValid()
        {
            SetUpNew();
            var actualResult = await testCandidate.GetDeliveryRouteDetailsforPdfGeneration(Guid.NewGuid(), referenceDataCategoryDtoList, Guid.NewGuid());
            Assert.IsNotNull(actualResult);
            Assert.IsNull(actualResult.Totaltime);
            Assert.IsTrue(actualResult.Aliases == 0);
            Assert.IsTrue(actualResult.Blocks == 0);
            Assert.IsTrue(actualResult.DPs == 0);
            Assert.IsTrue(actualResult.BusinessDPs == 0);
            Assert.IsTrue(actualResult.ResidentialDPs == 0);
            Assert.IsEmpty(actualResult.PairedRoute);
        }

        protected override void OnSetup()
        {
            var deliveryRoute = new List<DeliveryRoute>()
            {
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch1jbcjkdsghfjks", RouteNumber = "testsearch1jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch2jbcjkdsghfjks", RouteNumber = "testsearch2jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch3jbcjkdsghfjks", RouteNumber = "testsearch3jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch4jbcjkdsghfjks", RouteNumber = "testsearch4jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch5jbcjkdsghfjks", RouteNumber = "testsearch5jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch6jbcjkdsghfjks", RouteNumber = "testsearch6jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } },
                new DeliveryRoute() { ID = Guid.NewGuid(), OperationalStatus_GUID = Guid.NewGuid(), DeliveryScenario_GUID = Guid.NewGuid(), RouteName = "testsearch7jbcjkdsghfjks", RouteNumber = "testsearch7jbcjkdsghfjks", Scenario = new Scenario() { Unit_GUID = new Guid("7654810D-3EBF-420A-91FD-DABE05945A44") } }
            };

            var mockAsynEnumerable = new DbAsyncEnumerable<DeliveryRoute>(deliveryRoute);

            var mockDeliveryRouteDBSet = MockDbSet(deliveryRoute);
            mockReferenceDataCategoryDataService = CreateMock<IReferenceDataCategoryDataService>();
            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable.AsQueryable().Provider);
            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable.AsQueryable().Expression);
            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable.AsQueryable().ElementType);
            mockDeliveryRouteDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryRoute>)mockAsynEnumerable).GetAsyncEnumerator());

            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<DeliveryRoute>()).Returns(mockDeliveryRouteDBSet.Object);
            mockRMDBContext.Setup(x => x.DeliveryRoutes).Returns(mockDeliveryRouteDBSet.Object);

            mockRMDBContext.Setup(c => c.DeliveryRoutes.AsNoTracking()).Returns(mockDeliveryRouteDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new DeliveryRouteDataService(mockDatabaseFactory.Object);
        }

        protected void SetUp()
        {
            var scenario = new List<Scenario>() { new Scenario() { ScenarioName = "ScanarioName", ID = scenarioId, Unit_GUID = deliveryUnitID } };
            var referenceData = new List<ReferenceData>() { new ReferenceData() { DisplayText = "shared van", ID = refDataId } };
            var deliveryRoute = new List<DeliveryRoute>()
            { new DeliveryRoute()
                {
                    ID=deliveryRouteId,
                    RouteName = "Linkroad",
                    RouteNumber = "1",
                    Scenario = scenario[0],
                    ReferenceData = referenceData[0],
                    RouteMethodType_GUID = refDataId,
                    DeliveryScenario_GUID = scenarioId
                }
            };

            var mockDeliveryRouteDBSet = MockDbSet(deliveryRoute);
            mockReferenceDataCategoryDataService = CreateMock<IReferenceDataCategoryDataService>();
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<DeliveryRoute>()).Returns(mockDeliveryRouteDBSet.Object);
            mockRMDBContext.Setup(x => x.DeliveryRoutes).Returns(mockDeliveryRouteDBSet.Object);
            mockRMDBContext.Setup(c => c.DeliveryRoutes.AsNoTracking()).Returns(mockDeliveryRouteDBSet.Object);

            var mockReferenceDataDBSet = MockDbSet(referenceData);
            mockRMDBContext.Setup(x => x.Set<ReferenceData>()).Returns(mockReferenceDataDBSet.Object);
            mockRMDBContext.Setup(x => x.ReferenceDatas).Returns(mockReferenceDataDBSet.Object);
            mockRMDBContext.Setup(c => c.ReferenceDatas.AsNoTracking()).Returns(mockReferenceDataDBSet.Object);

            var mockScenarioDBSet = MockDbSet(scenario);
            mockRMDBContext.Setup(x => x.Set<Scenario>()).Returns(mockScenarioDBSet.Object);
            mockRMDBContext.Setup(x => x.Scenarios).Returns(mockScenarioDBSet.Object);
            mockRMDBContext.Setup(c => c.Scenarios.AsNoTracking()).Returns(mockScenarioDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new DeliveryRouteDataService(mockDatabaseFactory.Object);
        }

        protected void SetUpNew()
        {
            var scenario = new List<Scenario>() { new Scenario() { ScenarioName = "ScanarioName", ID = scenarioId, Unit_GUID = deliveryUnitID } };
            var referenceData = new List<ReferenceData>() { new ReferenceData() { DisplayText = "shared van", ID = refDataId } };
            var deliveryrouteblocks = new List<DeliveryRouteBlock>() { new DeliveryRouteBlock() { DeliveryRoute_GUID = deliveryRouteId } };
            var blocks = new List<Block>() { new Block() { ID = deliveryRouteId } };
            var blocksequence = new List<BlockSequence>() { new BlockSequence() { Block_GUID = deliveryRouteId, OperationalObject_GUID = Guid.NewGuid() } };
            var deliverypoint = new List<DeliveryPoint>() { new DeliveryPoint() { ID = Guid.NewGuid() } };
            var deliverypointalias = new List<DeliveryPointAlias>() { new DeliveryPointAlias() { DeliveryPoint_GUID = Guid.NewGuid() } };
            var postaladdress = new List<PostalAddress>() { new PostalAddress() { ID = Guid.NewGuid() } };
            var postcode = new List<Postcode>() { new Postcode() { ID = Guid.NewGuid() } };

            var deliveryRoute = new List<DeliveryRoute>()
            {
                new DeliveryRoute()
                {
                    ID = deliveryRouteId,
                    RouteName = "Linkroad",
                    RouteNumber = "1",
                    Scenario = scenario[0],
                    ReferenceData = referenceData[0],
                    RouteMethodType_GUID = refDataId,
                    DeliveryScenario_GUID = scenarioId,
                    TravelInTimeMin = 10,
                    TravelOutTimeMin = 15,
                    TravelInTransportType_GUID = Guid.NewGuid(),
                    TravelOutTransportType_GUID = Guid.NewGuid()
                }
            };

            var mockAsynEnumerable1 = new DbAsyncEnumerable<DeliveryRoute>(deliveryRoute);
            var mockDeliveryRouteDBSet = MockDbSet(deliveryRoute);
            mockReferenceDataCategoryDataService = CreateMock<IReferenceDataCategoryDataService>();

            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockDeliveryRouteDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockDeliveryRouteDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryRoute>)mockAsynEnumerable1).GetAsyncEnumerator());
            mockRMDBContext = CreateMock<RMDBContext>();
            mockRMDBContext.Setup(x => x.Set<DeliveryRoute>()).Returns(mockDeliveryRouteDBSet.Object);
            mockRMDBContext.Setup(x => x.DeliveryRoutes).Returns(mockDeliveryRouteDBSet.Object);
            mockRMDBContext.Setup(c => c.DeliveryRoutes.AsNoTracking()).Returns(mockDeliveryRouteDBSet.Object);

            var mockAsynEnumerable2 = new DbAsyncEnumerable<ReferenceData>(referenceData);
            var mockReferenceDataDBSet = MockDbSet(referenceData);
            mockReferenceDataDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockReferenceDataDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockReferenceDataDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockReferenceDataDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<ReferenceData>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<ReferenceData>()).Returns(mockReferenceDataDBSet.Object);
            mockRMDBContext.Setup(x => x.ReferenceDatas).Returns(mockReferenceDataDBSet.Object);

            var mockAsynEnumerable3 = new DbAsyncEnumerable<Scenario>(scenario);
            var mockScenarioDBSet = MockDbSet(scenario);
            mockScenarioDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockScenarioDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockScenarioDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockScenarioDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Scenario>)mockAsynEnumerable3).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<Scenario>()).Returns(mockScenarioDBSet.Object);
            mockRMDBContext.Setup(x => x.Scenarios).Returns(mockScenarioDBSet.Object);
            mockRMDBContext.Setup(c => c.Scenarios.AsNoTracking()).Returns(mockScenarioDBSet.Object);

            var mockAsynEnumerable4 = new DbAsyncEnumerable<DeliveryRouteBlock>(deliveryrouteblocks);
            var mockDeliveryRouteBlockDBSet = MockDbSet(deliveryrouteblocks);
            mockDeliveryRouteBlockDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable4.AsQueryable().Provider);
            mockDeliveryRouteBlockDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable4.AsQueryable().Expression);
            mockDeliveryRouteBlockDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable4.AsQueryable().ElementType);
            mockDeliveryRouteBlockDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryRouteBlock>)mockAsynEnumerable4).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<DeliveryRouteBlock>()).Returns(mockDeliveryRouteBlockDBSet.Object);
            mockRMDBContext.Setup(x => x.DeliveryRouteBlocks).Returns(mockDeliveryRouteBlockDBSet.Object);
            mockRMDBContext.Setup(c => c.DeliveryRouteBlocks.AsNoTracking()).Returns(mockDeliveryRouteBlockDBSet.Object);

            var mockAsynEnumerable5 = new DbAsyncEnumerable<Block>(blocks);
            var mockBlockDBSet = MockDbSet(blocks);
            mockBlockDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable5.AsQueryable().Provider);
            mockBlockDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable5.AsQueryable().Expression);
            mockBlockDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable5.AsQueryable().ElementType);
            mockBlockDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Block>)mockAsynEnumerable5).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<Block>()).Returns(mockBlockDBSet.Object);
            mockRMDBContext.Setup(x => x.Blocks).Returns(mockBlockDBSet.Object);
            mockRMDBContext.Setup(c => c.Blocks.AsNoTracking()).Returns(mockBlockDBSet.Object);

            var mockAsynEnumerable6 = new DbAsyncEnumerable<BlockSequence>(blocksequence);
            var mockBlockSequenceDBSet = MockDbSet(blocksequence);
            mockBlockSequenceDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable6.AsQueryable().Provider);
            mockBlockSequenceDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable6.AsQueryable().Expression);
            mockBlockSequenceDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable6.AsQueryable().ElementType);
            mockBlockSequenceDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<BlockSequence>)mockAsynEnumerable6).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<BlockSequence>()).Returns(mockBlockSequenceDBSet.Object);
            mockRMDBContext.Setup(x => x.BlockSequences).Returns(mockBlockSequenceDBSet.Object);
            mockRMDBContext.Setup(c => c.BlockSequences.AsNoTracking()).Returns(mockBlockSequenceDBSet.Object);

            var mockAsynEnumerable7 = new DbAsyncEnumerable<DeliveryPoint>(deliverypoint);
            var mockDeliveryPointDBSet = MockDbSet(deliverypoint);
            mockDeliveryPointDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable7.AsQueryable().Provider);
            mockDeliveryPointDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable7.AsQueryable().Expression);
            mockDeliveryPointDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable7.AsQueryable().ElementType);
            mockDeliveryPointDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPoint>)mockAsynEnumerable7).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockDeliveryPointDBSet.Object);
            mockRMDBContext.Setup(x => x.DeliveryPoints).Returns(mockDeliveryPointDBSet.Object);
            mockRMDBContext.Setup(c => c.DeliveryPoints.AsNoTracking()).Returns(mockDeliveryPointDBSet.Object);

            var mockAsynEnumerable8 = new DbAsyncEnumerable<DeliveryPointAlias>(deliverypointalias);
            var mockDeliveryPointAliasDBSet = MockDbSet(deliverypointalias);
            mockDeliveryPointAliasDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable8.AsQueryable().Provider);
            mockDeliveryPointAliasDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable8.AsQueryable().Expression);
            mockDeliveryPointAliasDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable8.AsQueryable().ElementType);
            mockDeliveryPointAliasDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPointAlias>)mockAsynEnumerable8).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<DeliveryPointAlias>()).Returns(mockDeliveryPointAliasDBSet.Object);
            mockRMDBContext.Setup(x => x.DeliveryPointAlias).Returns(mockDeliveryPointAliasDBSet.Object);
            mockRMDBContext.Setup(c => c.DeliveryPointAlias.AsNoTracking()).Returns(mockDeliveryPointAliasDBSet.Object);

            var mockAsynEnumerable9 = new DbAsyncEnumerable<PostalAddress>(postaladdress);
            var mockPostalAddressDBSet = MockDbSet(postaladdress);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable9.AsQueryable().Provider);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable9.AsQueryable().Expression);
            mockPostalAddressDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable9.AsQueryable().ElementType);
            mockPostalAddressDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddress>)mockAsynEnumerable9).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressDBSet.Object);
            mockRMDBContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressDBSet.Object);
            mockRMDBContext.Setup(c => c.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressDBSet.Object);

            var mockAsynEnumerable10 = new DbAsyncEnumerable<Postcode>(postcode);
            var mockPostcodeDBSet = MockDbSet(postcode);
            mockPostcodeDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable10.AsQueryable().Provider);
            mockPostcodeDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable10.AsQueryable().Expression);
            mockPostcodeDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable10.AsQueryable().ElementType);
            mockPostcodeDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Postcode>)mockAsynEnumerable10).GetAsyncEnumerator());
            mockRMDBContext.Setup(x => x.Set<Postcode>()).Returns(mockPostcodeDBSet.Object);
            mockRMDBContext.Setup(x => x.Postcodes).Returns(mockPostcodeDBSet.Object);
            mockRMDBContext.Setup(c => c.Postcodes.AsNoTracking()).Returns(mockPostcodeDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RMDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new DeliveryRouteDataService(mockDatabaseFactory.Object);
        }
    }
}