using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RM.Common.ActionManager.WebAPI.DataDTO;
using RM.Common.ActionManager.WebAPI.Entity;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.ActionManager.WebAPI.DataService
{
    /// <summary>
    /// This class provides methods for fetching data required for Action Manager
    /// </summary>
    public class ActionManagerDataService : DataServiceBase<AccessFunction, ActionDBContext>, IActionManagerDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.ActionManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.ActionManagerDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.ActionManagerDataServiceMethodExitEventId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionManagerDataService"/> class.
        /// </summary>
        /// <param name="databaseFactory"> reference to the databaseFactory</param>
        /// <param name="loggingHelper">reference to the loggingHelper</param>
        public ActionManagerDataService(IDatabaseFactory<ActionDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method fetches role based functions for the current user
        /// </summary>
        /// <param name="userUnitInfo">user unit information</param>
        /// <returns>functions available for current user</returns>
        public async Task<List<RoleAccessDataDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDataDTO userUnitInfo)
        {
            string methodName = typeof(ActionManagerDataService) + "." + nameof(GetRoleBasedAccessFunctions);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRoleBasedAccessFunctions"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var roleAccessDataDto = await DataContext.AccessFunctions.AsNoTracking()
                .Where(x => x.UserName.Equals(userUnitInfo.UserName) && x.LocationID.Equals(userUnitInfo.LocationId))
                .Select(x => new RoleAccessDataDTO
                {
                    RoleName = x.RoleName,
                    Unit_GUID = x.LocationID,
                    UserName = x.UserName,
                    FunctionName = x.FunctionName,
                    ActionName = x.ActionName,
                    UserId = x.UserId,
                    UnitType = userUnitInfo.UnitType,
                    UnitName = userUnitInfo.UnitName
                }).ToListAsync();

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
            string methodName = typeof(ActionManagerDataService) + "." + nameof(GetUserUnitInfo);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetUserUnitInfo"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var userUnitDetails = await (from r in DataContext.UserRoleLocations.AsNoTracking()
                                             join u in DataContext.Users on r.UserID equals u.ID
                                             join p in DataContext.PostalAddressIdentifiers on r.LocationID equals p.ID
                                             join rd in DataContext.ReferenceDatas on p.IdentifierTypeGUID equals rd.ID
                                             where (u.UserName == userName && r.LocationID == locationId) || u.UserName == userName
                                             select new UserUnitInfoDataDTO
                                             {
                                                 LocationId = r.LocationID,
                                                 UnitName = p.Name,
                                                 UnitType = rd.ReferenceDataValue
                                             }).FirstOrDefaultAsync();
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
            string methodName = typeof(ActionManagerDataService) + "." + nameof(GetUserUnitInfoFromReferenceData);
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetUserUnitFromReferenceData"))
            {
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var userUnitDetails = await (from r in DataContext.UserRoleLocations.AsNoTracking()
                                             join u in DataContext.Users on r.UserID equals u.ID
                                             join l in DataContext.LocationReferenceDatas on r.LocationID equals l.LocationID
                                             join rd in DataContext.ReferenceDatas on l.ReferenceDataID equals rd.ID
                                             where (u.UserName == userName && r.LocationID == locationId) || u.UserName == userName
                                             select new UserUnitInfoDataDTO
                                             {
                                                 LocationId = l.LocationID,
                                                 UnitName = rd.ReferenceDataValue,
                                                 UnitType = rd.ReferenceDataValue
                                             }).FirstOrDefaultAsync();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return userUnitDetails;
            }
        }
    }
}