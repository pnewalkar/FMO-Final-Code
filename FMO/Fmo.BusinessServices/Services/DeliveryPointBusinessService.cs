using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using System.Net.Http;
using System.IO;

namespace Fmo.BusinessServices.Services
{
    public class DeliveryPointBusinessService : IDeliveryPointBussinessService
    {
        private IDeliveryPointsRepository searchDeliveryPointsRepository = default(IDeliveryPointsRepository);

        public DeliveryPointBusinessService(IDeliveryPointsRepository searchDeliveryPointsRepository)
        {
            this.searchDeliveryPointsRepository = searchDeliveryPointsRepository;
        }

        public List<DeliveryPointDTO> SearchDelievryPoints()
        {
            return searchDeliveryPointsRepository.SearchDeliveryPoints();
        }

        public MemoryStream GetDeliveryPoints(string bbox)
        {
            object[] bboxArr = bbox.Split(',');
            return searchDeliveryPointsRepository.GetDeliveryPoints(bboxArr);
        }
    }
}
