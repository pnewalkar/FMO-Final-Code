using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// This class contains methods of Access Link DataService for fetching Access Link data.
    /// </summary>
    public class OSRoadLinkDataService : DataServiceBase<OSRoadLink, RMDBContext>, IOSRoadLinkDataService
    {
        public OSRoadLinkDataService(IDatabaseFactory<RMDBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        public async Task<string> GetOSRoadLink(string toid)
        {
            try
            {
                var result = await DataContext.OSRoadLinks.Where(x => x.TOID == toid).Select(z => z.RouteHierarchy).SingleOrDefaultAsync();
                return result;
            }
            catch (InvalidOperationException ex)
            {
                ex.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
        }
    }
}