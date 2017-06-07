using System;
using System.Collections.Generic;
using System.Linq;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class ScenarioDataService : DataServiceBase<Scenario, RMDBContext>, IScenarioDataService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory">IDatabaseFactory reference</param>
        public ScenarioDataService(IDatabaseFactory<RMDBContext> databaseFactory)
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
                IEnumerable<Scenario> result = DataContext.Scenarios.AsNoTracking().ToList().Where(x => x.OperationalState_GUID == operationStateID && x.Unit_GUID == deliveryUnitID);
                return GenericMapper.MapList<Scenario, DTO.ScenarioDTO>(result.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}