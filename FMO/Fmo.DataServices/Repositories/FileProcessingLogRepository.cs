using System;
using Fmo.Common.Interface;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// Repository to interact with file processing log entity
    /// </summary>
    public class FileProcessingLogRepository : RepositoryBase<FileProcessingLog, FMODBContext>, IFileProcessingLogRepository
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public FileProcessingLogRepository(IDatabaseFactory<FMODBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Log exception if the PAF and NYB record insertion fails
        /// </summary>
        /// <param name="fileProcessingLogDTO"> Expects DTO object to save exception while saving records in DB</param>
        public void LogFileException(FileProcessingLogDTO fileProcessingLogDTO)
        {
            try
            {
                var entity = GenericMapper.Map<FileProcessingLogDTO, FileProcessingLog>(fileProcessingLogDTO);
                DataContext.FileProcessingLogs.Add(entity);
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
            }
        }
    }
}
