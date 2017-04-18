using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fmo.DTO;
using Fmo.BusinessServices.Interfaces;
using System.Security.Claims;
using System.Threading;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    //[EnableCors("AllowCors")]
    [Route("api/[controller]")]
    public class RouteLogController : Controller
    {
        protected IDeliveryRouteBusinessService deliveryRouteBusinessService = default(IDeliveryRouteBusinessService);

        public RouteLogController(IDeliveryRouteBusinessService _deliveryRouteBusinessService)
        {
            deliveryRouteBusinessService = _deliveryRouteBusinessService;
        }

       
        [Authorize]
        [HttpGet("DeliveryUnit")]
        public List<DeliveryUnitLocationDTO> DeliveryUnit()
        {
            var u = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub)
                               .Select(c => c.Value).SingleOrDefault();

            var g = User.Claims.Where(c => c.Type == ClaimTypes.UserData)
                               .Select(c => c.Value).SingleOrDefault();

            return deliveryRouteBusinessService.FetchDeliveryUnit();
        }



        [HttpGet("FetchDeliveryRoute")]
        public List<DeliveryRouteDTO> FetchDeliveryRoute(Guid operationStateID, Guid deliveryScenarioID)
        {
            return deliveryRouteBusinessService.FetchDeliveryRoute(operationStateID, deliveryScenarioID);
        }

        [HttpGet("RouteLogsStatus")]
        public List<ReferenceDataDTO> RouteLogsStatus()
        {
            return deliveryRouteBusinessService.FetchRouteLogStatus();
        }

        [HttpGet("RouteLogsSelectionType")]
        public List<ReferenceDataDTO> RouteLogsSelectionType()
        {
            return deliveryRouteBusinessService.FetchRouteLogSelectionType();
        }

        [HttpGet("FetchDeliveryScenario")]
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryUnitID)
        {
            return deliveryRouteBusinessService.FetchDeliveryScenario(operationStateID, deliveryUnitID);
        }
    }
}
