using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.DataManagement.AccessLink.WebAPI.DTO;

namespace RM.DataManagement.AccessLink.WebAPI.Integration
{
    public interface IAccessLinkIntegrationService
    {
        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        Task<Tuple<NetworkLinkDTO, SqlGeometry>> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName);

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        Task<Tuple<NetworkLinkDTO, SqlGeometry>> GetNearestSegment(DbGeometry operationalObjectPoint);

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        Task<NetworkLinkDTO> GetNetworkLink(Guid networkLinkID);

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames);

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        Task<List<ReferenceDataCategoryDTO>> GetReferenceDataNameValuePairs(List<string> categoryNames);

        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        Task<List<NetworkLinkDTO>> GetCrossingNetworkLinks(string boundingBoxCoordinates, DbGeometry accessLinkCoordinates);

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        Task<string> GetOSRoadLink(string toid);

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>DeliveryPointDTO</returns>
        Task<DeliveryPointDTO> GetDeliveryPoint(Guid deliveryPointGuid);

        /// <summary>
        /// This method updates delivery point access link status
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        Task<bool> UpdateDeliveryPointAccessLinkCreationStatus(DeliveryPointDTO deliveryPointDTO);

        /// <summary> This method is used to get the delivery points crossing an operational object
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        Task<List<DeliveryPointDTO>> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject);
    }
}