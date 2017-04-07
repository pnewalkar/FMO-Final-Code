using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IFileProcessingLogRepository
    {
        bool LogFileException(FileProcessingLogDTO _fileProcessingLogDTO);
    }
}
