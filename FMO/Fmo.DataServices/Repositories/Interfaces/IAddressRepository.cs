using System;
using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        bool SaveAddress(PostalAddressDTO objPostalAddress, string strFileName);

        PostalAddressDTO GetPostalAddress(int? uDPRN);

        PostalAddressDTO GetPostalAddress(PostalAddressDTO objPostalAddress);

        bool InsertAddress(PostalAddressDTO objPostalAddress, string strFileName);

        bool DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType);
    }
}