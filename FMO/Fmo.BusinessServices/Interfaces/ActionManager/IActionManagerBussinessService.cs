using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// This interface contains declaration of method for fetching the role based access functions
    /// </summary>
    public interface IActionManagerBussinessService
    {
        /// <summary>
        /// Gets the role based access functions.
        /// </summary>
        /// <param name="userUnitInfo">The user unit information.</param>
        /// <returns>RoleAccessDTO</returns>
        Task<List<RoleAccessDTO>> GetRoleBasedAccessFunctions(UserUnitInfoDTO userUnitInfo);
    }
}
