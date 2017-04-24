using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// Repository for User Role and Unit information
    /// </summary>
    /// <seealso cref="Fmo.DataServices.Infrastructure.RepositoryBase{Fmo.Entities.UserRoleUnit, Fmo.DataServices.DBContext.FMODBContext}" />
    /// <seealso cref="Fmo.DataServices.Repositories.Interfaces.IUserRoleUnitRepository" />
    public class UserRoleUnitRepository : RepositoryBase<UserRoleUnit, FMODBContext>, IUserRoleUnitRepository
    {
        public UserRoleUnitRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Gets the user unit information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Guid</returns>
        public async Task<Guid> GetUserUnitInfo(string userName)
        {
            var userUnit = await (from r in DataContext.UserRoleUnits.AsNoTracking()
                        join u in DataContext.Users on r.User_GUID equals u.ID
                        where u.UserName == userName
                        select r.Unit_GUID).FirstOrDefaultAsync();

            return userUnit;
        }
    }
}
