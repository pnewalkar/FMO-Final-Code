namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public List<StreetNameDTO> FetchStreetNetwork(string searchText)
        {
            try
            {
                var result = DataContext.StreetNames.ToList();
                return GenericMapper.MapList<StreetName, StreetNameDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
