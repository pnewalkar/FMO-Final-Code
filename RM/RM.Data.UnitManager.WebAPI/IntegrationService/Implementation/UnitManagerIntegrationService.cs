using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.Data.UnitManager.WebAPI.DTO;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Integration.Interface;

namespace RM.DataManagement.UnitManager.WebAPI.IntegrationService.Implementation
{
    /// <summary>
    /// Integration service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class UnitManagerIntegrationService : IUnitManagerIntegrationService
    {
        private const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        private const string RouteDataWebAPIName = "RouteDataWebAPIName";

        #region Property Declarations

        private string referenceDataWebAPIName = string.Empty;
        private string routeDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Property Declarations

        #region Constructor

        public UnitManagerIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            // Store injected dependencies
            this.httpHandler = httpHandler;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(ReferenceDataWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        #region public methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public async Task<Guid> GetReferenceDataGuId(string categoryName, string itemName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataGuId"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                Guid referenceId = Guid.Empty;
                HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "simpleLists?listName=" + categoryName);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<string, SimpleListDTO> apiResult = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
                if (apiResult != null && apiResult.Item2 != null && apiResult.Item2.ListItems != null && apiResult.Item2.ListItems.Count > 0)
                {
                    referenceId = apiResult.Item2.ListItems.Where(n => n.Value.Equals(itemName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault().Id;
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return referenceId;
            }
        }

        /// <summary>
        /// Gets the name of the reference data categories by category.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns>ReferenceDataCategoryDTO</returns>
        public async Task<ReferenceDataCategoryDTO> GetReferenceDataSimpleLists(string listName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                ReferenceDataCategoryDTO referenceCategories = new ReferenceDataCategoryDTO();

                HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "simpleLists?listName=" + listName);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<string, SimpleListDTO> apiResult = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);

                referenceCategories = ReferenceDataHelper.MapDTO(apiResult.Item2);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return referenceCategories;
            }
        }

        /// <summary>
        /// Get the Route data for postcode
        /// </summary>
        /// <param name="postcode"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<List<DeliveryRouteDTO>> GetRouteData(string postcode, string fields)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetRouteData"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                ReferenceDataCategoryDTO referenceCategories = new ReferenceDataCategoryDTO();

                HttpResponseMessage result = await httpHandler.GetAsync(routeDataWebAPIName + "deliveryroute/postcode/" + postcode + "/" + fields);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<DeliveryRouteDTO> apiResult = JsonConvert.DeserializeObject<List<DeliveryRouteDTO>>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return apiResult;
            }
        }

        #endregion public methods
    }
}