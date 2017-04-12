namespace Fmo.DataServices.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;
    using System;

    public class ScenarioRepository : RepositoryBase<Scenario, FMODBContext>, IScenarioRepository
    {
        public ScenarioRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DTO.ScenarioDTO> FetchScenario(Guid operationStateID, Guid deliveryUnitID)
        {
            IEnumerable<Scenario> result = DataContext.Scenarios.ToList().Where(x => x.OperationalState_GUID == operationStateID && x.DeliveryUnit_GUID == deliveryUnitID);
            return GenericMapper.MapList<Scenario, DTO.ScenarioDTO>(result.ToList());
        }
    }
}
