namespace Fmo.DataServices.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    public class ScenarioRepository : RepositoryBase<Scenario, FMODBContext>, IScenarioRepository
    {
        public ScenarioRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<DTO.ScenarioDTO> Scenario(int operationStateID, int deliveryUnitID)
        {
            IEnumerable<Scenario> result = DataContext.Scenarios.ToList().Where(x => x.OperationalState_Id == operationStateID && x.DeliveryUnit_Id == deliveryUnitID);
            return GenericMapper.MapList<Scenario, DTO.ScenarioDTO>(result.ToList());
        }
    }
}
