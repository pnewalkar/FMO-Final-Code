namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DTO;
    using Fmo.MappingConfiguration;
    using Infrastructure;
    using Interfaces;
    using Entites = Fmo.Entities;

    /// <summary>
    /// This class contains for methods for fetching access action items.
    /// </summary>
    public class AccessActionRepository : RepositoryBase<Action, FMODBContext>, IAccessActionRepository
    {
        public AccessActionRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This metod is used to fetch Access action items.
        /// </summary>
        /// <returns>List of Access Action Dto</returns>
        public List<AccessActionDTO> FetchAccessActions()
        {
            try
            {
                var result = DataContext.Actions.ToList();
                List<AccessActionDTO> accessActionDTO = GenericMapper.MapList<Entites.Action, AccessActionDTO>(result);
                return accessActionDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}