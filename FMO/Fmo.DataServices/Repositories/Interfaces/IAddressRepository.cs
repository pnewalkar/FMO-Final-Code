using Fmo.DataServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        bool SaveAddress(PostalAddress objPostalAddress);
    }
}
