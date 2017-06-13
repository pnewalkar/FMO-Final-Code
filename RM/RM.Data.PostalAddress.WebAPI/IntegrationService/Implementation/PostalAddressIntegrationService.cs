using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;

namespace RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Implementation
{
    /// <summary>
    /// Integration service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressIntegrationService : IPostalAddressIntegrationService
    {
        #region Property Declarations

        private string referenceDataWebAPIName = string.Empty;
        private string deliveryPointManagerWebAPIName = string.Empty;
        private string NotificationManagerDataWebAPIName = string.Empty;
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
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.ReferenceDataWebAPIName).ToString() : string.Empty;
            this.deliveryPointManagerWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryPointManagerDataWebAPIName).ToString() : string.Empty;
            this.NotificationManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.NotificationManagerDataWebAPIName).ToString() : string.Empty;
            this.addressLocationManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.AddressLocationManagerDataWebAPIName).ToString() : string.Empty;
            this.unitManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.UnitManagerDataWebAPIName).ToString() : string.Empty;
            this.notificationManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.NotificationManagerDataWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        #region public methods

        public async Task<Guid> GetReferenceDataGuId(string categoryName, string itemName)
        {
            Guid referenceId = Guid.Empty;
            HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "simpleLists?listName=" + categoryName);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            Tuple<string, SimpleListDTO> apiResult = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
            if (apiResult != null && apiResult.Item2 != null && apiResult.Item2.ListItems != null && apiResult.Item2.ListItems.Count > 0)
            {
                referenceId = apiResult.Item2.ListItems.Where(n => n.Value.Equals(itemName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault().Id;
            }

            return referenceId;
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<ReferenceDataCategoryDTO> GetReferenceDataSimpleLists(string listName)
        {
            ReferenceDataCategoryDTO referenceCategories = new ReferenceDataCategoryDTO();

            HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "simpleLists?listName=" + listName);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            Tuple<string, SimpleListDTO> apiResult = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);

            referenceCategories = ReferenceDataHelper.MapDTO(apiResult.Item2);
            return referenceCategories;
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "simpleLists", listNames);
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
        /// This method will call Delivery point web api which is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public async Task<DeliveryPointDTO> GetDeliveryPointByUDPRN(int udprn)
        {
            //using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointByUDPRN"))
            //{
            string methodName = MethodBase.GetCurrentMethod().Name;

            // method logic here
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerWebAPIName + "deliverypoint/batch/" + udprn);
            if (!result.IsSuccessStatusCode)
            {
                // Log error with statuscode
                var responseContent = string.Format(Constants.ResponseContent, result.StatusCode.GetHashCode(), result.ReasonPhrase);
                this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                return null;
            }

            DeliveryPointDTO deliveryPoint = JsonConvert.DeserializeObject<DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);
            return deliveryPoint;

            // }
        }

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        public async Task<DeliveryPointDTO> GetDeliveryPointByPostalAddress(Guid addressId)
        {
            //using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointByPostalAddress"))
            //{
            string methodName = MethodBase.GetCurrentMethod().Name;

            // method logic here
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerWebAPIName + "deliverypoint/addressId:" + addressId);
            if (!result.IsSuccessStatusCode)
            {
                // Log error with statuscode
                var responseContent = string.Format(Constants.ResponseContent, result.StatusCode.GetHashCode(), result.ReasonPhrase);
                this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                return null;
            }

            DeliveryPointDTO deliveryPoint = JsonConvert.DeserializeObject<DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);
            return deliveryPoint;

            // }
        }

        /// <summary>
        /// This method will call Delivery point web api which is used to insert delivery point.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        public async Task<bool> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.InsertDeliveryPoint"))
            {
                string methodName = MethodHelper.GetRealMethodFromAsyncMethod(MethodBase.GetCurrentMethod()).Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);
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
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);
                return Convert.ToBoolean(isDeliveryPointCreated);
            }
        }

        /// <summary> Add new notification to the database </summary> <param
        /// name="notificationDTO">NotificationDTO object</param> <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.AddNewNotification"))
            {
                string methodName = MethodHelper.GetRealMethodFromAsyncMethod(MethodBase.GetCurrentMethod()).Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);
                // method logic here
                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(NotificationManagerDataWebAPIName + "notifications/add", notificationDTO);
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = string.Format(Constants.ResponseContent, result.StatusCode.GetHashCode(), result.ReasonPhrase);
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return 0;
                }

                int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return status;
            }
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        public async Task<AddressLocationDTO> GetAddressLocationByUDPRN(int udprn)
        {
            //using (loggingHelper.RMTraceManager.StartTrace("Integration.GetAddressLocationByUDPRN"))
            //{
            string methodName = MethodBase.GetCurrentMethod().Name;

            // method logic here
            HttpResponseMessage result = await httpHandler.GetAsync(addressLocationManagerDataWebAPIName + "addresslocation/udprn:" + udprn);
            if (!result.IsSuccessStatusCode)
            {
                // Log error with statuscode
                var responseContent = string.Format(Constants.ResponseContent, result.StatusCode.GetHashCode(), result.ReasonPhrase);
                this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                return null;
            }

            AddressLocationDTO addressLocation = JsonConvert.DeserializeObject<AddressLocationDTO>(result.Content.ReadAsStringAsync().Result);
            return addressLocation;

            // }
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode">Post Code</param>
        /// <returns>Post code ID</returns>
        public async Task<Guid> GetPostCodeID(string postCode)
        {
            //using (loggingHelper.RMTraceManager.StartTrace("Integration.GetPostCodeID"))
            //{
            string methodName = MethodBase.GetCurrentMethod().Name;

            // method logic here
            HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcode/guid/" + postCode);
            if (!result.IsSuccessStatusCode)
            {
                // Log error with statuscode
                var responseContent = string.Format(Constants.ResponseContent, result.StatusCode.GetHashCode(), result.ReasonPhrase);
                this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                return Guid.Empty;
            }

            Guid postCodeGuid = JsonConvert.DeserializeObject<Guid>(result.Content.ReadAsStringAsync().Result);
            return postCodeGuid;

            // }
        }

        #endregion public methods
    }
}