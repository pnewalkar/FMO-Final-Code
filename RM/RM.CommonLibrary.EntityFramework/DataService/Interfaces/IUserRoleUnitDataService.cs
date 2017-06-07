using System;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    public interface IUserRoleUnitDataService
    {
        Task<Guid> GetUserUnitInfo(string userName);
    }
}