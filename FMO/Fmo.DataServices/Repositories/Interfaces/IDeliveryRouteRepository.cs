using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID, Guid userUnit);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText, Guid userUnit);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText, Guid userUnit);

        Task<int> GetDeliveryRouteCount(string searchText, Guid userUnit);
    }
}