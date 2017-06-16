using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
//using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.Data.DeliveryPoint.WebAPI.DTO;

namespace RM.DataManagement.DeliveryPoint.WebAPI.BusinessService
{
    /// <summary>
    /// This interface contains declaration of methods for fetching Delivery Points data.
    /// </summary>
    public interface IDeliveryPointBusinessService
    {
        /// <summary>
        /// This method is used to fetch delivery points data on the basis of coordinates.
        /// </summary>
        /// <param name="boundaryBox">Boundarybox as string</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>delivery points data as object</returns>
        object GetDeliveryPoints(string boundaryBox, Guid unitGuid);

        /// <summary>
        /// This method is used to fetch delivery points data on the basis of udprn.
        /// </summary>
        /// <param name="udprn">udprn as string</param>
        /// <returns>delivery points data as object</returns>
        object GetDeliveryPointByUDPRN(int udprn);

        /// <summary>
        /// Get coordinates of the delivery point by address Id
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        Task<DeliveryPointDTO> GetDeliveryPointByAddressId(Guid addressId);

        /// <summary>
        /// This method is used to fetch ..........
        /// </summary>
        /// <param name="udprn">udprn as string</param>
        /// <returns>delivery points data as object</returns>
        AddDeliveryPointDTO GetDetailDeliveryPointByUDPRN(int udprn);

        /// <summary>
        /// Fetch the Delivery point for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>Task</returns>
        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery point</returns>
        Task<int> GetDeliveryPointsCount(string searchText, Guid userUnit);

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>string</returns>
        Task<CreateDeliveryPointModelDTO> CreateDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO);

        /// <summary>
        /// This Method is used to Update Delivery Points Co-ordinates.
        /// </summary>
        /// <param name="deliveryPointModelDTO">DeliveryPointDTO</param>
        /// <returns>message</returns>
        Task<UpdateDeliveryPointModelDTO> UpdateDeliveryPointLocation(DeliveryPointModelDTO deliveryPointModelDTO);

        /// <summary>
        /// This Method fetches Route and DPUse for a single DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>KeyValuePair for Route and DPUse</returns>
        List<KeyValuePair<string, string>> GetRouteForDeliveryPoint(Guid deliveryPointId);

        /// <summary>
        /// Check if the delivery point exists for a given UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        Task<bool> DeliveryPointExists(int udprn);

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDto);

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        Task<DeliveryPointDTO> GetDeliveryPointByUDPRNforBatch(int udprn);

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        DeliveryPointDTO GetDeliveryPointByPostalAddress(Guid addressId);

        /// <summary>
        /// Insert the DeliveryPointDTO object into the database
        /// </summary>
        /// <param name="objDeliveryPoint">DeliveryPointDTO object</param>
        /// <returns>boolean value</returns>
        Task<bool> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        /// <summary>
        /// This method updates delivery point access link status
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDto as DTO</param>
        /// <returns>success</returns>
        bool UpdateDeliveryPointAccessLinkCreationStatus(DeliveryPointDTO deliveryPointDTO);

        /// <summary> This method is used to get the delivery points crossing the operational object.
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        List<DeliveryPointDTO> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry accessLink);

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>DeliveryPointDTO</returns>
        DeliveryPointDTO GetDeliveryPoint(Guid deliveryPointGuid);

        /// <summary>
        /// This method updates delivery point location using ID
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        Task<Guid> UpdateDeliveryPointLocationOnID(DeliveryPointDTO deliveryPointDto);

        Task<bool> UpdatePAFIndicator(Guid addressGuid, Guid pafIndicator);
    }
}