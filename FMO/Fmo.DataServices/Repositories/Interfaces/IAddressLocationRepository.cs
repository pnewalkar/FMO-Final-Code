using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressLocationRepository
    {
        AddressLocationDTO GetAddressLocationByUDPRN(int uDPRN);

        void SaveNewAddressLocation(AddressLocationDTO addressLocationDTO);
    }
}