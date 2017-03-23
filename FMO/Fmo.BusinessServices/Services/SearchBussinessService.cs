using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;

namespace Fmo.BusinessServices.Services
{
    public class SearchBussinessService : ISearchBussinessService
    {

        ISearchRepository searchRepository = default(ISearchRepository);

        public SearchBussinessService(ISearchRepository _searchRepository)
        {
            this.searchRepository = _searchRepository;
        }

        public List<AdvanceSearch> FetchAdvanceSearchDetails()
        {
            return searchRepository.FetchAdvanceSearchDetails();
        }
    }
}
