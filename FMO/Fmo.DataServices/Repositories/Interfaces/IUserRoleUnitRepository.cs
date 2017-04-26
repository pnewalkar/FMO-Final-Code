using System;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IUserRoleUnitRepository
    {
        Task<Guid> GetUserUnitInfo(string userName);
    }
}