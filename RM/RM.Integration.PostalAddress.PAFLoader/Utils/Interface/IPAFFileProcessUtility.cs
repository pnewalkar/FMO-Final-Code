namespace RM.Integration.PostalAddress.PAFLoader.Utils.Interfaces
{
    using System.Collections.Generic;
    using RM.CommonLibrary.EntityFramework.DTO;

    public interface IPAFFileProcessUtility
    {
        bool LoadPAF(string fileName);

        List<PostalAddressDTO> ProcessPAF(string strLine, string strfileName);

        bool SavePAFDetails(List<PostalAddressDTO> lstPostalAddress);
    }
}
