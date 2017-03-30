using Fmo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressLocationRepository
    {
        AddressLocation GetAddressLocationByUDPRN(int uDPRN);
    }
}
