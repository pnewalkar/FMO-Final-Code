using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface ISearchRepository
    {
        AdvanceSearchDTO FetchAdvanceSearchDetails(string searchtext);
    }
}