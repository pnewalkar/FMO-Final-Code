using System.Collections.Generic;
using Fmo.Entities;

namespace Fmo.BusinessServices.Interfaces
{
    public interface ISearchBussinessService
    {
        List<AdvanceSearch> FetchAdvanceSearchDetails();
    }
}