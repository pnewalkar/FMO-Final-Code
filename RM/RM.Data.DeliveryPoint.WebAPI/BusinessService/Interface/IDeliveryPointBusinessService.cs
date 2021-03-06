﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using RM.Data.DeliveryPoint.WebAPI.DTO;
using RM.Data.DeliveryPoint.WebAPI.DTO.Model;

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
        /// <param name="currentUserUnitType">current user unit type.</param>
        /// <returns>delivery points data as object</returns>
        object GetDeliveryPoints(string boundaryBox, Guid unitGuid, string currentUserUnitType);

        /// <summary>
        /// This method is used to fetch delivery points data on the basis of Guid.
        /// </summary>
        /// <param name="Guid">Guid as unique Identifier</param>
        /// <returns>delivery points data as object</returns>
        object GetDeliveryPointByGuid(Guid id);

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
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        Task<List<DeliveryPointDTO>> GetDeliveryPointsForBasicSearch(string searchText, Guid userUnit, string currentUserUnitType);

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>The total count of delivery point</returns>
        Task<int> GetDeliveryPointsCount(string searchText, Guid userUnit, string currentUserUnitType);

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        Task<List<DeliveryPointDTO>> GetDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid, string currentUserUnitType);

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
        /// <returns>Unique identifier of delivery point.</returns>
        Task<Guid> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint);

        ///// <summary>
        ///// This method updates delivery point access link status
        ///// </summary>
        ///// <param name="deliveryPointDTO">deliveryPointDto as DTO</param>
        ///// <returns>success</returns>
        // bool UpdateDeliveryPointAccessLinkCreationStatus(DeliveryPointDTO deliveryPointDTO);

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

        Task<DeliveryPointDTO> GetDeliveryPointByPostalAddressWithLocation(Guid addressId);

        Task<bool> DeleteDeliveryPoint(Guid id);

        Task<CreateDeliveryPointForRangeModelDTO> CheckDeliveryPointForRange(AddDeliveryPointDTO addDeliveryPointDTO);

        Task<CreateDeliveryPointForRangeModelDTO> CreateDeliveryPointForRange(List<PostalAddressDTO> postalAddressDTOs);

        Task<bool> UpdateDPUse(PostalAddressDTO postalAddressDetails);

        /// <summary>
        /// Deletes a delivery point with all associated location offerings and locations.
        /// </summary>
        /// <param name="deliveryPointid">Delivery point unique identifier.</param>
        /// <param name="reasonCode">reason code</param>
        /// <param name="reasonText">reasonText</param>
        /// <param name="userName">userName</param>
        /// <returns>bool</returns>
        Task<bool> DeleteDeliveryPointWithAssociatedLocations(Guid deliveryPointid, string reasonCode, string reasonText, string userName);

        Task<List<UpdateDeliveryPointModelDTO>> UpdateDeliveryPointLocationForRange(List<DeliveryPointModelDTO> deliveryPointModelDTOs);
    }
}