using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Common.ActionManager.WebAPI.DataDTO;

namespace RM.Common.ActionManager.WebAPI.Interfaces
{
    public interface IActionManagerDataService
    {
        /// <summary>
        /// This method fetches role based functions for the current user
        /// </summary>
        /// <param name="userUnitInfo">user unit information</param>
        /// <returns>functions available for current user</returns>
        Task<List<RoleAccessDataDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDataDTO userUnitInfo);

        /// <summary>
        /// This method fetches Unit information for which user has access
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="locationId">unit id</param>
        /// <returns>unit details</returns>
        Task<UserUnitInfoDataDTO> GetUserUnitInfo(string userName, Guid locationId);

        /// <summary>
        /// This method fetches information for units above mail center for the current user
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="locationId">unit id</param>
        /// <returns>unit details</returns>
        Task<UserUnitInfoDataDTO> GetUserUnitInfoFromReferenceData(string userName, Guid locationId);
    }
}