using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using System.IO;

namespace Fmo.BusinessServices.Interfaces
{
   public interface IRoadNameBussinessService
    {
        Task<List<RoadNameDTO>> FetchRoadName();

        string GetRoadRoutes(string boundarybox);
    }
}
