using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Common.ActionManager.WebAPI.DataDTO;


namespace RM.Common.ActionManager.WebAPI.Interfaces
{
    public interface IActionManagerDataService
    {
        //TODO: Method comments to be updated once the code is finalized and tested
        /// <summary>
        /// This method fetches role based functions for the current user
        /// </summary>
        /// <param name="userUnitInfo"></param>
        /// <returns></returns>
        Task<List<RoleAccessDataDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDataDTO userUnitInfo);

        //TODO: Method comments to be updated once the code is finalized and tested
        /// <summary>
        /// This method fetches Unit information for which user has access
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UserUnitInfoDataDTO> GetUserUnitInfo(string userName, Guid locationId);

        //TODO: Method comments to be updated once the code is finalized and tested
        /// <summary>
        /// This information fetches information for units above mail center for the current user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UserUnitInfoDataDTO> GetUserUnitInfoFromReferenceData(string userName, Guid locationId);
    }
}