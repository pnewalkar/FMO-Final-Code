using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains methods for fetching the user unit information.
    /// </summary>
    /// <seealso cref="Fmo.BusinessServices.Interfaces.IUserRoleUnitBussinessService" />
    public class UserRoleUnitBussinessService : IUserRoleUnitBussinessService
    {
        private IUserRoleUnitRepository userRoleUnitRepository = default(IUserRoleUnitRepository);

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleUnitBussinessService"/> class.
        /// </summary>
        /// <param name="userRoleUnitRepository">The user role unit repository.</param>
        public UserRoleUnitBussinessService(IUserRoleUnitRepository userRoleUnitRepository)
        {
            this.userRoleUnitRepository = userRoleUnitRepository;
        }

        /// <summary>
        /// Gets the user unit information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Guid</returns>
        public Task<Guid> GetUserUnitInfo(string userName)
        {
            try
            {
                return userRoleUnitRepository.GetUserUnitInfo(userName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
