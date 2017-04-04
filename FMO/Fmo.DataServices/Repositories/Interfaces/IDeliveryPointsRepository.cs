using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryPointsRepository
    {
        DeliveryPoint GetDeliveryPointByUDPRN(int uDPRN);

        bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);
    }
}