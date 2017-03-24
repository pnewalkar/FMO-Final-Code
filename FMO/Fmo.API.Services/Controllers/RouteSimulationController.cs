using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class RouteSimulationController : Controller
    {
        protected IRouteSimulationBusinessService routeSimulationBusinessService;

        public List<DeliveryRoute> ListOfRouteLogs()
        {
            return routeSimulationBusinessService.ListOfRouteSimulations();
        }

        public List<ScenarioDTO> ListOfScenario()
        {
            return routeSimulationBusinessService.ListOfScenarios();
        }
    }
}
