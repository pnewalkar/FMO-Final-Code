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
    /// DataService for User Role and Unit information
    /// </summary>
    /// <seealso cref="Fmo.DataServices.Infrastructure.DataServiceBase{RM.CommonLibrary.EntityFramework.Entities.UserRoleUnit, Fmo.DataServices.DBContext.RMDBContext}"/>
    /// <seealso cref="RM.CommonLibrary.EntityFramework.DataService.Interfaces.IUserRoleUnitDataService"/>
    public class UserRoleUnitDataService : DataServiceBase<UserRoleUnit, RMDBContext>, IUserRoleUnitDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public UserRoleUnitDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Gets the user unit information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Guid</returns>
        public async Task<Guid> GetUserUnitInfo(string userName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetUserUnitInfo"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ActionManagerAPIPriority, LoggerTraceConstants.UserRoleUnitDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var userUnit = await (from r in DataContext.UserRoleUnits.AsNoTracking()
                                      join u in DataContext.Users on r.User_GUID equals u.ID
                                      where u.UserName == userName
                                      select r.Unit_GUID).FirstOrDefaultAsync();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ActionManagerAPIPriority, LoggerTraceConstants.UserRoleUnitDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return userUnit;
            }
        }
    }
}