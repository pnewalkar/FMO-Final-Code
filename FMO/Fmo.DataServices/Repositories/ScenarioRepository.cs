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
        /// <param name="operationStateID">Guid operationStateID</param>
        /// <param name="deliveryUnitID">Guid deliveryUnitID</param>
        /// <returns>List</returns>
        public List<DTO.ScenarioDTO> FetchScenario(Guid operationStateID, Guid deliveryUnitID)
        {
            try
            {
                IEnumerable<Scenario> result = DataContext.Scenarios.ToList().Where(x => x.OperationalState_GUID == operationStateID && x.Unit_GUID == deliveryUnitID);
                return GenericMapper.MapList<Scenario, DTO.ScenarioDTO>(result.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
