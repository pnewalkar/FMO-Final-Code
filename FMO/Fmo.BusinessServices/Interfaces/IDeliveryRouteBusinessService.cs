using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        List<DeliveryUnitLocationDTO> FetchDeliveryUnit();

        List<ReferenceDataDTO> FetchRouteLogStatus();

        List<DeliveryRouteDTO> FetchDeliveryRoute(int operationStateID, int deliveryScenarioID);

        List<ScenarioDTO> FetchDeliveryScenario(int operationStateID, int deliveryUnitID);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText);
    }
}