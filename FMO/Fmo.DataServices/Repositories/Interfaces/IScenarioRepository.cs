using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
   public interface IScenarioRepository
    {
        List<DTO.ScenarioDTO> FetchScenario(Guid operationStateID, Guid deliveryUnitID);
    }
}
