using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressLocationRepository
    {
        AddressLocation GetAddressLocationByUDPRN(int uDPRN);
    }
}