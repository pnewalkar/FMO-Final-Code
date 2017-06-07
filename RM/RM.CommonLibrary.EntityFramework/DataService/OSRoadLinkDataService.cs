﻿using System.Linq;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;

using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;
using System;
using RM.CommonLibrary.ResourceFile;
using System.Data.Entity;
using System.Threading.Tasks;

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
                ex.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new SystemException(ErrorMessageIds.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
        }
    }
}