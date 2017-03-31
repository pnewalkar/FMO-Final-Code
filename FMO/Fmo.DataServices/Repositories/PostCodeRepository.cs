
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

      public class PostCodeRepository : RepositoryBase<Postcode, FMODBContext>, IPostCodeRepository
    {

        public PostCodeRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<PostCodeDTO> FetchPostCodeUnit(string searchText)
        {
            try
            {
                var result = DataContext.Postcodes.ToList();
                return GenericMapper.MapList<Postcode, PostCodeDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
