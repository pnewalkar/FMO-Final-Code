namespace Fmo.DataServices.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fmo.DTO;

    public interface IAddressRepository
    {
        /// <summary>
        /// Save PostalAddress based on the PostalAddressDTO passed
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO</param>
        /// <param name="strFileName">Passing File Name in case PAF, NYB to track error</param>
        /// <returns>return status as bool</returns>
        bool SaveAddress(PostalAddressDTO objPostalAddress, string strFileName);

        /// <summary>
        /// Update PostalAddress based on the PostalAddressDTO passed for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO</param>
        /// <param name="strFileName">Passing File Name in case PAF, NYB to track error</param>
        /// <returns>return status as bool</returns>
        bool UpdateAddress(PostalAddressDTO objPostalAddress, string strFileName);

        /// <summary>
        /// Get the existing postal address details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostalAddress DTO</returns>
        PostalAddressDTO GetPostalAddress(int? uDPRN);

        /// <summary>
        /// Get the existing postal address details based on the address for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO for search operation</param>
        /// <returns>PostalAddress DTO</returns>
        PostalAddressDTO GetPostalAddress(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// Insert PostalAddress based on the PostalAddressDTO passed for PAF
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO</param>
        /// <param name="strFileName">Passing File Name in case PAF, NYB to track error</param>
        /// <returns>return status as bool</returns>
        bool InsertAddress(PostalAddressDTO objPostalAddress, string strFileName);

        /// <summary>
        /// Delete PostalAddress of NYB
        /// </summary>
        /// <param name="lstUDPRN">list of UDPRN id to be deleted</param>
        /// <param name="addressType">Guid of AddressType of NYB</param>
        /// <returns>return status as bool</returns>
        bool DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType);
    }
}