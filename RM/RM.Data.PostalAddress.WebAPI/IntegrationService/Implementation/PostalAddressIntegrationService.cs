using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware.GeoJsonData;
using RM.Data.PostalAddress.WebAPI.Utils;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
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
        private string addressLocationManagerDataWebAPIName = string.Empty;
        private string unitManagerDataWebAPIName = string.Empty;
        private string notificationManagerDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.PostalAddressAPIPriority;
        private int entryEventId = LoggerTraceConstants.PostalAddressIntegrationServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.PostalAddressIntegrationServiceMethodExitEventId;

        #endregion Property Declarations

        #region Constructor

        public PostalAddressIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PostalAddressConstants.ReferenceDataWebAPIName).ToString() : string.Empty;
            this.deliveryPointManagerWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PostalAddressConstants.DeliveryPointManagerDataWebAPIName).ToString() : string.Empty;
            this.addressLocationManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PostalAddressConstants.AddressLocationManagerDataWebAPIName).ToString() : string.Empty;
            this.unitManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PostalAddressConstants.UnitManagerDataWebAPIName).ToString() : string.Empty;
            this.notificationManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PostalAddressConstants.NotificationManagerDataWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        #region public methods

        public async Task<Guid> GetReferenceDataGuId(string categoryName, string itemName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataGuId"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetReferenceDataGuId);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return referenceId;
            }
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> GetReferenceDataSimpleLists(string listName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetReferenceDataSimpleLists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO referenceCategories = new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO();

                HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "simpleLists?listName=" + listName);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<string, SimpleListDTO> apiResult = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);

                referenceCategories = ReferenceDataHelper.MapDTO(apiResult.Item2);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return referenceCategories;
            }
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetReferenceDataSimpleLists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> listReferenceCategories = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "simpleLists", listNames);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);

                listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return listReferenceCategories;
            }
        }

        public async Task<List<PostcodeDTO>> GetPostcodes(Guid unitGuid, List<Guid> postcodeGuids)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetPostcodes"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetPostcodes);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<PostcodeDTO> postcodes = new List<PostcodeDTO>();

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(unitManagerDataWebAPIName + "postcode/search/" + unitGuid, postcodeGuids);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                postcodes = JsonConvert.DeserializeObject<List<PostcodeDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return postcodes;
            }
        }

        public async Task<PostcodeDTO> GetSelecetdPostcode(Guid postcodeGuid, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetSelecetdPostcode"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetSelecetdPostcode);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                PostcodeDTO postcode = new PostcodeDTO();

                HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcode/select/" + postcodeGuid + "/" + unitGuid);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                postcode = JsonConvert.DeserializeObject<PostcodeDTO>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return postcode;
            }
        }

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        public async Task<DTO.DeliveryPointDTO> GetDeliveryPointByPostalAddress(Guid addressId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointByPostalAddress"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetDeliveryPointByPostalAddress);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(InsertDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                // method logic here
                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryPointManagerWebAPIName + "deliverypoint/batch/", JsonConvert.SerializeObject(objDeliveryPoint, jsonSerializerSettings));
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return false;
                }

                bool isDeliveryPointCreated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Convert.ToBoolean(isDeliveryPointCreated);
            }
        }

        /// <summary>
        /// This method will call Delivery point web api which is used to
        /// update delivery point for resp PostalAddress which has type <USR>.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateDeliveryPoint(DTO.DeliveryPointDTO objDeliveryPoint) // (Guid addressId, Guid deliveryPointUseIndicatorPAF)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateDeliveryPoint"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(UpdateDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                // method logic here
                HttpResponseMessage result = await httpHandler.PutAsJsonAsync(deliveryPointManagerWebAPIName + "deliverypoint/batch/", JsonConvert.SerializeObject(objDeliveryPoint, jsonSerializerSettings));

                // (deliveryPointManagerWebAPIName + "deliverypoint/batch/" + addressId, deliveryPointUseIndicatorPAF);
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return false;
                }

                bool isDeliveryPointCreated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Convert.ToBoolean(isDeliveryPointCreated);
            }
        }

        /// <summary> Add new notification to the database </summary> <param
        /// name="notificationDTO">NotificationDTO object</param> <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.AddNewNotification"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(AddNewNotification);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                // method logic here
                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(notificationManagerDataWebAPIName + "notifications/add/", notificationDTO);
                if (!result.IsSuccessStatusCode)
                {
                    // Log error with statuscode
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(methodName + responseContent, TraceEventType.Error);
                    return 0;
                }

                int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetAddressLocationByUDPRN);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
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
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetPostCodeID);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return postCodeGuid;
            }
        }

        public async Task<bool> CheckIfNotificationExists(int uDPRN, string action)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.CheckIfNotificationExists"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(CheckIfNotificationExists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.GetAsync(string.Format(notificationManagerDataWebAPIName + "/Notifications/check/{0}/{1}", uDPRN.ToString(), action));
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                bool notificationExists = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return notificationExists;
            }
        }

        public async Task<bool> UpdateNotificationByUDPRN(int udprn, string oldAction, string newAction)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateNotificationByUDPRN"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(UpdateNotificationByUDPRN);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PutAsJsonAsync(string.Format(notificationManagerDataWebAPIName + "/notifications/location/{0}/{1}", udprn.ToString(), oldAction), newAction);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                bool isUpdated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isUpdated;
            }
        }

        public async Task<bool> UpdateNotificationMessageByUDPRN(int udprn, string action, string message)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateNotificationMessageByUDPRN"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(UpdateNotificationMessageByUDPRN);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.PutAsJsonAsync(string.Format(notificationManagerDataWebAPIName + "notifications/postaladdress/{0}/{1}", udprn.ToString(), action), message);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                bool isUpdated = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isUpdated;
            }
        }

        // <summary>
        /// Gets approx location based on the postal code.
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns>The approx location/</returns>
        public async Task<DbGeometry> GetApproxLocation(string postcode)
        {
            using (loggingHelper.RMTraceManager.StartTrace("IntegrationService.GetApproxLocation"))
            {
                string methodName = typeof(PostalAddressIntegrationService) + "." + nameof(GetApproxLocation);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcode/approxlocation/" + postcode);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                // DbGeometry approxLocation = JsonConvert.DeserializeObject<DbGeometry>(result.Content.ReadAsStringAsync().Result);
                DBGeometryObj locationObject = JsonConvert.DeserializeObject<DBGeometryObj>(result.Content.ReadAsStringAsync().Result);
                DbGeometry approxLocation = locationObject.dbGeometry;
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return approxLocation;
            }
        }

        #endregion public methods
    }
}