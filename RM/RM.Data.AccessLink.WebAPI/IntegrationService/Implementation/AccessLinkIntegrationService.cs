using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
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

namespace RM.DataManagement.AccessLink.WebAPI.Integration
{
    public class AccessLinkIntegrationService : IAccessLinkIntegrationService
    {
        #region Member Variables

        private string referenceDataWebAPIName = string.Empty;
        private string networkManagerDataWebAPIName = string.Empty;
        private string deliveryPointManagerDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Property Declarations

        #region Constructor
        public AccessLinkIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.ReferenceDataWebAPIName).ToString() : string.Empty;
            this.networkManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.NetworkManagerDataWebAPIName).ToString() : string.Empty;
            this.deliveryPointManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryPointManagerDataWebAPIName).ToString() : string.Empty;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public async Task<Tuple<NetworkLinkDTO, SqlGeometry>> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName)
        {
            var operationalObjectPointJson = JsonConvert.SerializeObject(operationalObjectPoint, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(networkManagerDataWebAPIName + "/nearestnamedroad/" + streetName, operationalObjectPointJson);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = string.Concat("Status Code:" + result.StatusCode.GetHashCode(), " Reason:", result.ReasonPhrase);
                throw new ServiceException(responseContent);
            }

            Tuple<NetworkLinkDTO, DbGeometry> nearestNamedRoad = JsonConvert.DeserializeObject<Tuple<NetworkLinkDTO, DbGeometry>>(result.Content.ReadAsStringAsync().Result);

            return new Tuple<NetworkLinkDTO, SqlGeometry>(nearestNamedRoad.Item1, nearestNamedRoad.Item2?.ToSqlGeometry());
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public async Task<Tuple<NetworkLinkDTO, SqlGeometry>> GetNearestSegment(DbGeometry operationalObjectPoint)
        {
            var operationalObjectPointJson = JsonConvert.SerializeObject(operationalObjectPoint, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(networkManagerDataWebAPIName + "nearestsegment", operationalObjectPointJson);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = string.Concat("Status Code:" + result.StatusCode.GetHashCode(), " Reason:", result.ReasonPhrase);
                throw new ServiceException(responseContent);
            }

            Tuple<NetworkLinkDTO, DbGeometry> nearestSegment = JsonConvert.DeserializeObject<Tuple<NetworkLinkDTO, DbGeometry>>(result.Content.ReadAsStringAsync().Result, new DbGeometryConverter());

            return new Tuple<NetworkLinkDTO, SqlGeometry>(nearestSegment.Item1, nearestSegment.Item2?.ToSqlGeometry());
        }

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public async Task<NetworkLinkDTO> GetNetworkLink(Guid networkLinkID)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "networklink/" + networkLinkID);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = string.Concat("Status Code:" + result.StatusCode.GetHashCode(), " Reason:", result.ReasonPhrase);
                throw new ServiceException(responseContent);
            }

            NetworkLinkDTO networkLink = JsonConvert.DeserializeObject<NetworkLinkDTO>(result.Content.ReadAsStringAsync().Result);

            return networkLink;
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> 
        /// <param name="categoryNames">The category names.</param> 
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataNameValuePairs(List<string> categoryNames)
        {
            List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();
            List<NameValuePair> nameValuePairs = new List<NameValuePair>();
            foreach (var category in categoryNames)
            {
                HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "nameValuePairs?appGroupName=" + category);
                if (!result.IsSuccessStatusCode)
                {
                    // LOG ERROR WITH Statuscode
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<string, List<NameValuePair>> apiResult = JsonConvert.DeserializeObject<Tuple<string, List<NameValuePair>>>(result.Content.ReadAsStringAsync().Result);
                nameValuePairs.AddRange(apiResult.Item2);
            }

            listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(nameValuePairs));
            return listReferenceCategories;
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> 
        /// <param name="categoryNames">The category names.</param> 
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "simpleLists", listNames);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = string.Concat("Status Code:" + result.StatusCode.GetHashCode(), " Reason:", result.ReasonPhrase);
                throw new ServiceException(responseContent);
            }

            List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);

            listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));
            return listReferenceCategories;
        }

        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        public async Task<List<NetworkLinkDTO>> GetCrossingNetworkLinks(string boundingBoxCoordinates, DbGeometry accessLinkCoordinates)
        {
            var accessLinkCoordinatesJson = JsonConvert.SerializeObject(accessLinkCoordinates, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(networkManagerDataWebAPIName + "networklink/" + boundingBoxCoordinates, accessLinkCoordinatesJson);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = string.Concat("Status Code:" + result.StatusCode.GetHashCode(), " Reason:", result.ReasonPhrase);
                throw new ServiceException(responseContent);
            }

            List<NetworkLinkDTO> networkLinks = JsonConvert.DeserializeObject<List<NetworkLinkDTO>>(result.Content.ReadAsStringAsync().Result);

            return networkLinks;
        }

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        public async Task<string> GetOSRoadLink(string toid)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "OSRoadLink/" + toid);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = string.Concat("Status Code:" + result.StatusCode.GetHashCode(), " Reason:", result.ReasonPhrase);
                throw new ServiceException(responseContent);
            }

            string roadLink = result.Content.ReadAsStringAsync().Result.Equals("[]") ? string.Empty : result.Content.ReadAsStringAsync().Result;
            return roadLink;
        }

        public async Task<DeliveryPointDTO> GetDeliveryPoint(Guid deliveryPointGuid)
        {
            HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerDataWebAPIName + "deliverypoint/guid:" + deliveryPointGuid);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Status code
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            var deliveryPoint = JsonConvert.DeserializeObject<DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);

            return deliveryPoint;
        }

        public async Task<bool> UpdateDeliveryPointAccessLinkCreationStatus(DeliveryPointDTO deliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateDeliveryPointAccessLinkCreationStatus"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkIntegrationMethodEntryEventId, LoggerTraceConstants.Title);

                var deliveryPointDTOJson = JsonConvert.SerializeObject(deliveryPointDTO, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            HttpResponseMessage result = await httpHandler.PutAsJsonAsync(deliveryPointManagerDataWebAPIName + "deliverypoint/accesslinkstatus", deliveryPointDTOJson);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = string.Concat("Status Code:" + result.StatusCode.GetHashCode(), " Reason:", result.ReasonPhrase);
                throw new ServiceException(responseContent);
            }

            var success = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);

            return success;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.AccessLinkAPIPriority, LoggerTraceConstants.AccessLinkIntegrationMethodExitEventId, LoggerTraceConstants.Title);
            }
        }

        /// <summary> This method is used to get the delivery points crossing an operational object </summary> 
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> 
        /// <param name="accessLink">access link coordinate array</param> 
        /// <returns>List<DeliveryPointDTO></returns>
        public async Task<List<DeliveryPointDTO>> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject)
        {
            var operationalObjectJson = JsonConvert.SerializeObject(operationalObject, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryPointManagerDataWebAPIName + "deliverypoint/crosses/operationalobject/" + boundingBoxCoordinates, operationalObjectJson);

            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            var success = JsonConvert.DeserializeObject<List<DeliveryPointDTO>>(result.Content.ReadAsStringAsync().Result);
            return success;
        } 
        #endregion
    }
}