﻿namespace Fmo.DataServices.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Threading.Tasks;
    using Fmo.DTO;

    /// <summary>
    /// This interface contains declarations of methods for fetching, Insertnig Delivery Points data.
    /// </summary>
    public interface IDeliveryPointsRepository
    {
        /// <summary>
        /// Get the delivery points by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>DeliveryPointDTO object</returns>
        DeliveryPointDTO GetDeliveryPointByUDPRN(int udprn);

        /// <summary>
        /// Update UDPRN of Delivery Point by matching udprn of postal address id.
        /// </summary>
        /// <param name="addressId">Address GUID</param>
        /// <param name="udprn">UDPRN id of postal address</param>
        /// <returns>DeliveryPointDTO object</returns>
        bool UpdateDeliveryPointByAddressId(Guid addressId, int udprn);

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

        /// <summary>
        /// This method is used to fetch Delivery Points as per advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// Task List of Delivery Point Dto
        /// </returns>
        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// This method is used to fetch Delivery Points as per basic search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// Task List of Delivery Point Dto
        /// </returns>
        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// Gets the delivery points count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>int</returns>
        Task<int> GetDeliveryPointsCount(string searchText, Guid unitGuid);

        /// <summary>
        /// This method is used to fetch Delivery Points data.
        /// </summary>
        /// <param name="boundingBox">BoundingBox Coordinates</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List of Delivery Point Dto</returns>
        List<DeliveryPointDTO> GetDeliveryPoints(string boundingBox, Guid unitGuid);

        /// <summary>
        /// Get the list of delivery points by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>DeliveryPointDTO object</returns>
        List<DeliveryPointDTO> GetDeliveryPointListByUDPRN(int udprn);

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        DeliveryPointDTO GetDeliveryPointByPostalAddress(Guid addressId);

        /// <summary>
        /// Check if the delivery point exists for a given UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        bool DeliveryPointExists(int udprn);

        /// <summary>
        /// Calculate the distance between a given point and delivery point
        /// </summary>
        /// <param name="deliveryPointDTO">DeliveryPointDTO object</param>
        /// <param name="newPoint">DbGeometry object</param>
        /// <returns>double?</returns>
        double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint);
    }
}