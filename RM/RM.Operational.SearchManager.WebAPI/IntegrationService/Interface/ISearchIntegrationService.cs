using System.Collections.Generic;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Operational.SearchManager.WebAPI.Integration
{
    public interface ISearchIntegrationService
    {
        // Basic Search & Count
        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText);

        Task<int> GetDeliveryRouteCount(string searchText);

        Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText);

        Task<int> GetPostCodeUnitCount(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText);

        Task<int> GetDeliveryPointsCount(string searchText);

        Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText);

        Task<int> GetStreetNameCount(string searchText);

        // Advance Search
        Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText);

        Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText);

        Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText);

        Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText);
    }
}