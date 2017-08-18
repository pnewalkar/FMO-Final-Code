using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using RM.Data.DeliveryPoint.WebAPI.DataDTO;

namespace RM.DataManagement.DeliveryPoint.WebAPI.DataService
{
    /// <summary>
    /// This interface contains declarations of methods for fetching, Insertnig Delivery Points data.
    /// </summary>
    public interface IDeliveryPointsDataService
    {
        /// <summary>
        /// Get the delivery points by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>Delivery point count that matches the criteria.</returns>
        Task<DeliveryPointDataDTO> GetDeliveryPointByUDPRN(int udprn);

        /// <summary>
        /// Insert the DeliveryPointDTO object into the database
        /// </summary>
        /// <param name="objDeliveryPoint">DeliveryPointDTO object</param>
        /// <returns>Unique identifier of delivery point.</returns>
        Task<Guid> InsertDeliveryPoint(DeliveryPointDataDTO objDeliveryPoint);

        /// <summary> Update the delivery point based on the UDPRN id </summary> <param
        /// name="deliveryPointDto">DeliveryPointDTO object</param> <returns>Task<Guid></returns>
        Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDataDTO deliveryPointDto);

        /// <summary>
        /// This method is used to fetch Delivery Points as per advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>Collection of Delivery Points that matches the criteria.</returns>
        Task<List<DeliveryPointDataDTO>> GetDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid, string currentUserUnitType);

        /// <summary>
        /// This method is used to fetch Delivery Points as per basic search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="numberOfRecordRequested">Number of records to be returned.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>Collection of Delivery Points that matches the criteria.</returns>
        Task<List<DeliveryPointDataDTO>> GetDeliveryPointsForBasicSearch(string searchText, int numberOfRecordRequested, Guid unitGuid, string currentUserUnitType);

        /// <summary>
        /// Gets the delivery points count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>Delivery point count that matches the criteria.</returns>
        Task<int> GetDeliveryPointsCount(string searchText, Guid unitGuid, string currentUserUnitType);

        /// <summary>
        /// This method is used to fetch Delivery Points data.
        /// </summary>
        /// <param name="boundingBox">BoundingBox Coordinates</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">current user unit type.</param>
        /// <returns>Collection of Delivery Points that matches the criteria.</returns>
        List<DeliveryPointDataDTO> GetDeliveryPoints(string boundingBoxCoordinates, Guid unitGuid, string currentUserUnitType);

        /// <summary>
        /// Get the list of delivery points by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>The delivery point that matches the criteria, or null if a match does not exist</returns>
        List<DeliveryPointDataDTO> GetDeliveryPointListByUDPRN(int udprn);

        /// <summary>
        /// Get the list of delivery points by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>DeliveryPointDTO object</returns>
        RM.Data.DeliveryPoint.WebAPI.DTO.AddDeliveryPointDTO GetDetailDeliveryPointByUDPRN(int udprn);

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        DeliveryPointDataDTO GetDeliveryPointByPostalAddress(Guid addressId);

        /// <summary>
        /// Check if the delivery point exists for a given UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        Task<bool> DeliveryPointExists(int udprn);

        /// <summary>
        /// Calculate the distance between a given point and delivery point
        /// </summary>
        /// <param name="deliveryPointDTO">DeliveryPointDTO object</param>
        /// <param name="newPoint">DbGeometry object</param>
        /// <returns>Distance from the provided loacation point.</returns>
        double? GetDeliveryPointDistance(DeliveryPointDataDTO deliveryPointDTO, DbGeometry newPoint);

        /// <summary>
        /// Get the delivery point row version
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>byte[]</returns>
        byte[] GetDeliveryPointRowVersion(Guid id);

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>The delivery point that matches the criteria, or null if a match does not exist.</returns>
        DeliveryPointDataDTO GetDeliveryPoint(Guid deliveryPointGuid);

        /// <summary> This method is used to get the delivery points crossing the created operational
        /// object </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">The collection of delivery points that matches the criteria, or null if a match does not exist</returns>
        List<DeliveryPointDataDTO> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject);

        /// <summary> Update the delivery point based on the id </summary> <param
        /// name="deliveryPointDto">DeliveryPointDTO object</param> <returns>Task<Guid></returns>
        Task<Guid> UpdateDeliveryPointLocationOnID(DeliveryPointDataDTO deliveryPointDto);

        Task<bool> UpdatePAFIndicator(Guid addressGuid, Guid pafIndicator);

        /// <summary>
        /// Gets delivery point with address that has location.
        /// </summary>
        /// <param name="addressId">The address unique identifier.</param>
        /// <returns>The delivery point that matches the criteria, or null if a match does not exist</returns>
        Task<DeliveryPointDataDTO> GetDeliveryPointByPostalAddressWithLocation(Guid addressId);

        /// <summary>
        /// Delets a delivery point.
        /// </summary>
        /// <param name="id">Delivery point unique identifier.</param>
        /// <returns>Boolean value indicating the success of delete operation.</returns>
        Task<bool> DeleteDeliveryPoint(Guid id);

        /// <summary>
        /// User Deletes a delivery point.
        /// </summary>
        /// <param name="id">Delivery point unique identifier.</param>
        /// <returns>Boolean value indicating the success of delete operation.</returns>
        Task<bool> UserDeleteDeliveryPoint(Guid id, bool isUserDelete);

        Task<bool> UpdateDPUse(int udprn, Guid deliveryPointUseIndicatorGUID);
    }
}