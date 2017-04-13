using Fmo.DTO;
using Fmo.Entities;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressLocationRepository
    {
        AddressLocationDTO GetAddressLocationByUDPRN(int uDPRN);

        Task<int> SaveNewAddressLocation(AddressLocationDTO addressLocationDTO);

        bool AddressLocationExists(int uDPRN);
    }
}