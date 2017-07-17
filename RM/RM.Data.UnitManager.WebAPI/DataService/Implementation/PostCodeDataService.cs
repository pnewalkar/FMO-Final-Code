using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.DataManagement.UnitManager.WebAPI.DataService
{
    /// <summary>
    /// DataService to interact with postal address entity
    /// </summary>
    public class PostCodeDataService : DataServiceBase<Postcode, RMDBContext>//, IPostCodeDataService
    {
        private const int searchResultCount = 5;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PostCodeDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Fetch postcode for basic search
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The result set of post code
        /// </returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchPostCodeUnitForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                int takeCount = searchResultCount;
                searchText = searchText ?? string.Empty;
                var postCodeDetailsDto = await (from p in DataContext.Postcodes.AsNoTracking()
                                                join s in DataContext.PostcodeSectors.AsNoTracking() on p.SectorGUID equals s.ID
                                                join u in DataContext.UnitPostcodeSectors.AsNoTracking() on s.ID equals u.PostcodeSector_GUID
                                                where p.PostcodeUnit.StartsWith(searchText)
                                                 && u.Unit_GUID == unitGuid
                                                select new PostCodeDTO
                                                {
                                                    PostcodeUnit = p.PostcodeUnit,
                                                    InwardCode = p.InwardCode,
                                                    OutwardCode = p.OutwardCode,
                                                    Sector = p.Sector
                                                }).Take(takeCount).ToListAsync();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postCodeDetailsDto;
            }
        }

        /// <summary>
        /// Get the count of post code
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The total count of post code
        /// </returns>
        public async Task<int> GetPostCodeUnitCount(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodeUnitCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                searchText = searchText ?? string.Empty;
                var postCodeDetailsDto = await (from p in DataContext.Postcodes.AsNoTracking()
                                                join s in DataContext.PostcodeSectors.AsNoTracking() on p.SectorGUID equals s.ID
                                                join u in DataContext.UnitPostcodeSectors.AsNoTracking() on s.ID equals u.PostcodeSector_GUID
                                                where p.PostcodeUnit.StartsWith(searchText)
                                                 && u.Unit_GUID == unitGuid
                                                select p).CountAsync();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postCodeDetailsDto;
            }
        }

        /// <summary>
        /// Fetches the post code unit for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List<PostCodeDTO></returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.FetchPostCodeUnitForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                searchText = searchText ?? string.Empty;
                var postCodeDetailsDto = await (from p in DataContext.Postcodes.AsNoTracking()
                                                join s in DataContext.PostcodeSectors.AsNoTracking() on p.SectorGUID equals s.ID
                                                join u in DataContext.UnitPostcodeSectors.AsNoTracking() on s.ID equals u.PostcodeSector_GUID
                                                where p.PostcodeUnit.StartsWith(searchText)
                                                 && u.Unit_GUID == unitGuid
                                                select new PostCodeDTO
                                                {
                                                    PostcodeUnit = p.PostcodeUnit,
                                                    InwardCode = p.InwardCode,
                                                    OutwardCode = p.OutwardCode,
                                                    Sector = p.Sector
                                                }).ToListAsync();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postCodeDetailsDto;
            }
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        public async Task<Guid> GetPostCodeID(string postCode)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostCodeID"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var postCodeDetail = await DataContext.Postcodes.Where(l => l.PostcodeUnit.Trim().Equals(postCode, StringComparison.OrdinalIgnoreCase)).SingleOrDefaultAsync();
                if (postCodeDetail != null)
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return postCodeDetail.ID;
                }
                else
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostCodePriority, LoggerTraceConstants.PostCodeDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return Guid.Empty;
                }
            }
        }
    }
}