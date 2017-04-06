using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IDeliveryPointsRepository
    {
        //   List<DeliveryPointDTO> SearchDeliveryPoints();

        DeliveryPointDTO GetDeliveryPointByUDPRN(int uDPRN);

        bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        MemoryStream GetDeliveryPoints();

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText);

        Task<int> GetDeliveryPointsCount(string searchText);

        List<DeliveryPointDTO> GetDeliveryPoints1(string coordinates);

        IEnumerable<DeliveryPoint> GetData(string coordinates);
    }
}