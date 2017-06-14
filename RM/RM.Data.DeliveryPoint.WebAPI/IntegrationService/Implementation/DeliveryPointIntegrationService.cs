using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.DataManagement.DeliveryPoint.WebAPI.Integration
{
    public class DeliveryPointIntegrationService : IDeliveryPointIntegrationService
    {
        #region Property Declarations

        private string referenceDataWebAPIName = string.Empty;
        private string postalAddressManagerWebAPIName = string.Empty;
        private string accessLinkWebAPIName = string.Empty;
        private string blockSequenceWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Property Declarations

        #region Constructor

        public DeliveryPointIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.loggingHelper = loggingHelper;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.ReferenceDataWebAPIName).ToString() : string.Empty;
            this.accessLinkWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.AccessLinkWebAPIName).ToString() : string.Empty;
            this.postalAddressManagerWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.PostalAddressManagerWebAPIName).ToString() : string.Empty;
            this.blockSequenceWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryRouteManagerWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        #region public methods

        /// <summary>
        /// Retreive reference data details from
        /// </summary>
        /// <param name="categoryName">categoryname</param>
        /// <param name="itemName">Reference data Name</param>
        /// <returns>GUID</returns>
        public Guid GetReferenceDataGuId(string categoryName, string itemName)
        {
            string actionUrl = "/simplelists?listName=" + categoryName;
            string requestUrl = referenceDataWebAPIName + actionUrl;
            HttpResponseMessage result = httpHandler.GetAsync(requestUrl).Result;
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            Tuple<string, SimpleListDTO> simpleListDTO = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
            return simpleListDTO.Item2.ListItems.Where(li => li.Value.Trim().Equals(itemName.Trim(), StringComparison.OrdinalIgnoreCase)).SingleOrDefault().Id;
        }

        public bool CreateAccessLink(Guid operationalObjectId, Guid operationObjectTypeId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.CreateAccessLink"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                accessLinkWebAPIName = accessLinkWebAPIName + "AccessLink/" + operationalObjectId + "/" + operationObjectTypeId;
                HttpResponseMessage result = httpHandler.GetAsync(accessLinkWebAPIName).Result;
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                string returnvalue = result.Content.ReadAsStringAsync().Result;
                return Convert.ToBoolean(returnvalue);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public async Task<CreateDeliveryPointModelDTO> CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.CreateAddressAndDeliveryPoint"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(postalAddressManagerWebAPIName + "postaladdress/savedeliverypointaddress/", addDeliveryPointDTO);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                return JsonConvert.DeserializeObject<CreateDeliveryPointModelDTO>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
            }
        }

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress as input</param>
        /// <returns>string</returns>
        public async Task<string> CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress)
        {
            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(postalAddressManagerWebAPIName + "postaladdress/nybduplicate/", objPostalAddress);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        public async Task<bool> CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress)
        {
            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(postalAddressManagerWebAPIName + "postaladdress/duplicatedeliverypoint/", objPostalAddress);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            return Convert.ToBoolean(result.Content.ReadAsStringAsync().Result);
        }

        /// <summary>
        /// Method to create block sequence for delivery point
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateBlockSequenceForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.CreateBlockSequenceForDeliveryPoint"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                blockSequenceWebAPIName = blockSequenceWebAPIName + "deliveryroute/deliverypointsequence/" + deliveryRouteId + "/" + deliveryPointId + "/";
                HttpResponseMessage result = await httpHandler.GetAsync(blockSequenceWebAPIName);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                string returnvalue = result.Content.ReadAsStringAsync().Result;
                return Convert.ToBoolean(returnvalue);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
            }
        }

        #endregion public methods
    }
}