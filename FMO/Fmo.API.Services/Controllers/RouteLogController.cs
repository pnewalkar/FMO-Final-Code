using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.DTO;
using Fmo.BusinessServices.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class RouteLogController : Controller
    {
        protected IDeliveryRouteBusinessService deliveryRouteBusinessService;

        [HttpGet]
        public List<DeliveryRouteDTO> ListOfRouteLogs(int operationStateID, int deliveryScenarioID)
        {
            return deliveryRouteBusinessService.SearchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        [HttpGet]
        public List<ReferenceDataDTO> RouteLogsStatus()
        {
            return deliveryRouteBusinessService.RouteLogStatus();
        }

        [HttpGet]
        public List<ScenarioDTO> ListOfScenario(int operationStateID, int deliveryUnitID)
        {
            return deliveryRouteBusinessService.SearchDeliveryScenario(operationStateID, deliveryUnitID);
        }
    }
}
