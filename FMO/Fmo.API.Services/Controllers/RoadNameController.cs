using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods used to fetch Road Link data.
    /// </summary>

    [Route("api/[controller]")]
    public class RoadNameController : Controller
    {
        private IRoadNameBusinessService roadNameBussinessService = default(IRoadNameBusinessService);

        public RoadNameController(IRoadNameBusinessService businessService)
        {
            this.roadNameBussinessService = businessService;
        }

        /// <summary>
        /// This method is used to fetch Road name
        /// </summary>
        /// <param name="RoadNameDTO">RoadNameDTO</param>
        /// <returns>Task List of Road Name Dto</returns>
        public async Task<List<RoadNameDTO>> FetchRoadName(List<RoadNameDTO> RoadNameDTO)
        {
            return await roadNameBussinessService.FetchRoadName();
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
            return roadNameBussinessService.GetRoadRoutes(boundaryBox);
        }
    }
}