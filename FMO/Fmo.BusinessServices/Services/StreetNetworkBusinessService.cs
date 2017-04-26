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
   public class StreetNetworkBusinessService : IStreetNetworkBusinessService
    {
        private IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);

        public StreetNetworkBusinessService(IStreetNetworkRepository streetNetworkRepository)
        {
            this.streetNetworkRepository = streetNetworkRepository;
        }
    }
}
