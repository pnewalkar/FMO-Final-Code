using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace RM.Operational.SearchManager.WebAPI.Integration
{
    /// <summary>
    /// Search Functionality Integration Service class
    /// </summary>
    /// <seealso cref="RM.Operational.SearchManager.WebAPI.Integration.ISearchIntegrationService"/>
    public class SearchIntegrationService : ISearchIntegrationService
    {
        private const string NetworkManagerDataWebAPIName = "NetworkManagerDataWebAPIName";
        private const string DeliveryRouteManagerWebAPIName = "DeliveryRouteManagerWebAPIName";
        private const string UnitManagerDataWebAPIName = "UnitManagerDataWebAPIName";
        private const string DeliveryPointManagerDataWebAPIName = "DeliveryPointManagerDataWebAPIName";

        #region Property Declarations

        private string deliveryPointManagerDataWebAPIName = string.Empty;
        private string unitManagerDataWebAPIName = string.Empty;
        private string deliveryRouteManagerDataWebAPIName = string.Empty;
        private string networkManagerDataWebAPIName = string.Empty;

        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Property Declarations

        #region Constructor

        public SearchIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.configurationHelper = configurationHelper;
            this.deliveryPointManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName).ToString() : string.Empty;
            this.unitManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(UnitManagerDataWebAPIName).ToString() : string.Empty;
            this.deliveryRouteManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryRouteManagerWebAPIName).ToString() : string.Empty;
            this.networkManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(NetworkManagerDataWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
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
            using (loggingHelper.RMTraceManager.StartTrace("Integration.FetchDeliveryRouteForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(deliveryRouteManagerDataWebAPIName + "deliveryroutes/basic/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<DeliveryRouteDTO> networkLinks = JsonConvert.DeserializeObject<List<DeliveryRouteDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return networkLinks;
            }
        }

        /// <summary>
        /// Gets the delivery route count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns></returns>
        public async Task<int> GetDeliveryRouteCount(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryRouteCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(deliveryRouteManagerDataWebAPIName + "deliveryroutes/count/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                int networkLinks = result.Content.ReadAsStringAsync().Result.Equals("[]") ? 0 : Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return networkLinks;
            }
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
            using (loggingHelper.RMTraceManager.StartTrace("Integration.FetchPostCodeUnitForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcodes/basic/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<PostCodeDTO> networkLinks = JsonConvert.DeserializeObject<List<PostCodeDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return networkLinks;
            }
        }

        /// <summary>
        /// Gets the post code unit count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<int> GetPostCodeUnitCount(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetPostCodeUnitCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcodes/count/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                int postCodes = result.Content.ReadAsStringAsync().Result.Equals("[]") ? 0 : Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return postCodes;
            }
        }

        /// <summary>
        /// Fetches the delivery points for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns></returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.FetchDeliveryPointsForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerDataWebAPIName + "deliverypoints/basic/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<DeliveryPointDTO> deliveryPoints = JsonConvert.DeserializeObject<List<DeliveryPointDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return deliveryPoints;
            }
        }

        public async Task<int> GetDeliveryPointsCount(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointsCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerDataWebAPIName + "deliverypoints/count/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                int deliveryPoints = result.Content.ReadAsStringAsync().Result.Equals("[]") ? 0 : Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return deliveryPoints;
            }
        }

        /// <summary>
        /// Fetches the street names for basic search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="userUnit">The user unit.</param>
        /// <returns></returns>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForBasicSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.FetchStreetNamesForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "streetnames/basic/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<StreetNameDTO> streetNames = JsonConvert.DeserializeObject<List<StreetNameDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return streetNames;
            }
        }

        /// <summary>
        /// Gets the street name count.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<int> GetStreetNameCount(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetStreetNameCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "streetnames/count/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                int streetNames = result.Content.ReadAsStringAsync().Result.Equals("[]") ? 0 : Convert.ToInt32(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return streetNames;
            }
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
            using (loggingHelper.RMTraceManager.StartTrace("Integration.FetchPostCodeUnitForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(unitManagerDataWebAPIName + "postcodes/advance/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<PostCodeDTO> postCodes = JsonConvert.DeserializeObject<List<PostCodeDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return postCodes;
            }
        }

        /// <summary>
        /// Fetches the delivery route for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<List<DeliveryRouteDTO>> FetchDeliveryRouteForAdvanceSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.FetchDeliveryRouteForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(deliveryRouteManagerDataWebAPIName + "deliveryroutes/advance/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<DeliveryRouteDTO> deliveryRoutes = JsonConvert.DeserializeObject<List<DeliveryRouteDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return deliveryRoutes;
            }
        }

        /// <summary>
        /// Fetches the street names for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<List<StreetNameDTO>> FetchStreetNamesForAdvanceSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.FetchStreetNamesForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "streetnames/advance/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<StreetNameDTO> streetNames = JsonConvert.DeserializeObject<List<StreetNameDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return streetNames;
            }
        }

        /// <summary>
        /// Fetches the delivery points for advance search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="ServiceException"></exception>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.FetchDeliveryPointsForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodEntryEventId, LoggerTraceConstants.Title);

                HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerDataWebAPIName + "deliverypoints/advance/" + searchText);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<DeliveryPointDTO> deliveryPoints = JsonConvert.DeserializeObject<List<DeliveryPointDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SearchManagerAPIPriority, LoggerTraceConstants.SearchManagerIntegrationServiceMethodExitEventId, LoggerTraceConstants.Title);

                return deliveryPoints;
            }
        }

        #endregion Advance Search
    }
}