using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.DataManagement.NetworkManager.WebAPI.DataDTO;

namespace RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// This interface contains declarations of methods for basic and advance search of street network
    /// </summary>
    public interface IStreetNetworkDataService
    {
        /// <summary>
        /// Fetches the street names for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>List<StreetNameDTO></returns>
        Task<List<StreetNameDataDTO>> GetStreetNamesForBasicSearch(string searchText, Guid unitGuid, string currentUserUnitType);

        /// <summary>
        /// Fetches the street names for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>List<StreetNameDTO></returns>
        Task<List<StreetNameDataDTO>> GetStreetNamesForAdvanceSearch(string searchText, Guid unitGuid, string currentUserUnitType);

        /// <summary>
        /// Gets the street name count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>int</returns>
        Task<int> GetStreetNameCount(string searchText, Guid unitGuid, string currentUserUnitType);

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <param name="referenceDataCategoryList">The reference data category list.</param>
        /// <returns>Nearest street and intersection point.</returns>
        Tuple<NetworkLinkDataDTO, SqlGeometry> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName, List<ReferenceDataCategoryDTO> referenceDataCategoryList);

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="referenceDataCategoryList">The reference data category list.</param>
        /// <returns>Nearest street and intersection point.</returns>
        Tuple<NetworkLinkDataDTO, List<SqlGeometry>> GetNearestSegment(DbGeometry operationalObjectPoint, List<ReferenceDataCategoryDTO> referenceDataCategoryList);

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        NetworkLinkDataDTO GetNetworkLink(Guid networkLinkID);

        /// <summary>
        /// Get the Network Links crossing access link
        /// </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">access link coordinate array</param>
        /// <returns>List<NetworkLinkDTO></returns>
        List<NetworkLinkDataDTO> GetCrossingNetworkLink(string boundingBoxCoordinates, DbGeometry accessLink);
    }
}