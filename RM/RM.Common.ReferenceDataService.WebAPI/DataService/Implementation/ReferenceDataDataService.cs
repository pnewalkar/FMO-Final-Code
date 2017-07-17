namespace RM.Common.ReferenceData.WebAPI.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using CommonLibrary.LoggingMiddleware;
    using Interface;
    using RM.Common.ReferenceData.WebAPI.Entities;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
    using RM.CommonLibrary.HelperMiddleware;

    public class ReferenceDataDataService : DataServiceBase<ReferenceDataCategory, ReferenceDataDBContext>, IReferenceDataDataService
    {
        private const int ReferenceDataCategoryTypeForNameValuePair = 1;
        private const int ReferenceDataCategoryTypeForSimpleList = 2;

        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.ReferenceDataAPIPriority;
        private int entryEventId = LoggerTraceConstants.ReferenceDataDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.ReferenceDataDataServiceMethodExitEventId;

        #region Constructor

        public ReferenceDataDataService(IDatabaseFactory<ReferenceDataDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets all reference data for category type nameValuePair.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <param name="dbItemName">dbItemName is recorded as the item name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public NameValuePair GetNameValueReferenceData(string dbGroupName, string dbItemName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNameValueReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                RM.Common.ReferenceData.WebAPI.Entities.ReferenceData referenceData = null;

                var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas)
                    .Where(n => n.CategoryName.Equals(dbGroupName, StringComparison.OrdinalIgnoreCase)
                                && n.CategoryType.Equals(ReferenceDataCategoryTypeForNameValuePair)).SingleOrDefault();

                if (referenceDataCategories?.ReferenceDatas != null && referenceDataCategories.ReferenceDatas.Count > 0)
                {
                    referenceData = referenceDataCategories.ReferenceDatas
                            .Where(n => n.ReferenceDataName.Trim().Equals(dbItemName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }

                if (referenceData != null)
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                    return new NameValuePair
                    {
                        Id = referenceData.ID,
                        Group = referenceDataCategories.CategoryName,
                        Name = referenceData.ReferenceDataName,
                        Value = referenceData.ReferenceDataValue,
                        DisplayText = referenceData.DisplayText,
                        Description = referenceData.DataDescription,
                        maintainable = referenceDataCategories.Maintainable
                    };
                }
                else
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets all reference data for category type nameValuePair using Guid Id
        /// </summary>
        /// <param name="id">Guid Id</param>
        /// <returns>NameValuePair</returns>
        public NameValuePair GetNameValueReferenceData(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNameValueReferenceData"))
            {
                string methodName = typeof(ReferenceDataDataService) + "." + nameof(GetNameValueReferenceData);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                RM.Common.ReferenceData.WebAPI.Entities.ReferenceData referenceData = null;

                referenceData = DataContext.ReferenceDatas.Include(m => m.ReferenceDataCategory)
                        .Where(n => n.ID == id && n.ReferenceDataCategory.CategoryType.Equals(ReferenceDataCategoryTypeForNameValuePair)).SingleOrDefault();

                if (referenceData != null)
                {
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return new NameValuePair
                    {
                        Id = referenceData.ID,
                        Group = referenceData.ReferenceDataCategory.CategoryName,
                        Name = referenceData.ReferenceDataName,
                        Value = referenceData.ReferenceDataValue,
                        DisplayText = referenceData.DisplayText,
                        Description = referenceData.DataDescription,
                        maintainable = referenceData.ReferenceDataCategory.Maintainable
                    };
                }
                else
                {
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets all reference data for category type NameValuePair.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public List<NameValuePair> GetNameValueReferenceData(string dbGroupName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetNameValueReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<NameValuePair> nameValuePairList = null;

                var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas)
                    .Where(n => n.CategoryName.Equals(dbGroupName, StringComparison.OrdinalIgnoreCase)
                                && n.CategoryType.Equals(ReferenceDataCategoryTypeForNameValuePair)).SingleOrDefault();
                if (referenceDataCategories?.ReferenceDatas != null && referenceDataCategories.ReferenceDatas.Count > 0)
                {
                    nameValuePairList = new List<NameValuePair>();
                    referenceDataCategories.ReferenceDatas.ToList().ForEach(refData => nameValuePairList.Add(
                    new NameValuePair
                    {
                        Id = refData.ID,
                        Group = referenceDataCategories.CategoryName,
                        Name = refData.ReferenceDataName,
                        Value = refData.ReferenceDataValue,
                        DisplayText = refData.DisplayText,
                        Description = refData.DataDescription,
                        maintainable = referenceDataCategories.Maintainable
                    }));
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return nameValuePairList;
            }
        }

        /// <summary>
        /// Gets all reference data for category type SimpleList.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public SimpleListDTO GetSimpleListReferenceData(string dbGroupName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetSimpleListReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<ListItems> listItems = new List<ListItems>();

                var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas)
                    .Where(n => n.CategoryName.Equals(dbGroupName, StringComparison.OrdinalIgnoreCase)
                                && n.CategoryType.Equals(ReferenceDataCategoryTypeForSimpleList)).SingleOrDefault();

                if (referenceDataCategories?.ReferenceDatas != null && referenceDataCategories.ReferenceDatas.Count > 0)
                {
                    referenceDataCategories.ReferenceDatas.ToList().ForEach(refData => listItems.Add(new ListItems
                    {
                        Id = refData.ID,
                        Name = refData.ReferenceDataName,
                        Value = refData.ReferenceDataValue,
                        DisplayText = refData.DisplayText,
                        Description = refData.DataDescription
                    }));
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }

                if (referenceDataCategories != null)
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return new SimpleListDTO
                    {
                        Id = referenceDataCategories.ID,
                        ListName = referenceDataCategories.CategoryName,
                        Maintainable = referenceDataCategories.Maintainable,
                        ListItems = listItems
                    };
                }
                else
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets all reference data for category type SimpleList using Guid Id.
        /// </summary>
        /// <param name="id">Guid Id</param>
        /// <returns>SimpleListDTO</returns>
        public SimpleListDTO GetSimpleListReferenceData(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetSimpleListReferenceData"))
            {
                string methodName = typeof(ReferenceDataDataService) + "." + nameof(GetSimpleListReferenceData);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<ListItems> listItems = new List<ListItems>();


                var referenceData = DataContext.ReferenceDatas.Include(m => m.ReferenceDataCategory)
                        .Where(n => n.ID == id && n.ReferenceDataCategory.CategoryType.Equals(ReferenceDataCategoryTypeForSimpleList)).SingleOrDefault();

                if (referenceData != null)
                {
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return new SimpleListDTO
                    {
                        Id = referenceData.ID,
                        ListName = referenceData.ReferenceDataCategory.CategoryName,
                        Maintainable = referenceData.ReferenceDataCategory.Maintainable,
                        ListItems = listItems
                    };
                }
                else
                {
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return null;
                }
            }
        }

        #endregion Methods
    }
}