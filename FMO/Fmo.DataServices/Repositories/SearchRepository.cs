using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Entities;

namespace Fmo.DataServices.Repositories
{
    public class SearchRepository : RepositoryBase<AdvanceSearch, FMODBContext>, ISearchRepository
    {

        public SearchRepository(IDatabaseFactory<FMODBContext> databaseFactory) : base(databaseFactory)
        {
        }

        public List<AdvanceSearch> FetchAdvanceSearchDetails()
        {
            try
            {
                var result = DataContext.AdvanceSearch.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
