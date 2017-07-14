using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Common.ActionManager.WebAPI.BusinessService.Interface;
using RM.Common.ActionManager.WebAPI.DataDTO;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.ActionManager.WebAPI.BusinessService
{
    public class ActionManagerBusinessService : IActionManagerBusinessService
    {
        private IActionManagerDataService actionManagerDataService = default(IActionManagerDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.ActionManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.ActionManagerBusinessServiceEntryEventId;
        private int exitEventId = LoggerTraceConstants.ActionManagerBusinessServiceExitEventId;

        public ActionManagerBusinessService(IActionManagerDataService actionManagerDataService, ILoggingHelper loggingHelper)
        {
            this.actionManagerDataService = actionManagerDataService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method fetches role based functions for the current user
        /// </summary>
        /// <param name="userUnitInfo">user unit information</param>
        /// <returns>functions available for current user</returns>
        public async Task<List<RoleAccessDataDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDataDTO userUnitInfo)
        {
            string methodName = typeof(ActionManagerBusinessService) + "." + nameof(GetRoleBasedAccessFunctions);
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetRoleBasedAccessFunctions"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                var roleAccessDataDto = await actionManagerDataService.GetRoleBasedAccessFunctions(userUnitInfo);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return roleAccessDataDto;
            }
        }

        /// <summary>
        /// This method fetches Unit information for which user has access
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="locationId">unit id</param>
        /// <returns>unit details</returns>
        public async Task<UserUnitInfoDataDTO> GetUserUnitInfo(string userName, Guid locationId)
        {
            string methodName = typeof(ActionManagerBusinessService) + "." + nameof(GetUserUnitInfo);
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetUserUnitInfo"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                var userUnitDetails = await actionManagerDataService.GetUserUnitInfo(userName, locationId);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return userUnitDetails;
            }
        }

        /// <summary>
        /// This method fetches information for units above mail center for the current user
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="locationId">unit id</param>
        /// <returns>unit details</returns>
        public async Task<UserUnitInfoDataDTO> GetUserUnitInfoFromReferenceData(string userName, Guid locationId)
        {
            string methodName = typeof(ActionManagerBusinessService) + "." + nameof(GetUserUnitInfoFromReferenceData);
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetUserUnitInfoFromReferenceData"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                var userUnitDetails = await actionManagerDataService.GetUserUnitInfoFromReferenceData(userName, locationId);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return userUnitDetails;
            }
        }
    }
}