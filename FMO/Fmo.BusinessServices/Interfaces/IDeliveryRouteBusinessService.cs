using System.Collections.Generic;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        List<DTO.ReferenceDataDTO> ListOfRouteLogStatus();

        List<DTO.DeliveryRouteDTO> ListOfRoute();

        List<DTO.ScenarioDTO> ListOfScenario();
    }
}