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

        Task<int> UpdateDeliveryPointLocationOnUDPRN(int uDPRN, decimal latitude, decimal longitude, DbGeometry locationXY);

        MemoryStream GetDeliveryPoints();

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText);

        Task<int> GetDeliveryPointsCount(string searchText);
    }
}