using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;

using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class ActionManagerDataService : DataServiceBase<AccessFunction, RMDBContext>, IActionManagerDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public ActionManagerDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        public async Task<List<RoleAccessDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDTO userUnitInfo)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRoleBasedAccessFunctions"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ActionManagerAPIPriority, LoggerTraceConstants.ActionManagerDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var roleAccessDto = await DataContext.AccessFunctions.AsNoTracking()
                .Where(x => x.UserName.Equals(userUnitInfo.UserName) && x.Unit_GUID.Equals(userUnitInfo.UnitGuid))
                .Select(x => new RoleAccessDTO
                {
                    RoleName = x.RoleName,
                    Unit_GUID = x.Unit_GUID,
                    UserName = x.UserName,
                    FunctionName = x.FunctionName,
                    ActionName = x.ActionName,
                    UserId = x.UserId
                }).ToListAsync();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ActionManagerAPIPriority, LoggerTraceConstants.ActionManagerDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return roleAccessDto;
            }
        }
    }
}