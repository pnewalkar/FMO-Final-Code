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

        public async Task<List<StreetNameDTO>> FetchStreetNamesforBasicSearch(string searchText)
        {
            try
            {
                int takeCount = 5;
                var streetNamesDto = await DataContext.StreetNames.Where(l => l.NationalRoadCode.StartsWith(searchText) || l.DesignatedName.StartsWith(searchText))
                    .Take(takeCount)
                    .Select(l => new StreetNameDTO
                    {
                        StreetName_Id = l.StreetName_Id,
                        StreetType = l.StreetType,
                        NationalRoadCode = l.NationalRoadCode,
                        DesignatedName = l.DesignatedName,
                        Descriptor = l.Descriptor
                    })
                    .ToListAsync();

                return streetNamesDto;
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }
    }
}
