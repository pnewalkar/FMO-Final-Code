using System.Collections.Generic;
using Fmo.DTO;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        bool SaveAddress(PostalAddressDTO objPostalAddress);

        PostalAddressDTO GetPostalAddress(int? uDPRN);

        PostalAddressDTO GetPostalAddress(PostalAddressDTO objPostalAddress);

        bool UpdateAddress(PostalAddressDTO objPostalAddress);

        bool InsertAddress(PostalAddressDTO objPostalAddress);

        bool DeleteNYBPostalAddress(List<int> lstUDPRN, int addressType);
    }
}