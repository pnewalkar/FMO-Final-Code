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

namespace RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService
{
    public class DeliveryRouteIntegrationService : IDeliveryRouteIntegrationService
    {
        private const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        private const string DeliveryRouteManagerWebAPIName = "DeliveryRouteManagerWebAPIName";
        private const string UnitManagerDataWebAPIName = "UnitManagerDataWebAPIName";

        #region Property Declarations

        private string deliveryRouteWebAPIName = string.Empty;
        private string referenceDataWebAPIName = string.Empty;
        private string unitManagerDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Property Declarations

        #region Constructor

        public DeliveryRouteIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.deliveryRouteWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryRouteManagerWebAPIName).ToString() : string.Empty;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(ReferenceDataWebAPIName).ToString() : string.Empty;
            this.unitManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(UnitManagerDataWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructor

        #region public methods

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> categoryNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "simpleLists", categoryNames);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
                listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return listReferenceCategories;
            }
        }

        /// <summary>
        /// Retreive reference data details from
        /// </summary>
        /// <param name="categoryName">categoryname</param>
        /// <param name="itemName">Reference data Name</param>
        /// <returns>GUID</returns>
        public async Task<Guid> GetReferenceDataGuId(string categoryName, string itemName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataGuId"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

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
                    referenceId = apiResult.Item2.ListItems.Where(n => n.Value == itemName).SingleOrDefault().Id;
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return referenceId;
            }
        }

        public async Task<Guid> GetUnitLocationTypeId(Guid unitId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetUnitLocationTypeId"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "Unit/" + unitId);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }
                Guid loactionTypeId = JsonConvert.DeserializeObject<Guid>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryRouteAPIPriority, LoggerTraceConstants.DeliveryRouteIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return loactionTypeId;
            }
        }

        #endregion public methods
    }
}