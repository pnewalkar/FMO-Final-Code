using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class RouteSimulationBusinessService : IRouteSimulationBusinessService
    {
        private IRouteSimulationRepository routeSimulationRespository;

        public RouteSimulationBusinessService(IRouteSimulationRepository routeSimulationRespository)
        {
            this.routeSimulationRespository = routeSimulationRespository;
        }

        public List<DTO.DeliveryRouteDTO> ListOfRouteSimulations()
        {
            List<DTO.DeliveryRouteDTO> lst = new List<DTO.DeliveryRouteDTO>();
            return lst;
        }

        public List<ScenarioDTO> ListOfScenarios()
        {
            List<ScenarioDTO> lst = new List<ScenarioDTO>();
            return lst;
        }
    }
}