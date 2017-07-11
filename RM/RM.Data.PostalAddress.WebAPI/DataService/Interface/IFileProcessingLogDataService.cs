using RM.DataManagement.PostalAddress.WebAPI.DTO;

namespace RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces
{
    /// <summary>
    /// Interface to perfrom CRUD operations on file procesing log
    /// </summary>
    public interface IFileProcessingLogDataService
    {
        /// <summary>
        /// Log exception if the NYB record insertion fails
        /// </summary>
        /// <param name="fileProcessingLogDTO"> Expects DTO object to save exception while saving records in DB</param>
        void LogFileException(FileProcessingLogDTO fileProcessingLogDTO);
    }
}