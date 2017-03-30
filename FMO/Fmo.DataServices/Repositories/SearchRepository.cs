using System;
using System.Collections.Generic;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    public class SearchRepository : RepositoryBase<AdvanceSearch, FMODBContext>, ISearchRepository
    {
        public SearchRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<AdvanceSearch> FetchAdvanceSearchDetails()
        {
            try
            {
               // var result = DataContext.AdvanceSearch.ToList();
                return new List<AdvanceSearch>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}