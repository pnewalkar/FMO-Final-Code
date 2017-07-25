using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.UnitManager.WebAPI.DataService;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.Entity;

namespace RM.Data.UnitManager.WebAPI.Test.DataService
{
    /// <summary>
    /// This class contains test methods for ScenarioDataService
    /// </summary>
    [TestFixture]
    public class ScenarioDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<UnitManagerDbContext> mockUnitManagerDbContext;
        private Mock<IDatabaseFactory<UnitManagerDbContext>> mockDatabaseFactory;
        private Mock<ILoggingHelper> mockILoggingHelper;
        private IScenarioDataService testCandidate;
        private Guid scenarioStatusGUID = new Guid("8534AA41-391F-4579-A18D-D7EDF5B5F918");
        private Guid locationID = new Guid("1534AA41-391F-4579-A18D-D7EDF5B5F918");

        /// <summary>
        /// Passed all correct values
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetScenariosByOperationStateAndDeliveryUnit_PositiveScenario()
        {
            var result = await testCandidate.GetScenariosByOperationStateAndDeliveryUnit(scenarioStatusGUID, locationID);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Value of locationID is empty
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetScenariosByOperationStateAndDeliveryUnit_NegativeScenario1()
        {
            var result = await testCandidate.GetScenariosByOperationStateAndDeliveryUnit(scenarioStatusGUID, Guid.Empty);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Value of operationStateID is empty
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetScenariosByOperationStateAndDeliveryUnit_NegativeScenario2()
        {
            var result = await testCandidate.GetScenariosByOperationStateAndDeliveryUnit(Guid.Empty, locationID);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// setup for nunit tests
        /// </summary>
        protected override void OnSetup()
        {
            // Data Setup
            List<Scenario> scenarioList = new List<Scenario>()
            {
                new Scenario()
                {
                    LocationID = locationID,
                    ID = new Guid("2534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            List<ScenarioStatus> scenarioStatusList = new List<ScenarioStatus>()
            {
                new ScenarioStatus()
                {
                    ScenarioStatusGUID = scenarioStatusGUID,
                    ScenarioID = new Guid("2534AA41-391F-4579-A18D-D7EDF5B5F918")
                }
            };

            mockUnitManagerDbContext = CreateMock<UnitManagerDbContext>();
            mockILoggingHelper = CreateMock<ILoggingHelper>();

            // Setup for Scenario
            var mockAsynEnumerable1 = new DbAsyncEnumerable<Scenario>(scenarioList);
            var mockScenario = MockDbSet(scenarioList);
            mockScenario.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockScenario.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockScenario.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockScenario.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<Scenario>)mockAsynEnumerable1).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<Scenario>()).Returns(mockScenario.Object);
            mockUnitManagerDbContext.Setup(x => x.Scenarios).Returns(mockScenario.Object);
            mockUnitManagerDbContext.Setup(c => c.Scenarios.AsNoTracking()).Returns(mockScenario.Object);

            // Setup for PostalAddress
            var mockAsynEnumerable2 = new DbAsyncEnumerable<ScenarioStatus>(scenarioStatusList);
            var mockScenarioStatus = MockDbSet(scenarioStatusList);
            mockScenarioStatus.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockScenarioStatus.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockScenarioStatus.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockScenarioStatus.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<ScenarioStatus>)mockAsynEnumerable2).GetAsyncEnumerator());
            mockUnitManagerDbContext.Setup(x => x.Set<ScenarioStatus>()).Returns(mockScenarioStatus.Object);
            mockUnitManagerDbContext.Setup(x => x.ScenarioStatus).Returns(mockScenarioStatus.Object);
            mockUnitManagerDbContext.Setup(c => c.ScenarioStatus.AsNoTracking()).Returns(mockScenarioStatus.Object);

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockILoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<UnitManagerDbContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockUnitManagerDbContext.Object);
            testCandidate = new ScenarioDataService(mockDatabaseFactory.Object, mockILoggingHelper.Object);
        }
    }
}