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
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(DeliveryPointExists);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);
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
                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);

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
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(GetDeliveryPointByUDPRNForThirdParty);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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
                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
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
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(GetReferenceDataId);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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
                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
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
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(UpdateDeliveryPointLocationOnUDPRN);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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
                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
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
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(CheckIfNotificationExists);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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
                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
                return notificationExists;
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
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(AddNewNotification);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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
                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
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
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(GetPostCodeSectorByUDPRN);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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
                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
                return postcodeSectorDTO;
            }
        }


        /// <summary>
        /// Get Delivery Point details depending on the UDPRN
        /// </summary>
        /// <param name="addressId">Postal Address id</param>
        /// <returns>returns DeliveryPoint object</returns>
        public async Task<DeliveryPointDTO> GetDeliveryPointByPostalAddress(Guid addressId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointByPostalAddress"))
            {
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(GetDeliveryPointByPostalAddress);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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
                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
                return deliveryPointDTO;
            }
        }

        /// <summary>
        /// Update the Notification By UDPRN
        /// </summary>
        /// <param name="listNames">Category names</param>
        /// <returns>returns Category details</returns>
        public async Task<List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(GetReferenceDataSimpleLists);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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

                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
                return listReferenceCategories;
            }
        }



        /// <summary>
        /// Update Delivery Point by Id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <param name="oldAction">old action</param>
        /// <param name="newAction">new action</param>
        /// <returns></returns>
        public async Task<bool> UpdateNotificationByUDPRN(int udprn, string oldAction, string newAction)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateNotificationByUDPRN"))
            {
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(UpdateNotificationByUDPRN);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

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

                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
                return isUpdated;
            }
        }



        /// <summary>
        /// Update Delivery Point by Id
        /// </summary>
        /// <param name="deliveryPointDTO">Delivery Point DTO</param>
        /// <returns>whether DP has been updated successfully</returns>
        public async Task<bool> UpdateDeliveryPointById(DeliveryPointDTO deliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateDeliveryPointById"))
            {
                string method = typeof(ThirdPartyAddressLocationIntegrationService) + "." + nameof(UpdateDeliveryPointById);
                loggingHelper.LogMethodEntry(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId);

                string methodName = MethodHelper.GetActualAsyncMethodName();
                string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName);
                string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);

                string serializedDeliveryPoint = JsonConvert.SerializeObject(deliveryPointDTO);

                HttpResponseMessage result = await httpHandler.PutAsJsonAsync(serviceUrl + route, serializedDeliveryPoint);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                bool isUpdated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(method, LoggerTraceConstants.ThirdPartyAddressLocationAPIPriority, LoggerTraceConstants.ThirdPartyAddressLocationIntegrationServiceMethodExitEventId);
                return isUpdated;
            }
        }
    }
}