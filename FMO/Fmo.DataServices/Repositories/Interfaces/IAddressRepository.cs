using Fmo.Entities;
using System.Collections.Generic;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        bool SaveAddress(PostalAddress objPostalAddress);

        bool DeleteNYBPostalAddress(List<int> lstUDPRN);
    }
}