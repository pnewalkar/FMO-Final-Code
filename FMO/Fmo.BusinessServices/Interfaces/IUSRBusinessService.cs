namespace Fmo.BusinessServices.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fmo.DTO;

    public interface IUSRBusinessService
    {
        bool SaveUSRDetails(AddressLocationUSRDTO addressLocationDTO);
    }
}
