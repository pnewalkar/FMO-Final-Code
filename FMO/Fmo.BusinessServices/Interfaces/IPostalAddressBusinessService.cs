namespace Fmo.BusinessServices.Interfaces
{
    using System.Collections.Generic;
    using Fmo.DTO;


    /// <summary>
    /// Interface for USR Business service
    /// </summary>
    public interface IPostalAddressBusinessService
    {
        /// <summary>
        /// Method to save the list of PAF data into the database.
        /// </summary>
        /// <returns>return success</returns>
        bool SavePostalAddress(List<PostalAddressDTO> lstPostalAddress, string strFileName);

        bool SavePAFDetails(List<PostalAddressDTO> postalAddress);

        void SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress);
    }
}