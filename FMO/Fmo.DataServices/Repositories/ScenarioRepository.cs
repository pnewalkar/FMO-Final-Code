namespace Fmo.DataServices.Repositories
{
    using System;
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

        /// <summary>
        /// Fetch the Delivery Scenario by passing the operationstateID and deliveryUnitID.
        /// </summary>
        /// <param name="operationStateID">Guid</param>
        /// <param name="deliveryUnitID">Guid</param>
        /// <returns>List</returns>
        public List<DTO.ScenarioDTO> FetchScenario(Guid operationStateID, Guid deliveryUnitID)
        {
            try
            {
                IEnumerable<Scenario> result = DataContext.Scenarios.ToList().Where(x => x.OperationalState_GUID == operationStateID && x.DeliveryUnit_GUID == deliveryUnitID);
                return GenericMapper.MapList<Scenario, DTO.ScenarioDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
