using Fmo.BusinessServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.DataServices.Repositories.Interfaces;

namespace Fmo.BusinessServices.Services
{
    public class RouteLogBusinessService : IRouteLogBusinessService
    {
        protected IRouteLogRepository _routeLogRepository;

        public RouteLogBusinessService(IRouteLogRepository routeLogRepository)
        {
            _routeLogRepository = routeLogRepository;
        }

        public List<DeliveryRoute> ListOfRouteLogs()
        {
            List<Entities.DeliveryRoute> ListOfDeliveryRoute = _routeLogRepository.ListOfRouteLogs();
            var listOfSimulationRoute = AutoMapper.Mapper.Map<List<Entities.DeliveryRoute>, List<DTO.DeliveryRoute>>(ListOfDeliveryRoute);
            return listOfSimulationRoute;
        }

        public List<ScenarioDTO> ListOfScenarios()
        {
            List<Entities.Scenario> ListOfScenario = _routeLogRepository.ListOfScenario();
            var listOfScenario = AutoMapper.Mapper.Map<List<Entities.Scenario>, List<DTO.ScenarioDTO>>(ListOfScenario);
            return listOfScenario;
        }
    }
}
