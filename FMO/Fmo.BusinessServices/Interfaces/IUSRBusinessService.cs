namespace Fmo.BusinessServices.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fmo.DTO.FileProcessing;

    /// <summary>
    /// Interface for USR Business service
    /// </summary>
    public interface IUSRBusinessService
    {
        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="lstAddressLocationUSRPOSTDTO">lstAddressLocationUSRPOSTDTO</param>
        /// <returns>Task</returns>
        Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> lstAddressLocationUSRPOSTDTO);
    }
}