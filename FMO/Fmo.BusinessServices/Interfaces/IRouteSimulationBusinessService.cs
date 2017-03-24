using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.BusinessServices.Interfaces
{
   public  interface IRouteSimulationBusinessService
    {
        List<DTO.DeliveryRoute> ListOfRouteSimulations();
        List<ScenarioDTO> ListOfScenarios();
    }
}
