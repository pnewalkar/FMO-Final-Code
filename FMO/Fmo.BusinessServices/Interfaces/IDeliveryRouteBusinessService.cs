using System.Collections.Generic;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        List<DTO.ReferenceDataDTO> RouteLogStatus();
        List<DTO.DeliveryRouteDTO> SearchDeliveryRoute(int operationStateID, int deliveryScenarioID);
        List<DTO.ScenarioDTO> SearchDeliveryScenario(int operationStateID, int deliveryUnitID);
    }
}