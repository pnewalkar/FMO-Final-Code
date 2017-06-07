using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;

namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService
{
    public class ThirdPartyAddressLocationIntegrationService : IThirdPartyAddressLocationIntegrationService
    {
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);

        public ThirdPartyAddressLocationIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper)
        {
            this.httpHandler = httpHandler;
            this.configurationHelper = configurationHelper;
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public async Task<bool> DeliveryPointExists(int uDPRN)
        {
            string methodName = Constants.DeliveryPointExists;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryPointManagerDataWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString()));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            bool deliveryPointExists = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
            return deliveryPointExists;
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public async Task<DeliveryPointDTO> GetDeliveryPointByUDPRNForThirdParty(int uDPRN)
        {
            string methodName = Constants.GetDeliveryPointByUDPRNForThirdParty;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryPointManagerDataWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString()));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            DeliveryPointDTO deliveryPointDTO = JsonConvert.DeserializeObject<DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);
            return deliveryPointDTO;
        }

        /// <summary>
        ///  Retreive GUID for specified category
        /// </summary>
        /// <param name="strCategoryname">categoryname</param>
        /// <param name="strRefDataName">Reference data Name</param>
        /// <returns>GUID</returns>
        public async Task<Guid> GetReferenceDataId(string strCategoryname, string strRefDataName)
        {
            string methodName = Constants.GetReferenceDataId;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.ReferenceDataWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, strCategoryname));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            Tuple<string, SimpleListDTO> simpleListDTO = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
            return simpleListDTO.Item2.ListItems.Where(li => li.Value.Trim().Equals(strRefDataName.Trim(), StringComparison.OrdinalIgnoreCase)).SingleOrDefault().Id;
        }

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDTO as DTO</param>
        /// <returns>updated delivery point</returns>
        public async Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDTO)
        {
            string methodName = Constants.UpdateDeliveryPointLocationOnUDPRN;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryPointManagerDataWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(serviceUrl + route, JsonConvert.SerializeObject(deliveryPointDTO, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
            return status;
        }

        /// <summary>
        /// Check if a notification exists for a given UDPRN id and action string.
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>boolean value</returns>
        public async Task<bool> CheckIfNotificationExists(int uDPRN, string action)
        {
            string methodName = Constants.CheckIfNotificationExists;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.NotificationManagerWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString(), action));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            bool notificationExists = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
            return notificationExists;
        }

        /// <summary>
        /// Delete the notification based on the UDPRN and the action
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <param name="action">action string</param>
        /// <returns>Task<int></returns>
        public async Task<int> DeleteNotificationbyUDPRNAndAction(int uDPRN, string action)
        {
            string methodName = Constants.DeleteNotificationbyUDPRNAndAction;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.NotificationManagerWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.DeleteAsync(string.Format(serviceUrl + route, uDPRN.ToString(), action));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
            return status;
        }

        /// <summary>
        /// Add new notification to the database
        /// </summary>
        /// <param name="notificationDTO">NotificationDTO object</param>
        /// <returns>Task<int></returns>
        public async Task<int> AddNewNotification(NotificationDTO notificationDTO)
        {
            string methodName = Constants.AddNewNotification;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.NotificationManagerWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.PostAsJsonAsync<NotificationDTO>(serviceUrl + route, notificationDTO);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            int status = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
            return status;
        }

        /// <summary>
        /// Get the PostCodeSectorDTO on UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO</returns>
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN)
        {
            string methodName = Constants.GetPostCodeSectorByUDPRN;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.UnitManagerDataWebAPIName);
            string route = configurationHelper.ReadAppSettingsConfigurationValues(methodName);
            HttpResponseMessage result = await httpHandler.GetAsync(string.Format(serviceUrl + route, uDPRN.ToString()));
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            PostCodeSectorDTO postcodeSectorDTO = JsonConvert.DeserializeObject<PostCodeSectorDTO>(result.Content.ReadAsStringAsync().Result);
            return postcodeSectorDTO;
        }

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDTO> GetPostalAddress(int uDPRN)
        {
            string methodName = Constants.GetPostalAddress;
            string serviceUrl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.PostalAddressManagerWebAPIName);
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
    }
}