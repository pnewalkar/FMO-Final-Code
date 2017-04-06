namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    public class StreetNetworkRepository : RepositoryBase<StreetName, FMODBContext>, IStreetNetworkRepository
    {
        public StreetNetworkRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public Task<List<StreetNameDTO>> FetchStreetNamesforAdvanceSearch(string searchText)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StreetNameDTO>> FetchStreetNamesforBasicSearch(string searchText)
        {
            try
            {
                int takeCount = 5;
                var streetNames = await DataContext.StreetNames.Where(l => l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText))
                    .Take(takeCount).ToListAsync();
                return GenericMapper.MapList<StreetName, StreetNameDTO>(streetNames.ToList());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetStreetNameCount(string searchText)
        {
            try
            {
                return await DataContext.StreetNames.Where(l => l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText))
                    .CountAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}