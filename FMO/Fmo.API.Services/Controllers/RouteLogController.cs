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
        protected IDeliveryRouteBusinessService deliveryRouteBusinessService = default(IDeliveryRouteBusinessService);

        public RouteLogController(IDeliveryRouteBusinessService _deliveryRouteBusinessService)
        {
            deliveryRouteBusinessService = _deliveryRouteBusinessService;
        }

        [HttpGet("DeliveryUnit")]
        public List<DeliveryUnitLocationDTO> DeliveryUnit()
        {
            return deliveryRouteBusinessService.FetchDeliveryUnit();
        }

        [HttpGet("ListOfRouteLogs")]
        public List<DeliveryRouteDTO> ListOfRouteLogs(int operationStateID, int deliveryScenarioID)
        {
            return deliveryRouteBusinessService.FetchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        [HttpGet("RouteLogsStatus")]
        public List<ReferenceDataDTO> RouteLogsStatus()
        {
            return deliveryRouteBusinessService.FetchRouteLogStatus();
        }

        [HttpGet("FetchDeliveryScenario")]
        public List<ScenarioDTO> FetchDeliveryScenario(int operationStateID, int deliveryUnitID)
        {
            return deliveryRouteBusinessService.FetchDeliveryScenario(operationStateID, deliveryUnitID);
        }
    }
}
