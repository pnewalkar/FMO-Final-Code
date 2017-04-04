using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Interfaces
{
    public interface INYBLoader
    {
        List<PostalAddressDTO> LoadNYBDetailsFromCSV(string strPath);

        bool SaveNYBDetails(List<PostalAddressDTO> lstAddress);
    }
}
