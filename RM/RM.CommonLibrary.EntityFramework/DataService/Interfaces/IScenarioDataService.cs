using System;
using System.Collections.Generic;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    public interface IScenarioDataService
    {
        List<DTO.ScenarioDTO> FetchScenario(Guid operationStateID, Guid deliveryUnitID);
    }
}