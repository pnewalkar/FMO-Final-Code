using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;

namespace Fmo.BusinessServices.Services
{
    public class DeliveryPointBusinessService : IDeliveryPointBussinessService
    {
        ISearchDeliveryPointsRepository searchDeliveryPointsRepository = default(ISearchDeliveryPointsRepository);

        public DeliveryPointBusinessService(ISearchDeliveryPointsRepository _searchDeliveryPointsRepository)
        {
            this.searchDeliveryPointsRepository = _searchDeliveryPointsRepository;
        }

        public List<DeliveryPoint> SearchDelievryPoints()
        {
            return searchDeliveryPointsRepository.SearchDelievryPoints();
        }
    }
}
