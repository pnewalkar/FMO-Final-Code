using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Fmo.BusinessServices.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RM.DataManagement.RoadName.WebAPI.Controllers
{
    /// <summary>
    /// This class contains methods used to fetch Road Link data.
    /// </summary>

    [Route("api/[controller]")]
    public class RoadNameController : RMBaseController
    {
        private IRoadNameBusinessService roadNameBussinessService = default(IRoadNameBusinessService);

        public RoadNameController(IRoadNameBusinessService businessService)
        {
            this.roadNameBussinessService = businessService;
        }

        /// <summary>
        /// This method is used to get Route Link data.
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as strintg</param>
        /// <returns></returns>
        [Route("GetRouteLinks")]
        [HttpGet]
        public string GetRoouteData(string boundaryBox)
        {
            return roadNameBussinessService.GetRoadRoutes(boundaryBox, CurrentUserUnit);
        }
    }
}