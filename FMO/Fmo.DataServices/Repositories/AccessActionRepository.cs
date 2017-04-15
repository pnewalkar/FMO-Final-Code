namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.MappingConfiguration;
    using Entites=Fmo.Entities;

  public class AccessActionRepository : RepositoryBase<Action, FMODBContext>, IAccessActionRepository
    {
        public AccessActionRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<AccessActionDTO> FetchAccessActions()
        {
            try
            {
              var result = DataContext.Actions.ToList();
                List<AccessActionDTO> accessActionDTO = GenericMapper.MapList<Entites.Action, AccessActionDTO>(result);
                return accessActionDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
