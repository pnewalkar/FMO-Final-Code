using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.DataManagement.RoadName.WebAPI.BusinessService.Interface
{
    public interface IRoadNameBusinessService
    {
        /// <summary>
        /// This method is used to Fetch Road Links data as per coordinates
        /// </summary>
        /// <param name="boundaryBox">boundaryBox as string.</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>string of Road Link data</returns>
        string GetRoadRoutes(string boundaryBox, Guid unitGuid);
    }
}
