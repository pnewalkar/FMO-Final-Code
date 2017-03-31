using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DTO.DeliveryRouteDTO> ListOfRoute(int operationStateID, int deliveryScenarioID);

        List<DeliveryRouteDTO> FetchDeliveryRoute(string searchText);
    }
}