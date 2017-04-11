using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;
using System;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryRouteBusinessService
    {
        List<DeliveryUnitLocationDTO> FetchDeliveryUnit();

        List<ReferenceDataDTO> FetchRouteLogStatus();

        List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID);

        List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteforBasicSearch(string searchText);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText);
    }
}