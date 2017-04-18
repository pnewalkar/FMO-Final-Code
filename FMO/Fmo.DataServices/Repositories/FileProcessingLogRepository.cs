using Fmo.DataServices.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.DBContext;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{

    public class FileProcessingLogRepository : RepositoryBase<FileProcessingLog, FMODBContext>, IFileProcessingLogRepository
    {
        public FileProcessingLogRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Log exception if the NYB record insertion fails
        /// </summary>
        /// <param name="_fileProcessingLogDTO"></param>
        /// <returns></returns>
        public bool LogFileException(FileProcessingLogDTO _fileProcessingLogDTO)
        {
            try
            {
                bool saveFlag = false;
                var entity = GenericMapper.Map<FileProcessingLogDTO, FileProcessingLog>(_fileProcessingLogDTO);
                DataContext.FileProcessingLogs.Add(entity);
                DataContext.SaveChanges();
                saveFlag = true;
                return saveFlag;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
