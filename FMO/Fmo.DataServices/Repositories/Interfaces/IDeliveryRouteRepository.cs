using System.Collections.Generic;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DTO.ReferenceDataDTO> ListOfRouteLogStatus();

        List<DTO.DeliveryRouteDTO> ListOfRoute();

        List<DTO.ScenarioDTO> ListOfScenario();
    }
}