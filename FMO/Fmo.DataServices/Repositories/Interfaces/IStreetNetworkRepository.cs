using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Fmo.DTO;
using Microsoft.SqlServer.Types;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// This interface contains declarations of methods for basic and advance search of street network
    /// </summary>
    public interface IStreetNetworkRepository
    {
        /// <summary>
        /// Fetches the street names for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List<StreetNameDTO></returns>
        Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// Fetches the street names for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List<StreetNameDTO></returns>
        Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText, Guid unitGuid);

        /// <summary>
        /// Gets the street name count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>int</returns>
        Task<int> GetStreetNameCount(string searchText, Guid unitGuid);

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <param name="referenceDataCategoryList"></param>
        /// <returns>Nearest street and intersection point.</returns>
        Tuple<NetworkLinkDTO, SqlGeometry> GetNearestNamedRoadForOperationalObject(DbGeometry operationalObjectPoint, string streetName, List<ReferenceDataCategoryDTO> referenceDataCategoryList);

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="referenceDataCategoryList"></param>
        /// <returns>Nearest street and intersection point.</returns>
        Tuple<NetworkLinkDTO, SqlGeometry> GetNearestRoadForOperationalObject(DbGeometry operationalObjectPoint, List<ReferenceDataCategoryDTO> referenceDataCategoryList);
    }
}