using System.Collections.Generic;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DTO.DeliveryRouteDTO> ListOfRoute(int operationStateID, int deliveryScenarioID);
    }
}