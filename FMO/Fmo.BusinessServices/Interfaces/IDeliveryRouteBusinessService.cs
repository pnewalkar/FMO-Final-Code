using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        List<ReferenceDataDTO> ListOfRouteLogStatus();

        List<DeliveryRouteDTO> ListOfRoute(int operationStateID, int deliveryScenarioID);

        List<ScenarioDTO> ListOfScenario(int operationStateID, int deliveryUnitID);

        List<DeliveryRouteDTO> FetchDeliveryRoute(string searchText);
    }
}