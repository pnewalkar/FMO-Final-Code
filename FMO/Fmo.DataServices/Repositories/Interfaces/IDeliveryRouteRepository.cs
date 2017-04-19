using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText);

        Task<int> GetDeliveryRouteCount(string searchText);
    }
}