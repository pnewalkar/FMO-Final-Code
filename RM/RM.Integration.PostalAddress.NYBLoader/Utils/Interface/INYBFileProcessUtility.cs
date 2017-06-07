namespace RM.Integration.PostalAddress.NYBLoader.Utils.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RM.CommonLibrary.EntityFramework.DTO;

    /// <summary>
    /// Load and process NYb files
    /// </summary>
    public interface INYBFileProcessUtility
    {
        /// <summary>
        /// Reads data from CSV file and maps to postalAddressDTO object
        /// </summary>
        /// <param name="line">Line read from CSV File</param>
        /// <returns>Postal Address DTO</returns>
        List<PostalAddressDTO> LoadNybDetailsFromCsv(string line);

        /// <summary>
        /// Web API call to save postalAddress to PostalAddress table
        /// </summary>
        /// <param name="postalAddresses">List of mapped address dto to validate each records</param>
        /// <param name="fileName">File Name</param>
        /// <returns>If success returns true else returns false</returns>
        Task<bool> SaveNybDetails(List<PostalAddressDTO> postalAddresses, string fileName);

        /// <summary>
        /// Read files from zip file and call NYBLoader Assembly to validate and save records
        /// </summary>
        /// <param name="fileName">Input file name as a param</param>
        void LoadNYBDetails(string fileName);
    }
}