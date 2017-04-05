using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.Entities;
using System.IO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryPointsRepository
    {
     //   List<DeliveryPointDTO> SearchDeliveryPoints();

        DeliveryPointDTO GetDeliveryPointByUDPRN(int uDPRN);

        bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        MemoryStream GetDeliveryPoints();
    }
}