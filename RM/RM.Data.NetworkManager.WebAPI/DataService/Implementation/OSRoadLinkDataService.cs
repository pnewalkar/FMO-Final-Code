using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.NetworkManager.WebAPI.Entities;

namespace RM.DataManagement.NetworkManager.WebAPI.DataService.Implementation
{
    /// <summary>
    /// This class contains methods of OS Road Link DataService.
    /// </summary>
    public class OSRoadLinkDataService : DataServiceBase<OSRoadLink, NetworkDBContext>, IOSRoadLinkDataService
    {
        #region Member Variables

        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.NetworkManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.OSRoadLinkDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.OSRoadLinkDataServiceMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public OSRoadLinkDataService(IDatabaseFactory<NetworkDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        public async Task<string> GetOSRoadLink(string toid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetOSRoadLink"))
            {
                string methodName = typeof(OSRoadLinkDataService) + "." + nameof(GetOSRoadLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    var result = await DataContext.OSRoadLinks.Where(x => x.TOID == toid).Select(z => z.RouteHierarchy).SingleOrDefaultAsync();

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return result;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
                }
            }
        }

        #endregion Public Methods
    }
}