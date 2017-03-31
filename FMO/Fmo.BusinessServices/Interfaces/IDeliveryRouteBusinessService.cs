using System.Collections.Generic;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        List<DTO.ReferenceDataDTO> ListOfRouteLogStatus();
        List<DTO.DeliveryRouteDTO> ListOfRoute(int operationStateID, int deliveryScenarioID);
        List<DTO.ScenarioDTO> ListOfScenario(int operationStateID, int deliveryUnitID);
    }
}