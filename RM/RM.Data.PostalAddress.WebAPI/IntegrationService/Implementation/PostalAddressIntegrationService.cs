﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Implementation
{
    /// <summary>
    /// Integration service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressIntegrationService : IPostalAddressIntegrationService
    {
        private const string ResponseContent = "Status Code: {0} Reason: {1} ";
        private const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        private const string UnitManagerDataWebAPIName = "UnitManagerDataWebAPIName";
        private const string DeliveryPointManagerDataWebAPIName = "DeliveryPointManagerDataWebAPIName";
        private const string NotificationManagerDataWebAPIName = "NotificationManagerDataWebAPIName";
        private const string AddressLocationManagerDataWebAPIName = "AddressLocationManagerDataWebAPIName";

        #region Property Declarations

        private string referenceDataWebAPIName = string.Empty;
        private string deliveryPointManagerWebAPIName = string.Empty;
        private string addressLocationManagerDataWebAPIName = string.Empty;
        private string unitManagerDataWebAPIName = string.Empty;
        private string notificationManagerDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Property Declarations

        #region Constructor

        public PostalAddressIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(ReferenceDataWebAPIName).ToString() : string.Empty;
            this.deliveryPointManagerWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName).ToString() : string.Empty;
            this.addressLocationManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(AddressLocationManagerDataWebAPIName).ToString() : string.Empty;
            this.unitManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(UnitManagerDataWebAPIName).ToString() : string.Empty;
            this.notificationManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(NotificationManagerDataWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        #region public methods

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

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> GetReferenceDataSimpleLists(string listName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO referenceCategories = new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO();


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

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

            List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> listReferenceCategories = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "simpleLists", listNames);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);

                listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return listReferenceCategories;
            }
        }

        public async Task<List<CommonLibrary.EntityFramework.DTO.PostCodeDTO>> GetPostcodes(Guid unitGuid, List<Guid> postcodeGuids)
        {
            List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> postcodes = new List<CommonLibrary.EntityFramework.DTO.PostCodeDTO>();

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(unitManagerDataWebAPIName + "postcode/search/" + unitGuid, postcodeGuids);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            postcodes = JsonConvert.DeserializeObject<List<CommonLibrary.EntityFramework.DTO.PostCodeDTO>>(result.Content.ReadAsStringAsync().Result);

            return postcodes;
        }


        public async Task<CommonLibrary.EntityFramework.DTO.PostCodeDTO> GetSelecetdPostcode(Guid postcodeGuid, Guid unitGuid)
        {
            CommonLibrary.EntityFramework.DTO.PostCodeDTO postcode = new CommonLibrary.EntityFramework.DTO.PostCodeDTO();

            HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcode/select/" + postcodeGuid +"/" + unitGuid);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            postcode = JsonConvert.DeserializeObject<CommonLibrary.EntityFramework.DTO.PostCodeDTO>(result.Content.ReadAsStringAsync().Result);

            return postcode;
        }

        ///// <summary>
        ///// This method will call Delivery point web api which is used to fetch Delivery Point by udprn.
        ///// </summary>
        ///// <param name="addressGuid">addressGuid instead of udprn</param>
        ///// <returns>DeliveryPointDTO</returns>
        //public async Task<DTO.DeliveryPointDTO> GetDeliveryPointByID(Guid addressGuid)
        //{
        //    //using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointByID"))
        //    //{
        //    string methodName = MethodBase.GetCurrentMethod().Name;

        //    // method logic here
        //    HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerWebAPIName + "deliverypoint/batch/addressGuid:" + addressGuid);
        //    if (!result.IsSuccessStatusCode)
        //    {
        //        // Log error with statuscode
        //        var responseContent = string.Format(Constants.ResponseContent, result.StatusCode.GetHashCode(), result.ReasonPhrase);
        //        this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
        //        return null;
        //    }

        //    DTO.DeliveryPointDTO deliveryPoint = JsonConvert.DeserializeObject<DTO.DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);
        //    return deliveryPoint;

        //    // }
        //}

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        public async Task<DTO.DeliveryPointDTO> GetDeliveryPointByPostalAddress(Guid addressId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointByPostalAddress"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // method logic here
                HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerWebAPIName + "deliverypoint/addressId:" + addressId);
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return null;
                }

                DTO.DeliveryPointDTO deliveryPoint = JsonConvert.DeserializeObject<DTO.DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);
                return deliveryPoint;

            }
        }

        /// <summary>
        /// This method will call Delivery point web api which is used to insert delivery point.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        public async Task<bool> InsertDeliveryPoint(DTO.DeliveryPointDTO objDeliveryPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.InsertDeliveryPoint"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // method logic here
                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryPointManagerWebAPIName + "deliverypoint/batch/", JsonConvert.SerializeObject(objDeliveryPoint, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return false;
                }

                bool isDeliveryPointCreated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return Convert.ToBoolean(isDeliveryPointCreated);

            }
        }

        /// <summary>
        /// This method will call Delivery point web api which is used to 
        /// update delivery point for resp PostalAddress which has type <USR>.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateDeliveryPoint(Guid addressId, Guid deliveryPointUseIndicatorPAF)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateDeliveryPoint"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // method logic here
                HttpResponseMessage result = await httpHandler.PutAsJsonAsync(deliveryPointManagerWebAPIName + "deliverypoint/batch/" + addressId, deliveryPointUseIndicatorPAF);
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return false;
                }

                bool isDeliveryPointCreated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return Convert.ToBoolean(isDeliveryPointCreated);

            }
        }

        /// <summary> Add new notification to the database </summary> <param
        /// name="notificationDTO">NotificationDTO object</param> <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.AddNewNotification"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // method logic here
                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(NotificationManagerDataWebAPIName + "notifications/add", notificationDTO);
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return 0;
                }

                int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return status;

            }
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        public async Task<DTO.AddressLocationDTO> GetAddressLocationByUDPRN(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetAddressLocationByUDPRN"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // method logic here
                HttpResponseMessage result = await httpHandler.GetAsync(addressLocationManagerDataWebAPIName + "addresslocation/udprn:" + udprn);
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return null;
                }

                DTO.AddressLocationDTO addressLocation = JsonConvert.DeserializeObject<DTO.AddressLocationDTO>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return addressLocation;

            }
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode">Post Code</param>
        /// <returns>Post code ID</returns>
        public async Task<Guid> GetPostCodeID(string postCode)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetPostCodeID"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // method logic here
                HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcode/guid/" + postCode);
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return Guid.Empty;
                }

                Guid postCodeGuid = JsonConvert.DeserializeObject<Guid>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postCodeGuid;

            }
        }

        public async Task<bool> CheckIfNotificationExists(int uDPRN, string action)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(string.Format(notificationManagerDataWebAPIName + "/Notifications/check/{0}/{1}", uDPRN.ToString(), action));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            bool notificationExists = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
            return notificationExists;
        }

        public async Task<bool> UpdateNotificationByUDPRN(int udprn, string oldAction, string newAction)
        {
            HttpResponseMessage result = await httpHandler.PutAsJsonAsync(string.Format(notificationManagerDataWebAPIName + "/notifications/location/{0}/{1}", udprn.ToString(), oldAction), newAction);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            bool isUpdated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);

            return isUpdated;
        }

        public async Task<bool> UpdateNotificationMessageByUDPRN(int udprn, string action, string message)
        {
            HttpResponseMessage result = await httpHandler.PutAsJsonAsync(string.Format(notificationManagerDataWebAPIName + "notifications/postaladdress/{0}/{1}", udprn.ToString(), action), message);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            bool isUpdated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);

            return isUpdated;
        }

        #endregion public methods
    }
}