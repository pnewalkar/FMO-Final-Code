using Fmo.BusinessServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.DataServices.Repositories.Interfaces;

namespace Fmo.BusinessServices.Services
{
    public class RouteSimulationBusinessService : IRouteSimulationBusinessService
    {
        protected IRouteSimulationRepository _routeSimulationRespository;
        public RouteSimulationBusinessService(IRouteSimulationRepository routeSimulationRespository)
        {
            _routeSimulationRespository = routeSimulationRespository;
        }

        public  List<DTO.DeliveryRoute> ListOfRouteSimulations()
        {
            List<Entities.DeliveryRoute> ListOfDeliveryRoute = _routeSimulationRespository.ListOfRoute();
            var listOfSimulationRoute = AutoMapper.Mapper.Map<List<Entities.DeliveryRoute>, List<DTO.DeliveryRoute>>(ListOfDeliveryRoute);
            return listOfSimulationRoute;
        }

        public List<ScenarioDTO> ListOfScenarios()
        {
            List<Entities.Scenario> ListOfScenario = _routeSimulationRespository.ListOfScenario();
            var listOfScenario = AutoMapper.Mapper.Map<List<Entities.Scenario>, List<DTO.ScenarioDTO>>(ListOfScenario);
            return listOfScenario;

        }

    }
}
