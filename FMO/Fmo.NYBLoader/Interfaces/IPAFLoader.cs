using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.NYBLoader.Interfaces
{
    public interface IPAFLoader
    {
        void LoadPAF(string fileName);

        List<PostalAddressDTO> ProcessPAF(string strLine, string strfileName);

        bool SavePAFDetails(List<PostalAddressDTO> lstPostalAddress);
    }
}
