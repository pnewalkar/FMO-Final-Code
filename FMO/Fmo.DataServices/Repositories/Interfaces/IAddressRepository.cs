namespace Fmo.DataServices.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fmo.DTO;

    /// <summary>
    /// Interface to interact with postal address entity
    /// </summary>
    public interface IAddressRepository
    {
        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        bool SaveAddress(PostalAddressDTO objPostalAddress, string strFileName);

        bool UpdateAddress(PostalAddressDTO objPostalAddress, string strFileName);

        PostalAddressDTO GetPostalAddress(int? uDPRN);

        PostalAddressDTO GetPostalAddress(PostalAddressDTO objPostalAddress);

        bool InsertAddress(PostalAddressDTO objPostalAddress, string strFileName);

        /// <summary>
        /// Delete postal Address records do not have an associated Delivery Point
        /// </summary>
        /// <param name="lstUDPRN">List of UDPRN</param>
        /// <param name="addressType">NYB</param>
        /// <returns>true or false</returns>
        bool DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType);
    }
}