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
        public List<DeliveryRouteDTO> SearchDeliveryRoute(int operationStateID, int deliveryScenarioID)
        {
            return deliveryRouteBusinessService.SearchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        [HttpGet]
        public List<ReferenceDataDTO> RouteLogsStatus()
        {
            return deliveryRouteBusinessService.RouteLogStatus();
        }

        [HttpGet]
        public List<ScenarioDTO> SearchDeliveryScenario(int operationStateID, int deliveryUnitID)
        {
            return deliveryRouteBusinessService.SearchDeliveryScenario(operationStateID, deliveryUnitID);
        }
    }
}
