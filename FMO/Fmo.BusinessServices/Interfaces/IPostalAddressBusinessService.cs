using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// Interface for USR Business service
    /// </summary>
    public interface IPostalAddressBusinessService
    {
        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress">List Of address DTO</param>
        /// <param name="strFileName">CSV filename</param>
        /// <returns>return success</returns>
        bool SavePostalAddress(List<PostalAddressDTO> lstPostalAddress, string strFileName);

        /// <summary>
        /// Save list of PAF details into database.
        /// </summary>
        /// <param name="postalAddress">list of PostalAddress DTO</param>
        /// <returns>returns true or false</returns>
        bool SavePAFDetails(List<PostalAddressDTO> postalAddress);

        /// <summary>
        /// Method defination to save delivery point and Task for notification for PAF create events
        /// </summary>
        /// <param name="objPostalAddress">Postal address DTO object</param>
        void SaveDeliveryPointProcess(PostalAddressDTO objPostalAddress);
    }
}