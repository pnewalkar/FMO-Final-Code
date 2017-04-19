using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IFileProcessingLogRepository
    {
        void LogFileException(FileProcessingLogDTO fileProcessingLogDTO);
    }
}
