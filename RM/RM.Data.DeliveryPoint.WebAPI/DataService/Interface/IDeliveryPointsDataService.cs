using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using RM.Data.DeliveryPoint.WebAPI.DTO;

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
        /// <returns>DeliveryPointDTO object</returns>
        Task<DeliveryPointDataDTO> GetDeliveryPointByUDPRN(int udprn);

        /// <summary>
        /// Insert the DeliveryPointDTO object into the database
        /// </summary>
        /// <param name="objDeliveryPoint">DeliveryPointDTO object</param>
        /// <returns>boolean value</returns>
        Task<bool> InsertDeliveryPoint(DeliveryPointDataDTO objDeliveryPoint);

        /// <summary> Update the delivery point based on the UDPRN id </summary> <param
        /// name="deliveryPointDto">DeliveryPointDTO object</param> <returns>Task<Guid></returns>
        Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDataDTO deliveryPointDto);

        /// <summary>
        /// This method is used to fetch Delivery Points as per advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        Task<List<DeliveryPointDataDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// This method is used to fetch Delivery Points as per basic search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        Task<List<DeliveryPointDataDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid unitGuid);

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
        List<DeliveryPointDataDTO> GetDeliveryPoints(string boundingBoxCoordinates, Guid unitGuid);

        /// <summary>
        /// Get the list of delivery points by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>DeliveryPointDTO object</returns>
        List<DeliveryPointDataDTO> GetDeliveryPointListByUDPRN(int udprn);

        /// <summary>
        /// Get the list of delivery points by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>DeliveryPointDTO object</returns>
        AddDeliveryPointDTO GetDetailDeliveryPointByUDPRN(int udprn);

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
        /// <returns>double?</returns>
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
        /// <returns>DeliveryPointDTO</returns>
        DeliveryPointDataDTO GetDeliveryPoint(Guid deliveryPointGuid);
               
        /// <summary>
        /// This Method provides Route Name for a single DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>Route Name for a single DeliveryPoint</returns>
        string GetRouteForDeliveryPoint(Guid deliveryPointId);

        /// <summary>
        /// This Method fetches DPUse value for the DeliveryPoint
        /// </summary>
        /// <param name="referenceDataCategoryDtoList">
        /// referenceDataCategoryDtoList as List of ReferenceDataCategoryDTO
        /// </param>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>DPUse value as string</returns>
        string GetDPUse(Guid deliveryPointId, Guid operationalObjectTypeForDpOrganisation, Guid operationalObjectTypeForDpResidential);

        /// <summary> This method is used to get the delivery points crossing the created operational
        /// object </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        List<DeliveryPointDataDTO> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject);

        /// <summary> Update the delivery point based on the id </summary> <param
        /// name="deliveryPointDto">DeliveryPointDTO object</param> <returns>Task<Guid></returns>
        Task<Guid> UpdateDeliveryPointLocationOnID(DeliveryPointDataDTO deliveryPointDto);

        Task<bool> UpdatePAFIndicator(Guid addressGuid, Guid pafIndicator);

        Task<DeliveryPointDataDTO> GetDeliveryPointByPostalAddressWithLocation(Guid addressId);

        Task<bool> DeleteDeliveryPoint(Guid id);
    }
}