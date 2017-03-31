using System.Collections.Generic;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DTO.DeliveryRouteDTO> SearchDeliveryRoute(int operationStateID, int deliveryScenarioID);
    }
}