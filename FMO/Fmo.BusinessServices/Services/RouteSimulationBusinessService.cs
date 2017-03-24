using Fmo.BusinessServices.Interfaces;

using System.Collections.Generic;

using Fmo.DTO;

using Fmo.DataServices.Repositories.Interfaces;
using Fmo.MappingConfiguration.Interface;

namespace Fmo.BusinessServices.Services
{
    public class RouteSimulationBusinessService : IRouteSimulationBusinessService
    {
        private IRouteSimulationRepository routeSimulationRespository;

        public RouteSimulationBusinessService(IRouteSimulationRepository routeSimulationRespository)
        {
            this.routeSimulationRespository = routeSimulationRespository;
        }

        public List<DTO.DeliveryRoute> ListOfRouteSimulations()
        {
            List<DTO.DeliveryRoute> lst = new List<DTO.DeliveryRoute>();
            return lst;
        }

        public List<ScenarioDTO> ListOfScenarios()
        {
            List<ScenarioDTO> lst = new List<ScenarioDTO>();
            return lst;
        }
    }
}
