using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AutoMapper;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// To interact with reference data entity
    /// </summary>
    public class ReferenceDataCategoryDataService : DataServiceBase<ReferenceDataCategory, RMDBContext>, IReferenceDataCategoryDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #region Constructor

        public ReferenceDataCategoryDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructor

        public Guid GetReferenceDataId(string strCategoryname, string strRefDataName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetReferenceDataId"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    Guid statusId = Guid.Empty;
                    var result = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).Where(n => n.CategoryName.Trim().Equals(strCategoryname, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (result?.ReferenceDatas != null && result.ReferenceDatas.Count > 0)
                    {
                        var referenceData = result.ReferenceDatas.Where(n => n.ReferenceDataValue.Trim().Equals(strRefDataName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                        if (referenceData != null)
                        {
                            statusId = referenceData.ID;
                        }
                    }

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return statusId;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
                }
            }
        }

        /// <summary>
        ///  Retreive GUIDs for specified categories
        /// </summary>
        /// <param name="strCategoryname">categoryname</param>
        /// <param name="lstRefDataName">Reference data Names</param>
        /// <returns>List<Guid></returns>
        public List<Guid> GetReferenceDataIds(string strCategoryname, List<string> lstRefDataName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetReferenceDataIds"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    List<Guid> statusIds = new List<Guid>();
                    var result = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).Where(n => n.CategoryName.Trim().Equals(strCategoryname, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (result?.ReferenceDatas != null && result.ReferenceDatas.Count > 0)
                    {
                        var referenceData = result.ReferenceDatas.Where(n => lstRefDataName.Contains(n.DataDescription.Trim().ToUpper())).ToList();
                        referenceData.ForEach(r => statusIds.Add(r.ID));
                    }

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return statusIds;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
                }
            }
        }

        /// <summary>
        /// Fetch the Route log status.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> RouteLogStatus()
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.RouteLogStatus"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                try
                {
                    List<ReferenceDataDTO> lstReferenceDt = new List<ReferenceDataDTO>();
                    string categoryname = "Delivery Point Operational Status";
                    var query = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).Where(n => n.CategoryName.Trim().Equals(categoryname, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (query?.ReferenceDatas != null && query.ReferenceDatas.Count > 0)
                    {
                        lstReferenceDt = GenericMapper.MapList<ReferenceData, ReferenceDataDTO>(query.ReferenceDatas.ToList());
                    }

                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return lstReferenceDt;
                }
                catch (InvalidOperationException ex)
                {
                    ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                    throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
                }
            }
        }

        /// <summary>
        /// Fetch the Route log selection type.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> RouteLogSelectionType()
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.RouteLogSelectionType"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<ReferenceDataDTO> lstReferenceDt = null;
                var query = DataContext.ReferenceDataCategories.Include(m => m.ReferenceDatas)
                    .AsNoTracking().SingleOrDefault(n => n.CategoryName.Equals(
                        "UI_RouteLogSearch_SelectionType",
                        StringComparison.OrdinalIgnoreCase));
                if (query?.ReferenceDatas != null && query.ReferenceDatas.Count > 0)
                {
                    lstReferenceDt = GenericMapper.MapList<ReferenceData, ReferenceDataDTO>(query.ReferenceDatas.ToList());
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return lstReferenceDt;
            }
        }

        /// <summary>
        /// Gets all reference category list along with associated reference data.
        /// </summary>
        /// <returns>List<ReferenceDataCategoryDTO></returns>
        public List<ReferenceDataCategoryDTO> GetAllReferenceCategoryList()
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetAllReferenceCategoryList"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                Guid statusId = Guid.Empty;
                var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).ToList();
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<ReferenceDataCategory, ReferenceDataCategoryDTO>();
                    cfg.CreateMap<ReferenceData, ReferenceDataDTO>();
                });

                Mapper.Configuration.CreateMapper();
                List<ReferenceDataCategoryDTO> referenceDataCategoryListDto = Mapper.Map<List<ReferenceDataCategory>, List<ReferenceDataCategoryDTO>>(referenceDataCategories);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return referenceDataCategoryListDto;
            }
        }

        /// <summary>
        /// Gets the name of the reference data categories by category.
        /// </summary>
        /// <param name="categoryNames">The category names.</param>
        /// <returns>List ReferenceDataCategoryDTO</returns>
        public List<ReferenceDataCategoryDTO> GetReferenceDataCategoriesByCategoryNames(List<string> categoryNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetReferenceDataCategoriesByCategoryNames"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).Where(m => categoryNames.Contains(m.CategoryName.Trim())).ToList();
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<ReferenceDataCategory, ReferenceDataCategoryDTO>();
                    cfg.CreateMap<ReferenceData, ReferenceDataDTO>();
                });

                Mapper.Configuration.CreateMapper();
                List<ReferenceDataCategoryDTO> referenceDataCategoryListDto = Mapper.Map<List<ReferenceDataCategory>, List<ReferenceDataCategoryDTO>>(referenceDataCategories);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataCategoryDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return referenceDataCategoryListDto;
            }
        }
    }
}