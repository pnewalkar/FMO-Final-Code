namespace Fmo.DataServices.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Threading.Tasks;
    using Fmo.DTO;
    using Fmo.Entities;

    /// <summary>
    /// This class methods for fetching, Insertnig Delivery Points data.
    /// </summary>
    public interface IDeliveryPointsRepository
    {
        DeliveryPointDTO GetDeliveryPointByUDPRN(int uDPRN);

        bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDTO);

        /// <summary>
        /// This method is used to fetch Delivery Points as per advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText);

        Task<int> GetDeliveryPointsCount(string searchText);

        /// <summary>
        /// This method is used to fetch Delivery Points data.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>List of Delivery Point Dto</returns>
        List<DeliveryPointDTO> GetDeliveryPoints(string coordinates);

        /// <summary>
        /// This method is used to fetch delivery points data as per coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>Ienumerable of Delivery Point Dto</returns>
        IEnumerable<DeliveryPoint> GetData(string coordinates);

        List<DeliveryPointDTO> GetDeliveryPointListByUDPRN(int udprn);

        bool DeliveryPointExists(int uDPRN);

        double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint);
    }
}