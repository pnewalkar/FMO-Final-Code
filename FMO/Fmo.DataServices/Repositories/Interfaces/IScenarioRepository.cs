using System;
using System.Collections.Generic;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IScenarioRepository
    {
        List<DTO.ScenarioDTO> FetchScenario(Guid operationStateID, Guid deliveryUnitID);
    }
}