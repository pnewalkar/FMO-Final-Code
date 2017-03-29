using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;

namespace Fmo.BusinessServices.Services
{
    public class SearchBussinessService : ISearchBussinessService
    {
        private ISearchRepository searchRepository = default(ISearchRepository);

        public SearchBussinessService(ISearchRepository searchRepository)
        {
            this.searchRepository = searchRepository;
        }

        public List<AdvanceSearch> FetchAdvanceSearchDetails()
        {
            return searchRepository.FetchAdvanceSearchDetails();
        }
    }
}