using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IRouteSimulationBusinessService
    {
        List<DTO.DeliveryRouteDTO> ListOfRouteSimulations();

        List<ScenarioDTO> ListOfScenarios();
    }
}