﻿using System;
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
        public List<DeliveryRoute> ListOfRouteLogs()
        {
            return deliveryRouteBusinessService.ListOfRoute();
        }

        [HttpGet]
        public List<ReferenceDataDTO> RouteLogsStatus()
        {
            return deliveryRouteBusinessService.ListOfRouteLogStatus();
        }

        [HttpGet]
        public List<ScenarioDTO> ListOfScenario()
        {
            return deliveryRouteBusinessService.ListOfScenario();
        }
    }
}
