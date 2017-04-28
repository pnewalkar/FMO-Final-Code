using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.DTO.FileProcessing;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// Interface for USR Business service
    /// </summary>
    public interface IUSRBusinessService
    {
        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="addressLocationUsrpostdtos">addressLocationUsrpostdtos</param>
        /// <returns>Task</returns>
        Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos);
    }
}