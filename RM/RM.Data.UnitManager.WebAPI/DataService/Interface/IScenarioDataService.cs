using System;
using System.Collections.Generic;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    public interface IScenarioDataService
    {
        List<DTO.ScenarioDTO> FetchScenario(Guid operationStateID, Guid deliveryUnitID);
    }
}