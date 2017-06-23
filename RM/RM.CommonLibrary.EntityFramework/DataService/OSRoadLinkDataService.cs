using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// This class contains methods of Access Link DataService for fetching Access Link data.
    /// </summary>
    public class OSRoadLinkDataService : DataServiceBase<OSRoadLink, RMDBContext>, IOSRoadLinkDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public OSRoadLinkDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        public async Task<string> GetOSRoadLink(string toid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetOSRoadLink"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.OSRoadLinkPriority, LoggerTraceConstants.OSRoadLinkDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    var result = await DataContext.OSRoadLinks.Where(x => x.TOID == toid).Select(z => z.RouteHierarchy).SingleOrDefaultAsync();
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.OSRoadLinkPriority, LoggerTraceConstants.OSRoadLinkDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return result;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
                }
            }
        }
    }
}