using System;
using System.Diagnostics;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Implementation.MappingConfiguration;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.Entities;

namespace RM.DataManagement.PostalAddress.WebAPI.DataService.Implementation
{
    /// <summary>
    /// DataService to interact with file processing log entity
    /// </summary>
    public class FileProcessingLogDataService : DataServiceBase<FileProcessingLog, PostalAddressDBContext>, IFileProcessingLogDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.FileProcessingLogPriority;
        private int entryEventId = LoggerTraceConstants.FileProcessingLogDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.FileProcessingLogDataServiceMethodExitEventId;

        public FileProcessingLogDataService(IDatabaseFactory<PostalAddressDBContext> databaseFactory, ILoggingHelper loggingHelper)
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
                using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SavePostalAddressForNYB"))
                {
                    string methodName = typeof(FileProcessingLogDataService) + "." + nameof(LogFileException);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    var entity = GenericMapper.Map<FileProcessingLogDTO, FileProcessingLog>(fileProcessingLogDTO);
                    DataContext.FileProcessingLogs.Add(entity);
                    DataContext.SaveChanges();

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                }
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex, TraceEventType.Error);
            }
        }
    }
}