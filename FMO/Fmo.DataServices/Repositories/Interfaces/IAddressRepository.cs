using System.Collections.Generic;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        bool SaveAddress(PostalAddress objPostalAddress);

        int GetPostalAddress(int? uDPRN);

        int GetPostalAddress(PostalAddress objPostalAddress);

        bool UpdateAddress(PostalAddress objPostalAddress, int addressType);

        bool DeleteNYBPostalAddress(List<int> lstUDPRN, int addressType);
    }
}