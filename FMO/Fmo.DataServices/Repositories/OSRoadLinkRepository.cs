using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.Common.Constants;
using Fmo.Common.ExceptionManagement;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// This class contains methods of Access Link Repository for fetching Access Link data.
    /// </summary>
    public class OSRoadLinkRepository : RepositoryBase<OSRoadLink, FMODBContext>, IOSRoadLinkRepository
    {
        public OSRoadLinkRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        public string GetOSRoadLink(string toid)
        {
            try
            {
                var result = DataContext.OSRoadLinks.Where(x => x.TOID == toid).Select(z => z.RouteHierarchy).SingleOrDefault();
                return result;
            }
            catch (InvalidOperationException ex)
            {
                throw new SystemException(ErrorMessageConstants.InvalidOperationExceptionMessageForSingleorDefault, ex);
            }
        }
    }
}
