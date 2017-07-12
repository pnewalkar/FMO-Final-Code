using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RM.Common.ActionManager.WebAPI.DTO;
using RM.Common.ActionManager.WebAPI.Entity;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.Common.ActionManager.WebAPI.DataService
{
    public class ActionManagerDataService : DataServiceBase<AccessFunction, ActionDBContext>, IActionManagerDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public ActionManagerDataService(IDatabaseFactory<ActionDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method fetches role based function for the current user
        /// </summary>
        /// <param name="userUnitInfo"></param>
        /// <returns></returns>
        public async Task<List<RoleAccessDataDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDataDTO userUnitInfo)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRoleBasedAccessFunctions"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ActionManagerAPIPriority, LoggerTraceConstants.ActionManagerDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var roleAccessDataDto = await DataContext.AccessFunctions.AsNoTracking()
                .Where(x => x.UserName.Equals(userUnitInfo.UserName) && x.LocationID.Equals(userUnitInfo.LocationId))
                .Select(x => new RoleAccessDataDTO
                {
                    RoleName = x.RoleName,
                    Unit_GUID = x.LocationID,
                    UserName = x.UserName,
                    FunctionName = x.FunctionName,
                    ActionName = x.ActionName,
                    UserId = x.UserId
                }).ToListAsync();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ActionManagerAPIPriority, LoggerTraceConstants.ActionManagerDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return roleAccessDataDto;
            }
        }

        /// <summary>
        /// This method fetches units for which user has access
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<Guid> GetUserUnitInfo(string userName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetUserUnitInfo"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ActionManagerAPIPriority, LoggerTraceConstants.UserRoleUnitDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var userUnit = await (from r in DataContext.UserRoleLocations.AsNoTracking()
                                      join u in DataContext.Users on r.UserID equals u.ID
                                      where u.UserName == userName
                                      select r.LocationID).FirstOrDefaultAsync();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ActionManagerAPIPriority, LoggerTraceConstants.UserRoleUnitDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return userUnit;
            }
        }
    }
}