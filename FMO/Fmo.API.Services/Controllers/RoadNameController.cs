using Fmo.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods used to fetch Road Link data.
    /// </summary>

    [Route("api/[controller]")]
    public class RoadNameController : FmoBaseController
    {
        private IRoadNameBusinessService roadNameBussinessService = default(IRoadNameBusinessService);

        public RoadNameController(IRoadNameBusinessService businessService)
        {
            this.roadNameBussinessService = businessService;
        }

        /// <summary>
        /// This method is used to get Route Link data.
        /// </summary>
        /// <param name="bbox">boundaryBox as strintg</param>
        /// <returns></returns>
        [Route("GetRouteLinks")]
        [HttpGet]
        public IActionResult GetRoouteData(string bbox)
        {
            string route = roadNameBussinessService.GetRoadRoutes(bbox, CurrentUserUnit);
            return Ok(route);
        }
    }
}