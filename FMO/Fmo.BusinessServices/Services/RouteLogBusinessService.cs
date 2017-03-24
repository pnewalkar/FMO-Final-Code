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
        private IRouteLogRepository routeLogRepository;

        public RouteLogBusinessService(IRouteLogRepository routeLogRepository)
        {
            this.routeLogRepository = routeLogRepository;
        }

        public List<DeliveryRouteDTO> ListOfRouteLogs()
        {
            List<DeliveryRouteDTO> lst = new List<DeliveryRouteDTO>();
            return lst;
        }

        public List<ScenarioDTO> ListOfScenarios()
        {
            List<ScenarioDTO> lst = new List<ScenarioDTO>();
            return lst;
        }
    }
}
