using System.Collections.Generic;
using Fmo.DTO;
using Fmo.Entities;
using System;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        bool SaveAddress(PostalAddressDTO objPostalAddress, string strFileName);

        PostalAddressDTO GetPostalAddress(int? uDPRN);

        PostalAddressDTO GetPostalAddress(PostalAddressDTO objPostalAddress);

        //bool UpdateAddress(PostalAddressDTO objPostalAddress, string strFileName);

        bool InsertAddress(PostalAddressDTO objPostalAddress, string strFileName);

        bool DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType);
    }
}