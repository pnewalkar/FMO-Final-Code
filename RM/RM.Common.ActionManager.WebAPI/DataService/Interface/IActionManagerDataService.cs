using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.Common.ActionManager.WebAPI.DTO;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.Common.ActionManager.WebAPI.Interfaces
{
    public interface IActionManagerDataService
    {
        Task<List<RoleAccessDataDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDataDTO userUnitInfo);

        Task<Guid> GetUserUnitInfo(string userName);

        Task<UserUnitInfoDataDTO> GetUserUnitFromReferenceData(string userName);
    }
}