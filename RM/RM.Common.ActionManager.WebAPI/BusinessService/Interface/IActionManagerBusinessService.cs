using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Common.ActionManager.WebAPI.DTO;

namespace RM.Common.ActionManager.WebAPI.BusinessService.Interface
{
    public interface IActionManagerBusinessService
    {
        /// <summary>
        /// This method fetches role based functions for the current user
        /// </summary>
        /// <param name="userUnitInfo">user unit information</param>
        /// <returns>functions available for current user</returns>
        Task<List<RoleAccessDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDTO userUnitInfo);

        /// <summary>
        /// This method fetches Unit information for which user has access
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="locationId">unit id</param>
        /// <returns>unit details</returns>
        Task<UserUnitInfoDTO> GetUserUnitInfo(string userName, Guid locationId);
    }
}