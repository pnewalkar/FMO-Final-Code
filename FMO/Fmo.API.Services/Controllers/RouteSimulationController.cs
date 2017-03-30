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
        protected IDeliveryRouteBusinessService deliveryRouteBusinessService;

        [HttpGet]
        public List<DeliveryRouteDTO> RouteSimulations(int operationStateID, int deliveryScenarioID)
        {
            return deliveryRouteBusinessService.ListOfRoute(operationStateID, deliveryScenarioID);
        }

        [HttpGet]
        public List<ReferenceDataDTO> RouteLogsStatus()
        {
            return deliveryRouteBusinessService.ListOfRouteLogStatus();
        }

        [HttpGet]
        public List<ScenarioDTO> ListOfScenario(int operationStateID, int deliveryUnitID)
        {
            return deliveryRouteBusinessService.ListOfScenario(operationStateID, deliveryUnitID);
        }
    }
}
