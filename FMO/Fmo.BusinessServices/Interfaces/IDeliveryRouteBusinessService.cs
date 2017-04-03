using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        List<DeliveryUnitLocationDTO> FetchDeliveryUnit();

        List<ReferenceDataDTO> FetchRouteLogStatus();

        List<DeliveryRouteDTO> FetchDeliveryRoute(int operationStateID, int deliveryScenarioID);

        List<ScenarioDTO> FetchDeliveryScenario(int operationStateID, int deliveryUnitID);

        List<DeliveryRouteDTO> FetchDeliveryRoute(string searchText);
    }
}