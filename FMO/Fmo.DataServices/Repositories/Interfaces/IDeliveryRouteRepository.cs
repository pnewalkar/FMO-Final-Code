using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DeliveryRouteDTO> FetchDeliveryRoute(int operationStateID, int deliveryScenarioID);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText);

        Task<List<DeliveryRouteDTO>> FetchPostCodeUnitforBasicSearch(string searchText);

        Task<int> GetDeliveryRouteUnitCount(string searchText);
    }
}