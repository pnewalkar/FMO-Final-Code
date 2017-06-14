using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.Operational.SearchManager.WebAPI.Integration
{
    /// <summary>
    /// Search Functionality Integration Service class
    /// </summary>
    /// <seealso cref="RM.Operational.SearchManager.WebAPI.Integration.ISearchIntegrationService"/>
    public class SearchIntegrationService : ISearchIntegrationService
    {
        #region Property Declarations

        private string deliveryPointManagerDataWebAPIName = string.Empty;
        private string unitManagerDataWebAPIName = string.Empty;
        private string deliveryRouteManagerDataWebAPIName = string.Empty;
        private string networkManagerDataWebAPIName = string.Empty;

        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);

        #endregion Property Declarations

        #region Constructor

        public SearchIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper)
        {
            this.httpHandler = httpHandler;
            this.configurationHelper = configurationHelper;
            this.deliveryPointManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryPointManagerDataWebAPIName).ToString() : string.Empty;
            this.unitManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.UnitManagerDataWebAPIName).ToString() : string.Empty;
            this.deliveryRouteManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryRouteManagerWebAPIName).ToString() : string.Empty;
            this.networkManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.NetworkManagerDataWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        /// <summary>
        /// Fetches the delivery route for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns></returns>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForBasicSearch(string searchText)
        {
            string methodName = MethodHelper.GetActualAsyncMethodName();
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryRouteManagerDataWebAPIName + "deliveryroutes/basic/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<DeliveryRouteDTO> networkLinks = JsonConvert.DeserializeObject<List<DeliveryRouteDTO>>(result.Content.ReadAsStringAsync().Result);

            return networkLinks;
        }

        /// <summary>
        /// Gets the delivery route count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns></returns>
        public async Task<int> GetDeliveryRouteCount(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryRouteManagerDataWebAPIName + "deliveryroutes/count/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            int networkLinks = result.Content.ReadAsStringAsync().Result.Equals("[]") ? 0 : Convert.ToInt32(result.Content.ReadAsStringAsync().Result);

            return networkLinks;
        }

        /// <summary>
        /// Fetches the post code unit for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcodes/basic/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<PostCodeDTO> networkLinks = JsonConvert.DeserializeObject<List<PostCodeDTO>>(result.Content.ReadAsStringAsync().Result);

            return networkLinks;
        }

        /// <summary>
        /// Gets the post code unit count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<int> GetPostCodeUnitCount(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcodes/count/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            int postCodes = result.Content.ReadAsStringAsync().Result.Equals("[]") ? 0 : Convert.ToInt32(result.Content.ReadAsStringAsync().Result);

            return postCodes;
        }

        /// <summary>
        /// Fetches the delivery points for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns></returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerDataWebAPIName + "deliverypoints/basic/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<DeliveryPointDTO> deliveryPoints = JsonConvert.DeserializeObject<List<DeliveryPointDTO>>(result.Content.ReadAsStringAsync().Result);

            return deliveryPoints;
        }

        public async Task<int> GetDeliveryPointsCount(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerDataWebAPIName + "deliverypoints/count/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            int deliveryPoints = result.Content.ReadAsStringAsync().Result.Equals("[]") ? 0 : Convert.ToInt32(result.Content.ReadAsStringAsync().Result);

            return deliveryPoints;
        }

        /// <summary>
        /// Fetches the street names for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns></returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "streetnames/basic/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<StreetNameDTO> streetNames = JsonConvert.DeserializeObject<List<StreetNameDTO>>(result.Content.ReadAsStringAsync().Result);

            return streetNames;
        }

        /// <summary>
        /// Gets the street name count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<int> GetStreetNameCount(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "streetnames/count/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            int streetNames = result.Content.ReadAsStringAsync().Result.Equals("[]") ? 0 : Convert.ToInt32(result.Content.ReadAsStringAsync().Result);

            return streetNames;
        }

        #region Advance Search

        /// <summary>
        /// Fetches the post code unit for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcodes/advance/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<PostCodeDTO> postCodes = JsonConvert.DeserializeObject<List<PostCodeDTO>>(result.Content.ReadAsStringAsync().Result);

            return postCodes;
        }

        /// <summary>
        /// Fetches the delivery route for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryRouteManagerDataWebAPIName + "deliveryroutes/advance/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<DeliveryRouteDTO> deliveryRoutes = JsonConvert.DeserializeObject<List<DeliveryRouteDTO>>(result.Content.ReadAsStringAsync().Result);

            return deliveryRoutes;
        }

        /// <summary>
        /// Fetches the street names for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "streetnames/advance/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<StreetNameDTO> streetNames = JsonConvert.DeserializeObject<List<StreetNameDTO>>(result.Content.ReadAsStringAsync().Result);

            return streetNames;
        }

        /// <summary>
        /// Fetches the delivery points for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerDataWebAPIName + "deliverypoints/advance/" + searchText);

            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<DeliveryPointDTO> deliveryPoints = JsonConvert.DeserializeObject<List<DeliveryPointDTO>>(result.Content.ReadAsStringAsync().Result);

            return deliveryPoints;
        }

        #endregion Advance Search
    }
}