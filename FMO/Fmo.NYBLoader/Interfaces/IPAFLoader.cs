using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Interfaces
{
    public interface IPAFLoader
    {
        /*void LoadPAFDetailsFromCSV(string strPath);*/

        void LoadPAF(string fileName);
        List<PostalAddressDTO> ProcessPAF(string strLine, string strfileName);
        bool SavePAFDetails(List<PostalAddressDTO> lstPostalAddress);
    }
}
