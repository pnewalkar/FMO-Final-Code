namespace Fmo.BusinessServices.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
        /// <returns><placeholder>A <see cref="Task"/> representing the asynchronous operation.</placeholder></returns><param name="lstAddressLocationUSRPOSTDTO">List<AddressLocationUSRPOSTDTO></param>
        /// <returns>Task</returns>
        Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> lstAddressLocationUSRPOSTDTO);
    }
}
