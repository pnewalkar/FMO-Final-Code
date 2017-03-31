using System.Collections.Generic;
using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryPointsRepository
    {
        List<DeliveryPointDTO> SearchDeliveryPoints();

        DeliveryPoint GetDeliveryPointByUDPRN(int uDPRN);

        bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);
    }
}