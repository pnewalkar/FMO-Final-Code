namespace RM.Common.ReferenceData.WebAPI.BusinessService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;
    using CommonLibrary.ConfigurationMiddleware;
    using CommonLibrary.HelperMiddleware;
    using CommonLibrary.LoggingMiddleware;
    using Microsoft.Extensions.FileProviders;
    using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
    using RM.Common.ReferenceData.WebAPI.DataService.Interface;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
    using Utils;

    public class ReferenceDataBusinessService : IReferenceDataBusinessService
    {
        private const string RefDataXMLFileName = "RefDataXMLFileName";

        #region Member Variables

        private static ReferenceDataMapping referenceDataMapping;
        private readonly IFileProvider fileProvider;
        private string refDataXMLFileName = string.Empty;
        private IReferenceDataDataService referenceDataDataService;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Member Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDataBusinessService"/> class and
        /// other classes.
        /// </summary>
        /// <param name="referenceDataCategoryDataService">IReferenceDataCategoryDataService reference</param>
        public ReferenceDataBusinessService(IFileProvider fileProvider, IReferenceDataDataService referenceDataDataService, IConfigurationHelper configHelper, ILoggingHelper loggingHelper)
        {
            this.fileProvider = fileProvider;
            this.referenceDataDataService = referenceDataDataService;
            IDirectoryContents contents = fileProvider.GetDirectoryContents(string.Empty); // the applicationRoot contents
            refDataXMLFileName = configHelper.ReadAppSettingsConfigurationValues(RefDataXMLFileName);
            IFileInfo fileInfo = fileProvider.GetFileInfo(refDataXMLFileName); // a file under applicationRoot
            this.loggingHelper = loggingHelper;

            XmlSerializer serializer = new XmlSerializer(typeof(ReferenceDataMapping));

            using (StreamReader reader = new StreamReader(fileInfo.CreateReadStream()))
            {
                referenceDataMapping = (ReferenceDataMapping)serializer.Deserialize(reader);
                reader.Close();
            }
        }

        #endregion Constructor

        #region Methods

        /// <summary> Gets all reference data. </summary> <param name="group">group is recorded as
        /// the category name</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public NameValuePair GetReferenceDataByNameValuePairs(string appGroupName, string appItemName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetReferenceDataByNameValuePairs"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var itemName = default(string);
                var categoryName = referenceDataMapping.CategoryNames
                                    .Where(a => a.AppCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase)
                                               || a.DBCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase))
                                    .Select(s => s.DBCategoryName).FirstOrDefault();

                if (categoryName != null)
                {
                    itemName = referenceDataMapping.CategoryNames
                                        .Where(a => (a.AppCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase)
                                            || a.DBCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase))
                                            && a.ReferenceDataNames.Any(b => b.AppReferenceDataName.Equals(appItemName, StringComparison.InvariantCultureIgnoreCase)
                                                                           || b.DBReferenceDataName.Equals(appItemName, StringComparison.InvariantCultureIgnoreCase)))
                                        .Select(s => s.ReferenceDataNames.Select(t => t.DBReferenceDataName).FirstOrDefault()).FirstOrDefault();
                }

                var getNameValueReferenceData = referenceDataDataService.GetNameValueReferenceData(categoryName, itemName);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);

                return getNameValueReferenceData;
            }
        }

        /// <summary> Gets all reference data. </summary> <param name="group">group is recorded as
        /// the category name</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public List<NameValuePair> GetReferenceDataByNameValuePairs(string appGroupName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetReferenceDataByNameValuePairs"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var categoryName = referenceDataMapping.CategoryNames
                                    .Where(a => a.AppCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase)
                                             || a.DBCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase))
                                    .Select(s => s.DBCategoryName).FirstOrDefault();
                var getNameValueReferenceData = referenceDataDataService.GetNameValueReferenceData(categoryName);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);

                return getNameValueReferenceData;
            }
        }

        /// <summary> Gets all reference data. </summary> <param name="group">group is recorded as
        /// the category name</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public SimpleListDTO GetSimpleListsReferenceData(string listName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetSimpleListsReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var categoryName = referenceDataMapping.CategoryNames
                                    .Where(a => a.AppCategoryName.Equals(listName, StringComparison.InvariantCultureIgnoreCase)
                                             || a.DBCategoryName.Equals(listName, StringComparison.InvariantCultureIgnoreCase))
                                    .Select(s => s.DBCategoryName).FirstOrDefault();
                var getSimpleListReferenceData = referenceDataDataService.GetSimpleListReferenceData(categoryName);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);

                return getSimpleListReferenceData;
            }
        }

        /// <summary> Gets all reference data. </summary> <param name="group">group is recorded as
        /// the category name</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public List<SimpleListDTO> GetSimpleListsReferenceData(List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetSimpleListsReferenceData"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<SimpleListDTO> simpleDtoList = new List<SimpleListDTO>();

                var categoryNames = referenceDataMapping.CategoryNames
                                        .Where(a => listNames.Any(x => x.Equals(a.AppCategoryName, StringComparison.InvariantCultureIgnoreCase)
                                                                    || x.Equals(a.DBCategoryName, StringComparison.InvariantCultureIgnoreCase)))
                                        .Select(s => s.DBCategoryName);

                foreach (var categoryName in categoryNames)
                {
                    var simpleListObject = referenceDataDataService.GetSimpleListReferenceData(categoryName);
                    if (simpleListObject != null)
                    {
                        simpleDtoList.Add(simpleListObject);
                    }
                }
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ReferenceDataAPIPriority, LoggerTraceConstants.ReferenceDataBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);

                return simpleDtoList;
            }
        }

        #endregion Methods
    }
}