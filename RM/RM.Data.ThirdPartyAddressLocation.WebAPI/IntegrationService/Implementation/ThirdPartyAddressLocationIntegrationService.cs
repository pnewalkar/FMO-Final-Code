using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Utils;

/// <summary>
/// Class definition for the Third Party integration Service members
/// </summary>
namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService
{
    public class ThirdPartyAddressLocationIntegrationService : IThirdPartyAddressLocationIntegrationService
    {
        private const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        private const string UnitManagerDataWebAPIName = "UnitManagerDataWebAPIName";
        private const string PostalAddressManagerWebAPIName = "PostalAddressManagerWebAPIName";
        private const string DeliveryPointManagerDataWebAPIName = "DeliveryPointManagerDataWebAPIName";
        private const string NotificationManagerWebAPIName = "NotificationManagerWebAPIName";

        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public ThirdPartyAddressLocationIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            // Store injected dependencies
            this.httpHandler = httpHandler;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public async Task<bool> DeliveryPointExists(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.DeliveryPointExists"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.DeliveryPointExists;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString()));
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                bool deliveryPointExists = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return deliveryPointExists;
            }
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public async Task<DeliveryPointDTO> GetDeliveryPointByUDPRNForThirdParty(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointByUDPRNForThirdParty"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.GetDeliveryPointByUDPRNForThirdParty;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString()));
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                DeliveryPointDTO deliveryPointDTO = JsonConvert.DeserializeObject<DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return deliveryPointDTO;
            }
        }

        /// <summary>
        ///  Retreive GUID for specified category
        /// </summary>
        /// <param name="strCategoryname">categoryname</param>
        /// <param name="strRefDataName">Reference data Name</param>
        /// <returns>GUID</returns>
        public async Task<Guid> GetReferenceDataId(string strCategoryname, string strRefDataName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataId"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.GetReferenceDataId;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(ReferenceDataWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, strCategoryname));
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<string, SimpleListDTO> simpleListDTO = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
                var getReferenceDataId = simpleListDTO.Item2.ListItems.Where(li => li.Value.Trim().Equals(strRefDataName.Trim(), StringComparison.OrdinalIgnoreCase)).SingleOrDefault().Id;
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return getReferenceDataId;
            }
        }

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDTO as DTO</param>
        /// <returns>updated delivery point</returns>
        public async Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateDeliveryPointLocationOnUDPRN"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.UpdateDeliveryPointLocationOnUDPRN;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(serviceUrl + route, JsonConvert.SerializeObject(deliveryPointDTO, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return status;
            }
        }

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        public async Task<bool> CheckIfNotificationExists(int uDPRN, string action)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.CheckIfNotificationExists"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.CheckIfNotificationExists;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(NotificationManagerWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString(), action));
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                bool notificationExists = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return notificationExists;
            }
        }

        /// <summary>
        /// Delete the notification based on the UDPRN and the action
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>Task<int></returns>
        public async Task<int> DeleteNotificationbyUDPRNAndAction(int uDPRN, string action)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.DeleteNotificationbyUDPRNAndAction"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.DeleteNotificationbyUDPRNAndAction;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(NotificationManagerWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.DeleteAsync(string.Format(serviceUrl + route, uDPRN.ToString(), action));
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return status;
            }
        }

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.AddNewNotification"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.AddNewNotification;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(NotificationManagerWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.PostAsJsonAsync<NotificationDTO>(serviceUrl + route, notificationDTO);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return status;
            }
        }

        /// <summary>
        /// Get the PostCodeSectorDTO on UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO</returns>
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetPostCodeSectorByUDPRN"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.GetPostCodeSectorByUDPRN;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(UnitManagerDataWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString()));
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                PostCodeSectorDTO postcodeSectorDTO = JsonConvert.DeserializeObject<PostCodeSectorDTO>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postcodeSectorDTO;
            }
        }

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDTO> GetPostalAddress(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetPostalAddress"))
            {
                string method = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string methodName = ThirdPartyAddressLocationConstants.GetPostalAddress;
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(PostalAddressManagerWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString()));
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                PostalAddressDTO postalAddressDTO = JsonConvert.DeserializeObject<PostalAddressDTO>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(method + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postalAddressDTO;
            }
        }

        /// <summary>
        /// Get PAF address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDTO> GetPAFAddress(int uDPRN)
        {
            string methodName = MethodHelper.GetActualAsyncMethodName();
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(PostalAddressManagerWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString()));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            PostalAddressDTO postalAddressDTO = JsonConvert.DeserializeObject<PostalAddressDTO>(result.Content.ReadAsStringAsync().Result);
            return postalAddressDTO;
        }

        /// <summary>
        /// Get Delivery Point details depending on the UDPRN
        /// </summary>
        /// <param name="addressId">Postal Address id</param>
        /// <returns>returns DeliveryPoint object</returns>
        public async Task<DeliveryPointDTO> GetDeliveryPointByPostalAddress(Guid addressId)
        {
            try
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
                HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, addressId));
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                DeliveryPointDTO deliveryPointDTO = JsonConvert.DeserializeObject<DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);
                return deliveryPointDTO;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Delete Delivery Point details depending on the DeliveryPoint id
        /// </summary>
        /// <param name="addressId">Delivery Point id</param>
        /// <returns>returns whether Delivery Point is deleted or not</returns>
        public async Task<bool> DeleteDeliveryPoint(Guid id)
        {
            string methodName = MethodHelper.GetActualAsyncMethodName();
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.DeleteAsync(string.Format(serviceUrl + route, id));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            bool status = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
            return status;
        }

        /// <summary>
        /// Insert new Delivery Point details depending on the DeliveryPoint id
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery Point object</param>
        /// <returns>returns whether Delivery Point is created or not</returns>
        public async Task<bool> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
            string methodName = MethodHelper.GetActualAsyncMethodName();
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);

            // method logic here
            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(serviceUrl + route, JsonConvert.SerializeObject(objDeliveryPoint, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
            if (!result.IsSuccessStatusCode)
            {
                // Log error with statuscode
                var responseContent = result.ReasonPhrase;
                return false;
            }

            bool isDeliveryPointCreated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
            return Convert.ToBoolean(isDeliveryPointCreated);
        }

        /// <summary>
        /// Get the reference data category details based on the list of categores
        /// </summary>
        /// <param name="listNames">Category names</param>
        /// <returns>returns Category details</returns>
        public async Task<List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> listReferenceCategories = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();

            string methodName = MethodHelper.GetActualAsyncMethodName();
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(ReferenceDataWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(serviceUrl + route, listNames);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);

            listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));
            return listReferenceCategories;
        }

        /// <summary>
        /// Update the Notification By UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <param name="oldAction">old action</param>
        /// <param name="newAction">new action</param>
        /// <returns></returns>
        public async Task<bool> UpdateNotificationByUDPRN(int udprn, string oldAction, string newAction)
        {
            string methodName = MethodHelper.GetActualAsyncMethodName();
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(NotificationManagerWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);

            HttpResponseMessage result = await httpHandler.PutAsJsonAsync(string.Format(serviceUrl + route, udprn.ToString(), oldAction), newAction);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            bool isUpdated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);

            return isUpdated;
        }
    }
}