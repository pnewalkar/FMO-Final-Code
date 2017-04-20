using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface ISearchBusinessService
    {
        Task<SearchResultDTO> FetchBasicSearchDetails(string searchText, Guid userUnit);

        Task<SearchResultDTO> FetchAdvanceSearchDetails(string searchText, Guid userUnit);
    }
}