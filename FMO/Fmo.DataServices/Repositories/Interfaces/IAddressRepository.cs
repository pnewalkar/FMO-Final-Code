using System.Collections.Generic;
using Fmo.Entities;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        bool SaveAddress(PostalAddress objPostalAddress);

        int GetPostalAddress(int? uDPRN);

        int GetPostalAddress(PostalAddressDTO objPostalAddress);

        bool UpdateAddress(PostalAddressDTO objPostalAddress, int addressType);

        bool DeleteNYBPostalAddress(List<int> lstUDPRN, int addressType);
    }
}