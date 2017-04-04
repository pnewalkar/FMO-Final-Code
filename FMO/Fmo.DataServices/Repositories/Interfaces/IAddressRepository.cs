using System.Collections.Generic;
using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        bool SaveAddress(PostalAddressDTO objPostalAddress);

        int GetPostalAddress(int? uDPRN);

        int GetPostalAddress(PostalAddressDTO objPostalAddress);

        bool UpdateAddress(PostalAddressDTO objPostalAddress, int addressType);

        bool DeleteNYBPostalAddress(List<int> lstUDPRN, int addressType);
    }
}