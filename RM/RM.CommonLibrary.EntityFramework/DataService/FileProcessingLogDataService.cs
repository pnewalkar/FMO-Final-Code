using System;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.DataMiddleware;
using System.Diagnostics;
using System.Reflection;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.CommonLibrary.HelperMiddleware;

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
                using (loggingHelper.RMTraceManager.StartTrace("DataService.LogFileException"))
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.FileProcessingLogPriority, LoggerTraceConstants.FileProcessingLogPriorityDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    var entity = GenericMapper.Map<FileProcessingLogDTO, FileProcessingLog>(fileProcessingLogDTO);
                    DataContext.FileProcessingLogs.Add(entity);
                    DataContext.SaveChanges();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.FileProcessingLogPriority, LoggerTraceConstants.FileProcessingLogPriorityDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex, TraceEventType.Error);
            }
        }
    }
}