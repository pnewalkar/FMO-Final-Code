using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IRouteLogBusinessService
    {
        List<DeliveryRouteDTO> ListOfRouteLogs();

        List<ScenarioDTO> ListOfScenarios();
    }
}