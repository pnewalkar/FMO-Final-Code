using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// This interface contains declaration of method for fetching the user unit information.
    /// </summary>
    public interface IUserRoleUnitBussinessService
    {
        /// <summary>
        /// Gets the user unit information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Guid</returns>
        Task<Guid> GetUserUnitInfo(string userName);
    }
}
