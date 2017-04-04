using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.Entities;
using System.IO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryPointsRepository
    {
        Task<List<DeliveryPointDTO>> SearchDeliveryPoints();

        DeliveryPoint GetDeliveryPointByUDPRN(int uDPRN);

        bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        MemoryStream GetDeliveryPoints(object[] parameters);
    }
}