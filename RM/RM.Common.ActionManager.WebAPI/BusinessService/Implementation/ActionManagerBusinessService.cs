using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Common.ActionManager.WebAPI.BusinessService.Interface;
using RM.Common.ActionManager.WebAPI.DataDTO;
using RM.Common.ActionManager.WebAPI.DTO;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.ActionManager.WebAPI.BusinessService
{
    /// <summary>
    /// This class provides methods for getting data from ActionManagerDataService and perform business logic if required
    /// </summary>
    public class ActionManagerBusinessService : IActionManagerBusinessService
    {
        private IActionManagerDataService actionManagerDataService = default(IActionManagerDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.ActionManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.ActionManagerBusinessServiceEntryEventId;
        private int exitEventId = LoggerTraceConstants.ActionManagerBusinessServiceExitEventId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionManagerBusinessService"/> class.
        /// </summary>
        /// <param name="actionManagerDataService"> reference to the ActionManagerDataService</param>
        /// <param name="loggingHelper">reference to the loggingHelper class</param>
        public ActionManagerBusinessService(IActionManagerDataService actionManagerDataService, ILoggingHelper loggingHelper)
        {
            // store injected dependancies.
            this.actionManagerDataService = actionManagerDataService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method fetches role based functions for the current user
        /// </summary>
        /// <param name="userUnitInfo">user unit information</param>
        /// <returns>functions available for current user</returns>
        public async Task<List<RoleAccessDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDTO userUnitInfo)
        {
            string methodName = typeof(ActionManagerBusinessService) + "." + nameof(GetRoleBasedAccessFunctions);
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetRoleBasedAccessFunctions"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                // mapping public DTO to dataDTO
                UserUnitInfoDataDTO userUnitInfoDataDTO = GenericMapper.Map<UserUnitInfoDTO, UserUnitInfoDataDTO>(userUnitInfo);

                var roleAccessDataDto = await actionManagerDataService.GetRoleBasedAccessFunctions(userUnitInfoDataDTO);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                // mapping dataDTO to public DTO
                List<RoleAccessDTO> roleAccessDTO = GenericMapper.MapList<RoleAccessDataDTO, RoleAccessDTO>(roleAccessDataDto);
                return roleAccessDTO;
            }
        }

        /// <summary>
        /// This method fetches Unit information for which user has access
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="locationId">unit id</param>
        /// <returns>unit details</returns>
        public async Task<UserUnitInfoDTO> GetUserUnitInfo(string userName, Guid locationId)
        {
            string methodName = typeof(ActionManagerBusinessService) + "." + nameof(GetUserUnitInfo);
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetUserUnitInfo"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                var userUnitDetails = await actionManagerDataService.GetUserUnitInfo(userName, locationId);

                // Get the Unit details from reference data if current user has access to the units above mail center
                if (userUnitDetails == null)
                {
                    userUnitDetails = await actionManagerDataService.GetUserUnitInfoFromReferenceData(userName, locationId);
                }

                // mapping dataDTO to public DTO
                UserUnitInfoDTO userUnitInfoDTO = GenericMapper.Map<UserUnitInfoDataDTO, UserUnitInfoDTO>(userUnitDetails);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return userUnitInfoDTO;
            }
        }
    }
}