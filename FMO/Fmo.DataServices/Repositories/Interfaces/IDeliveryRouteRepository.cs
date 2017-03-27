
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryRouteRepository
    {
        List<DTO.ReferenceDataDTO> ListOfRouteLogStatus();
        List<DTO.DeliveryRouteDTO> ListOfRoute();
        List<DTO.ScenarioDTO> ListOfScenario();
    }
}
