using System;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.DataMiddleware;
using System.Diagnostics;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// DataService to interact with file processing log entity
    /// </summary>
    public class FileProcessingLogDataService : DataServiceBase<FileProcessingLog, RMDBContext>, IFileProcessingLogDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public FileProcessingLogDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Log exception if the PAF and NYB record insertion fails
        /// </summary>
        /// <param name="fileProcessingLogDTO">
        /// Expects DTO object to save exception while saving records in DB
        /// </param>
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
                this.loggingHelper.Log(ex, TraceEventType.Error);
            }
        }
    }
}