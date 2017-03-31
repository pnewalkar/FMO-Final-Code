using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services 
{
   public class RoadNameBussinessService : IRoadNameBussinessService
    {
        private IRoadNameRepository roadNameRepository = default(IRoadNameRepository);

        public RoadNameBussinessService(IRoadNameRepository _roadNameRepository)
        {
            this.roadNameRepository = _roadNameRepository;
        }

        public List<RoadNameDTO> FetchRoadName()
        {
            return roadNameRepository.FetchRoadName();
        }
    }
}
