using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.Entities;
using System.IO;
using System.Data.Entity.Spatial;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryPointsRepository
    {
        //   List<DeliveryPointDTO> SearchDeliveryPoints();

        DeliveryPointDTO GetDeliveryPointByUDPRN(int uDPRN);

        bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDTO);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText);

        Task<int> GetDeliveryPointsCount(string searchText);

        List<DeliveryPointDTO> GetDeliveryPoints(string coordinates);

        IEnumerable<DeliveryPoint> GetData(string coordinates);

        List<DeliveryPointDTO> GetDeliveryPointListByUDPRN(int udprn);

        bool DeliveryPointExists(int uDPRN);

        double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint);

    }
}