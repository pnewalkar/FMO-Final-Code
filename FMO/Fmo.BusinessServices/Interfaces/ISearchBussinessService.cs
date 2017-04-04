using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface ISearchBussinessService
    {
        Task<SearchResultDTO> FetchBasicSearchDetails(string searchText);

        Task<SearchResultDTO> FetchAdvanceSearchDetails(string searchText);
    }
}