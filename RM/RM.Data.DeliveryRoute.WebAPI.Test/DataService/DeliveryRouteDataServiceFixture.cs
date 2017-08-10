using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryRoute.WebAPI.Entities;
using RM.DataManagement.DeliveryRoute.WebAPI.DataService;
using Common = RM.CommonLibrary.EntityFramework.DataService.Interfaces;

namespace RM.Data.DeliveryRoute.WebAPI.Test
{
    [TestFixture]
    public class DeliveryRouteDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<RouteDBContext> mockRouteDBContext;
        private Mock<IConfigurationHelper> mockConfigHelper;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private Mock<IDatabaseFactory<RouteDBContext>> mockDatabaseFactory;
        private IDeliveryRouteDataService testCandidate;

        [Test]
        public async Task TestGetScenarioRoutes_PositiveScenario()
        {
            var actualResult = await testCandidate.GetScenarioRoutes(new Guid("ED148EE4-46A7-4EA5-B0C9-1654085159D0"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 1);
        }

        [Test]
        public async Task TestGetScenarioRoutes_NegativeScenario()
        {
            var actualResult = await testCandidate.GetScenarioRoutes(new Guid("95154783-1D4C-4F2B-9652-CFEEDB8C1EDF"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestGetRoutesForAdvanceSearch_PositiveScenario()
        {
            var actualResult = await testCandidate.GetRoutesForAdvanceSearch("141", new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 1);
        }

        [Test]
        public async Task TestGetRoutesForAdvanceSearch_NegativeScenario()
        {
            var actualResult = await testCandidate.GetRoutesForAdvanceSearch("141", new Guid("B512BC9F-F22B-444D-86C3-FB55F1151885"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestGetRoutesForBasicSearch_PositiveScenario()
        {
            var actualResult = await testCandidate.GetRoutesForBasicSearch("141", new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 1);
        }

        [Test]
        public async Task TestGetRoutesForBasicSearch_NegativeScenario()
        {
            var actualResult = await testCandidate.GetRoutesForBasicSearch("ABC", new Guid("B512BC9F-F22B-444D-86C3-FB55F1151885"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestGetRouteCount_PositiveScenario()
        {
            var actualResult = await testCandidate.GetRouteCount("141", new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F"));
            Assert.IsTrue(actualResult == 1);
        }

        [Test]
        public async Task TestGetRouteCount_NegativeScenario()
        {
            var actualResult = await testCandidate.GetRouteCount("ABC", new Guid("B512BC9F-F22B-444D-86C3-FB55F1151885"));
            Assert.IsTrue(actualResult == 0);
        }

        [Test]
        public async Task TestGetRoutesByLocation_PositiveScenario()
        {
            var actualResult = await testCandidate.GetRoutesByLocation(new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 1);
        }

        [Test]
        public async Task TestGetRoutesByLocation_NegativeScenario()
        {
            var actualResult = await testCandidate.GetRoutesByLocation(new Guid("53BF03D5-D4C2-43CA-BFEE-A7328B3592FB"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestGetRouteByDeliverypoint_PositiveScenario()
        {
            var actualResult = await testCandidate.GetRouteByDeliverypoint(new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F"));
            Assert.IsNotNull(actualResult);
        }

        [Test]
        public async Task TestGetRouteByDeliverypoint_NegativeScenario()
        {
            var actualResult = await testCandidate.GetRouteByDeliverypoint(new Guid("2A0CDB34-7ABF-480E-B8BB-1A7C131ABAB5"));
            Assert.IsNull(actualResult.RouteName);
        }

        [Test]
        public async Task TestGetRouteSummary_PositiveScenario()
        {
            var actualResult = await testCandidate.GetRouteSummary(new Guid("F87F67EC-FAFA-4156-9C3D-85722719CE12"), GetReferenceDataCategory());
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Method == "RM Van (Shared)");
            Assert.IsTrue(actualResult.Totaltime == "2:00 mins");
            Assert.IsTrue(actualResult.DPs == 1);
            Assert.IsTrue(actualResult.ResidentialDPs == 1);
            Assert.IsFalse(actualResult.BusinessDPs == 1);
        }

        [Test]
        public async Task TestGetRouteSummary_NegativeScenario()
        {
            var actualResult = await testCandidate.GetRouteSummary(new Guid("6134CF8B-20AD-407B-8D0D-BBCC4198C35A"), GetReferenceDataCategory());
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(string.IsNullOrEmpty(actualResult.Method));
            Assert.IsTrue(string.IsNullOrEmpty(actualResult.Totaltime));
        }

        [Test]
        public async Task TestGetSequencedRouteDetails_NegativeScenario()
        {
            var actualResult = await testCandidate.GetSequencedRouteDetails(new Guid("6134CF8B-20AD-407B-8D0D-BBCC4198C35A"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 0);
        }

        [Test]
        public async Task TestGetSequencedRouteDetails_PositiveScenario()
        {
            var actualResult = await testCandidate.GetSequencedRouteDetails(new Guid("F87F67EC-FAFA-4156-9C3D-85722719CE12"));
            Assert.IsNotNull(actualResult);
            Assert.IsTrue(actualResult.Count == 1);
            Assert.IsTrue(actualResult[0].StreetName == "Steyne Gardens");
            Assert.IsTrue(actualResult[0].BuildingNumber == 10);
            Assert.IsTrue(actualResult[0].SubBuildingName == "Apartment 33");
            Assert.IsTrue(actualResult[0].BuildingName == "Warnes");
        }

        protected override void OnSetup()
        {
            var scenarios = new List<Scenario>() { new Scenario() { ID = new Guid("ED148EE4-46A7-4EA5-B0C9-1654085159D0"), ScenarioName = "High Wycome Collection Office - Baseline week", LocationID = new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F") } };
            var scenarioRoutes = new List<ScenarioRoute>() { new ScenarioRoute() { ScenarioID = new Guid("ED148EE4-46A7-4EA5-B0C9-1654085159D0"), RouteID = new Guid("F87F67EC-FAFA-4156-9C3D-85722719CE12") } };
            var routes = new List<Route>() { new Route() { ID = new Guid("F87F67EC-FAFA-4156-9C3D-85722719CE12"), SpanTimeMinute = 120, RouteName = "1415 BROADWATER ST EAST", RouteNumber = "415", UnSequencedBlockID = new Guid("61154482-8493-4130-A5E1-64E86C9757BC"), RouteMethodTypeGUID = new Guid("E1D25B7F-561B-E711-9F8C-28D244AEF9ED") } };
            var routeActivites = new List<RouteActivity>() { new RouteActivity() { ID = new Guid("F21BD083-9FB5-4420-86EF-7205A5075870"), RouteID = new Guid("F87F67EC-FAFA-4156-9C3D-85722719CE12"), ActivityTypeGUID = new Guid("7803247C-F866-E711-80ED-000D3A22173B"), ResourceGUID = new Guid("E1D25B7F-561B-E711-9F8C-28D244AEF9ED"), LocationID = new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F") } };
            var blockSequences = new List<BlockSequence>() { new BlockSequence() { ID = new Guid("4FEB3A4A-A25E-42AF-AAD9-6B0BD51C9A1D"), BlockID = new Guid("61154482-8493-4130-A5E1-64E86C9757BC"), LocationID = new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F") } };
            var postcodes = new List<Postcode>() { new Postcode() { ID = new Guid("E436B42D-A2C1-4E15-9E84-EB48A4474608"), PrimaryRouteGUID = new Guid("02B18F5E-1C1A-410F-AA51-C7CA0EC7216C"), PostcodeUnit = "EC4P 4GY" } };
            var deliveryPoints = new List<DeliveryPoint>() {
                new DeliveryPoint() { ID = new Guid("F08AAD61-B83F-463D-95D6-F0583F24644F"), DeliveryPointUseIndicatorGUID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), MultipleOccupancyCount = 10, PostalAddressID = new Guid("7A5BF574-9ACA-4B8E-809D-00062A033342"),
                    PostalAddress = new PostalAddress() { ID = new Guid("7A5BF574-9ACA-4B8E-809D-00062A033342"), Thoroughfare = "Steyne Gardens", BuildingNumber = 10, SubBuildingName = "Apartment 33", BuildingName = "Warnes", Postcode = "EC4P 4GY" } } };
            var postalAddresses = new List<PostalAddress>() { new PostalAddress() { ID = new Guid("7A5BF574-9ACA-4B8E-809D-00062A033342"), Thoroughfare = "Steyne Gardens", BuildingNumber = 10, SubBuildingName = "Apartment 33", BuildingName = "Warnes", Postcode = "EC4P 4GY" } };

            mockRouteDBContext = CreateMock<RouteDBContext>();

            mockLoggingHelper = CreateMock<ILoggingHelper>();
            mockConfigHelper = CreateMock<IConfigurationHelper>();
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);
            mockConfigHelper.Setup(x => x.ReadAppSettingsConfigurationValues(It.IsAny<string>())).Returns("5");

            var mockAsyncScenarios = new DbAsyncEnumerable<Scenario>(scenarios);
            var mockScenariosDBSet = MockDbSet(scenarios);
            mockScenariosDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsyncScenarios.AsQueryable().Provider);
            mockScenariosDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsyncScenarios.AsQueryable().Expression);
            mockScenariosDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsyncScenarios.AsQueryable().ElementType);
            mockScenariosDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Scenario>)mockAsyncScenarios).GetAsyncEnumerator());
            mockRouteDBContext.Setup(x => x.Set<Scenario>()).Returns(mockScenariosDBSet.Object);
            mockRouteDBContext.Setup(x => x.Scenarios).Returns(mockScenariosDBSet.Object);
            mockRouteDBContext.Setup(c => c.Scenarios.AsNoTracking()).Returns(mockScenariosDBSet.Object);

            var mockAsynScenarioRoutes = new DbAsyncEnumerable<ScenarioRoute>(scenarioRoutes);
            var mockScenarioRoutesDBSet = MockDbSet(scenarioRoutes);
            mockScenarioRoutesDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynScenarioRoutes.AsQueryable().Provider);
            mockScenarioRoutesDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynScenarioRoutes.AsQueryable().Expression);
            mockScenarioRoutesDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynScenarioRoutes.AsQueryable().ElementType);
            mockScenarioRoutesDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<ScenarioRoute>)mockAsynScenarioRoutes).GetAsyncEnumerator());
            mockRouteDBContext.Setup(x => x.Set<ScenarioRoute>()).Returns(mockScenarioRoutesDBSet.Object);
            mockRouteDBContext.Setup(x => x.ScenarioRoutes).Returns(mockScenarioRoutesDBSet.Object);
            mockRouteDBContext.Setup(c => c.ScenarioRoutes.AsNoTracking()).Returns(mockScenarioRoutesDBSet.Object);

            var mockAsynRoutes = new DbAsyncEnumerable<Route>(routes);
            var mockRoutesDBSet = MockDbSet(routes);
            mockRoutesDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynRoutes.AsQueryable().Provider);
            mockRoutesDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynRoutes.AsQueryable().Expression);
            mockRoutesDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynRoutes.AsQueryable().ElementType);
            mockRoutesDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Route>)mockAsynRoutes).GetAsyncEnumerator());
            mockRouteDBContext.Setup(x => x.Set<Route>()).Returns(mockRoutesDBSet.Object);
            mockRouteDBContext.Setup(x => x.Routes).Returns(mockRoutesDBSet.Object);
            mockRouteDBContext.Setup(c => c.Routes.AsNoTracking()).Returns(mockRoutesDBSet.Object);

            var mockAsynRouteActivites = new DbAsyncEnumerable<RouteActivity>(routeActivites);
            var mockRouteAcitivityDBSet = MockDbSet(routeActivites);
            mockRouteAcitivityDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynRouteActivites.AsQueryable().Provider);
            mockRouteAcitivityDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynRouteActivites.AsQueryable().Expression);
            mockRouteAcitivityDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynRouteActivites.AsQueryable().ElementType);
            mockRouteAcitivityDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<RouteActivity>)mockAsynRouteActivites).GetAsyncEnumerator());
            mockRouteDBContext.Setup(x => x.Set<RouteActivity>()).Returns(mockRouteAcitivityDBSet.Object);
            mockRouteDBContext.Setup(x => x.RouteActivities).Returns(mockRouteAcitivityDBSet.Object);
            mockRouteDBContext.Setup(c => c.RouteActivities.AsNoTracking()).Returns(mockRouteAcitivityDBSet.Object);

            var mockAsynBlockSequences = new DbAsyncEnumerable<BlockSequence>(blockSequences);
            var mockBlockSequencesDBSet = MockDbSet(blockSequences);
            mockBlockSequencesDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynBlockSequences.AsQueryable().Provider);
            mockBlockSequencesDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynBlockSequences.AsQueryable().Expression);
            mockBlockSequencesDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynBlockSequences.AsQueryable().ElementType);
            mockBlockSequencesDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<BlockSequence>)mockAsynBlockSequences).GetAsyncEnumerator());
            mockRouteDBContext.Setup(x => x.Set<BlockSequence>()).Returns(mockBlockSequencesDBSet.Object);
            mockRouteDBContext.Setup(x => x.BlockSequences).Returns(mockBlockSequencesDBSet.Object);
            mockRouteDBContext.Setup(c => c.BlockSequences.AsNoTracking()).Returns(mockBlockSequencesDBSet.Object);

            var mockAsynPostcodes = new DbAsyncEnumerable<Postcode>(postcodes);
            var mockPoscodesDBSet = MockDbSet(postcodes);
            mockPoscodesDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynPostcodes.AsQueryable().Provider);
            mockPoscodesDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynPostcodes.AsQueryable().Expression);
            mockPoscodesDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynPostcodes.AsQueryable().ElementType);
            mockPoscodesDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Postcode>)mockAsynPostcodes).GetAsyncEnumerator());
            mockRouteDBContext.Setup(x => x.Set<Postcode>()).Returns(mockPoscodesDBSet.Object);
            mockRouteDBContext.Setup(x => x.Postcodes).Returns(mockPoscodesDBSet.Object);
            mockRouteDBContext.Setup(c => c.Postcodes.AsNoTracking()).Returns(mockPoscodesDBSet.Object);

            var mockAsyndeliveryPoints = new DbAsyncEnumerable<DeliveryPoint>(deliveryPoints);
            var mockdeliveryPointsDBSet = MockDbSet(deliveryPoints);
            mockdeliveryPointsDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsyndeliveryPoints.AsQueryable().Provider);
            mockdeliveryPointsDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsyndeliveryPoints.AsQueryable().Expression);
            mockdeliveryPointsDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsyndeliveryPoints.AsQueryable().ElementType);
            mockdeliveryPointsDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<DeliveryPoint>)mockAsyndeliveryPoints).GetAsyncEnumerator());
            mockRouteDBContext.Setup(x => x.Set<DeliveryPoint>()).Returns(mockdeliveryPointsDBSet.Object);
            mockRouteDBContext.Setup(x => x.DeliveryPoints).Returns(mockdeliveryPointsDBSet.Object);
            mockdeliveryPointsDBSet.Setup(x => x.Include(It.IsAny<string>())).Returns(mockdeliveryPointsDBSet.Object);
            mockdeliveryPointsDBSet.Setup(c => c.AsNoTracking()).Returns(mockdeliveryPointsDBSet.Object);

            var mockAsyncPostalAddresses = new DbAsyncEnumerable<PostalAddress>(postalAddresses);
            var mockPostalAddressesDBSet = MockDbSet(postalAddresses);
            mockPostalAddressesDBSet.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsyncPostalAddresses.AsQueryable().Provider);
            mockPostalAddressesDBSet.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsyncPostalAddresses.AsQueryable().Expression);
            mockPostalAddressesDBSet.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsyncPostalAddresses.AsQueryable().ElementType);
            mockPostalAddressesDBSet.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddress>)mockAsyncPostalAddresses).GetAsyncEnumerator());
            mockRouteDBContext.Setup(x => x.Set<PostalAddress>()).Returns(mockPostalAddressesDBSet.Object);
            mockRouteDBContext.Setup(x => x.PostalAddresses).Returns(mockPostalAddressesDBSet.Object);
            mockRouteDBContext.Setup(c => c.PostalAddresses.AsNoTracking()).Returns(mockPostalAddressesDBSet.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<RouteDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRouteDBContext.Object);

            testCandidate = new DeliveryRouteDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object, mockConfigHelper.Object);
        }

        private List<ReferenceDataCategoryDTO> GetReferenceDataCategory()
        {
            return new List<ReferenceDataCategoryDTO>()
        {
            new ReferenceDataCategoryDTO()
            {
                CategoryName = "DeliveryPoint Use Indicator", CategoryType = 2, Maintainable = false, Id = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED"),
                ReferenceDatas = new List<ReferenceDataDTO>()
                {
                    new ReferenceDataDTO() { DataDescription = "Commercial", ReferenceDataValue = "Commercial", ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"), ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") },
                    new ReferenceDataDTO() { DataDescription = "Residential", ReferenceDataValue = "Residential", ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") }
                }
            },
             new ReferenceDataCategoryDTO()
            {
                CategoryName = "Delivery Route Method Type", CategoryType = 2, Maintainable = false, Id = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed"),
                ReferenceDatas = new List<ReferenceDataDTO>()
                {
                    new ReferenceDataDTO() { DataDescription = "High Capacity Trolle", ReferenceDataValue = "High Capacity Trolle", ID = new Guid("c168f46e-561b-e711-9f8c-28d244aef9ed"), ReferenceDataCategory_GUID = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed") },
                    new ReferenceDataDTO() { DataDescription = "RM Van (Shared)", ReferenceDataValue = "RM Van (Shared)", ID = new Guid("e1d25b7f-561b-e711-9f8c-28d244aef9ed"), ReferenceDataCategory_GUID = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed") },
                    new ReferenceDataDTO() { DataDescription = "RM Van", ReferenceDataValue = "RM Van", ID = new Guid("c5b94b88-561b-e711-9f8c-28d244aef9ed"), ReferenceDataCategory_GUID = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed") },
                    new ReferenceDataDTO() { DataDescription = "PO BOX", ReferenceDataValue = "PO BOX", ID = new Guid("492f4394-561b-e711-9f8c-28d244aef9ed"), ReferenceDataCategory_GUID = new Guid("d1ce5369-551b-e711-9f8c-28d244aef9ed") }
                }
            },
              new ReferenceDataCategoryDTO()
            {
                CategoryName = "Operational Object Type", CategoryType = 2, Maintainable = false, Id = new Guid("bbc205a9-97c4-4345-ae8f-c485d243ecfc"),
                ReferenceDatas = new List<ReferenceDataDTO>()
                {
                    new ReferenceDataDTO() { DataDescription = "Delivery point", ReferenceDataValue = "DP", ID = new Guid("9f82733d-c72c-4111-815d-8813790b5cfb"), ReferenceDataCategory_GUID = new Guid("bbc205a9-97c4-4345-ae8f-c485d243ecfc") },
                    new ReferenceDataDTO() { DataDescription = "RMG Delivery point", ReferenceDataValue = "RMG DP", ID = new Guid("e0b3dbc4-c2e3-40f7-9df0-eb13c6da0cb0"), ReferenceDataCategory_GUID = new Guid("bbc205a9-97c4-4345-ae8f-c485d243ecfc") }
                }
            },
               new ReferenceDataCategoryDTO()
            {
                CategoryName = "Route Activity Type", CategoryType = 2, Maintainable = false, Id = new Guid("0102B1D1-EF66-E711-80ED-000D3A22173B"),
                ReferenceDatas = new List<ReferenceDataDTO>()
                {
                    new ReferenceDataDTO() { DataDescription = "Service Demand Activity", ReferenceDataValue = "Service Demand Activity", ID = new Guid("F22DC917-F066-E711-80ED-000D3A22173B"), ReferenceDataCategory_GUID = new Guid("0102B1D1-EF66-E711-80ED-000D3A22173B") },
                     new ReferenceDataDTO() { DataDescription = "Travel Out", ReferenceDataValue = "Travel Out", ID = new Guid("7803247C-F866-E711-80ED-000D3A22173B"), ReferenceDataCategory_GUID = new Guid("0102B1D1-EF66-E711-80ED-000D3A22173B") },
                     new ReferenceDataDTO() { DataDescription = "Travel In", ReferenceDataValue = "Travel In", ID = new Guid("9CC0D090-F866-E711-80ED-000D3A22173B"), ReferenceDataCategory_GUID = new Guid("0102B1D1-EF66-E711-80ED-000D3A22173B") },
                }
            }
        };
        }
    }
}