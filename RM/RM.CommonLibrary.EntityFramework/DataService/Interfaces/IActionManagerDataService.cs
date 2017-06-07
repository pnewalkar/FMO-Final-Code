using RM.CommonLibrary.EntityFramework.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    public interface IActionManagerDataService
    {
        Task<List<RoleAccessDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDTO userUnitInfo);
    }
}