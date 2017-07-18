using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.DataManagement.SearchManager.WebAPI.DTO;

namespace RM.Operational.SearchManager.WebAPI.Integration
{
    public interface ISearchIntegrationService
    {
        // Basic Search & Count
        Task<List<RouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText);

        Task<int> GetDeliveryRouteCount(string searchText);

        Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText);

        Task<int> GetPostCodeUnitCount(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText);

        Task<int> GetDeliveryPointsCount(string searchText);

        Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText);

        Task<int> GetStreetNameCount(string searchText);

        // Advance Search
        Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText);

        Task<List<RouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText);

        Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText);

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> categoryNames);

        /// <summary>
        /// Fetches unit Location type id for current user
        /// </summary>
        /// <returns>Guid</returns>
        Task<Guid> GetUnitLocationTypeId(Guid unitId);
    }
}