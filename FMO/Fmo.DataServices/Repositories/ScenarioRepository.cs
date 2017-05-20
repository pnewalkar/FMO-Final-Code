using System;
using System.Collections.Generic;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class ScenarioRepository : RepositoryBase<Scenario, FMODBContext>, IScenarioRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public ScenarioRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch the Delivery Scenario by passing the operationstateID and deliveryUnitID.
        /// </summary>
        /// <param name="operationStateId">Guid operationStateID</param>
        /// <param name="deliveryUnitId">Guid deliveryUnitID</param>
        /// <returns>List</returns>
        public List<DTO.ScenarioDTO> FetchScenario(Guid operationStateId, Guid deliveryUnitId)
        {
            IEnumerable<Scenario> result = DataContext.Scenarios.AsNoTracking().ToList()
                .Where(x => x.OperationalState_GUID == operationStateId && x.Unit_GUID == deliveryUnitId);
            return GenericMapper.MapList<Scenario, DTO.ScenarioDTO>(result.ToList());
        }
    }
}