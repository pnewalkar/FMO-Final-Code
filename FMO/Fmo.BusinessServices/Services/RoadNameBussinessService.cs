using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using System.IO;

namespace Fmo.BusinessServices.Services 
{
   public class RoadNameBussinessService : IRoadNameBussinessService
    {
        private IRoadNameRepository roadNameRepository = default(IRoadNameRepository);

        public RoadNameBussinessService(IRoadNameRepository roadNameRepository)
        {
            this.roadNameRepository = roadNameRepository;
        }

        public async Task<List<RoadNameDTO>> FetchRoadName()
        {
            return await roadNameRepository.FetchRoadName();
        }

        public MemoryStream GetRoadRoutes(string bbox)
        {
            string[] bboxArr = bbox.Split(',');
            return roadNameRepository.GetRoadRoutes(bboxArr);
        }
    }
}
