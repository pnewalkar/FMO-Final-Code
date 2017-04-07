using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IPostalAddressBusinessService
    {
        bool SavePostalAddress(List<PostalAddressDTO> lstPostalAddress, string strFileName);

        bool SavePAFDetails(PostalAddressDTO postalAddress);
    }
}