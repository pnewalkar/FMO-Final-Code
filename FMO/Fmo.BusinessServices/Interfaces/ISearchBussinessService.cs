using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface ISearchBussinessService
    {
       AdvanceSearchDTO FetchAdvanceSearchDetails(string searchText);
    }
}