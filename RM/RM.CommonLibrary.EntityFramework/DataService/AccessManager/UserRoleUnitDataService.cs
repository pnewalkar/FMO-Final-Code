using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.Entities;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// DataService for User Role and Unit information
    /// </summary>
    /// <seealso cref="Fmo.DataServices.Infrastructure.DataServiceBase{RM.CommonLibrary.EntityFramework.Entities.UserRoleUnit, Fmo.DataServices.DBContext.RMDBContext}"/>
    /// <seealso cref="RM.CommonLibrary.EntityFramework.DataService.Interfaces.IUserRoleUnitDataService"/>
    public class UserRoleUnitDataService : DataServiceBase<UserRoleUnit, RMDBContext>, IUserRoleUnitDataService
    {
        public UserRoleUnitDataService(IDatabaseFactory<RMDBContext> databaseFactory)
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