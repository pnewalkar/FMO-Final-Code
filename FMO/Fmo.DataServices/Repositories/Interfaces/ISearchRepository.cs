using System.Collections.Generic;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface ISearchRepository
    {
        List<AdvanceSearch> FetchAdvanceSearchDetails();
    }
}