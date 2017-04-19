namespace Fmo.DataServices.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Threading.Tasks;
    using Fmo.DTO;
    using Fmo.Entities;

    public interface IDeliveryPointsRepository
    {
        /// <summary>
        /// Get the delivery points by the UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>DeliveryPointDTO object</returns>
        DeliveryPointDTO GetDeliveryPointByUDPRN(int uDPRN);

        /// <summary>
        /// Insert the DeliveryPointDTO object into the database
        /// </summary>
        /// <param name="objDeliveryPoint">DeliveryPointDTO object</param>
        /// <returns>boolean value</returns>
        bool InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        /// <summary>
        /// Update the delivery point based on the UDPRN id
        /// </summary>
        /// <param name="deliveryPointDTO">DeliveryPointDTO object</param>
        /// <returns>Task<int></returns>
        Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDTO);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText);

        Task<int> GetDeliveryPointsCount(string searchText);

        List<DeliveryPointDTO> GetDeliveryPoints(string coordinates);

        IEnumerable<DeliveryPoint> GetData(string coordinates);

        List<DeliveryPointDTO> GetDeliveryPointListByUDPRN(int udprn);

        /// <summary>
        /// Check if the delivery point exists for a given UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>boolean value</returns>
        bool DeliveryPointExists(int uDPRN);

        /// <summary>
        /// Calculate the distance between a given point and delivery point
        /// </summary>
        /// <param name="deliveryPointDTO">DeliveryPointDTO object</param>
        /// <param name="newPoint">DbGeometry object</param>
        /// <returns>double?</returns>
        double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint);
    }
}