using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.AccessLink.WebAPI.DTOs;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;

namespace RM.DataManagement.AccessLink.WebAPI.Integration
{
    public class AccessLinkIntegrationService : IAccessLinkIntegrationService
    {
        private const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        private const string NetworkManagerDataWebAPIName = "NetworkManagerDataWebAPIName";
        private const string DeliveryPointManagerDataWebAPIName = "DeliveryPointManagerDataWebAPIName";

        private int priority = LoggerTraceConstants.AccessLinkAPIPriority;
        private int entryEventId = LoggerTraceConstants.AccessLinkIntegrationMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.AccessLinkIntegrationMethodExitEventId;

        #region Member Variables

        private string referenceDataWebAPIName = string.Empty;
        private string networkManagerDataWebAPIName = string.Empty;
        private string deliveryPointManagerDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Member Variables

        #region Constructor

        public AccessLinkIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.httpHandler = httpHandler;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(ReferenceDataWebAPIName).ToString() : string.Empty;
            this.networkManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(NetworkManagerDataWebAPIName).ToString() : string.Empty;
            this.deliveryPointManagerDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointManagerDataWebAPIName).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <param name="streetName">Street name.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public async Task<Tuple<NetworkLinkDTO, SqlGeometry>> GetNearestNamedRoad(DbGeometry operationalObjectPoint, string streetName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetNearestNamedRoad"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(GetNearestNamedRoad);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var operationalObjectPointJson = JsonConvert.SerializeObject(operationalObjectPoint, jsonSerializerSettings);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(networkManagerDataWebAPIName + "/nearestnamedroad/" + streetName, operationalObjectPointJson);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<NetworkLinkDTO, DbGeometry> nearestNamedRoad = JsonConvert.DeserializeObject<Tuple<NetworkLinkDTO, DbGeometry>>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return new Tuple<NetworkLinkDTO, SqlGeometry>(nearestNamedRoad.Item1, nearestNamedRoad.Item2?.ToSqlGeometry());
            }
        }

        /// <summary>
        /// Get the nearest street for operational object.
        /// </summary>
        /// <param name="operationalObjectPoint">Operational object unique identifier.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public async Task<Tuple<NetworkLinkDTO, SqlGeometry>> GetNearestSegment(DbGeometry operationalObjectPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetNearestSegment"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(GetNearestNamedRoad);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var operationalObjectPointJson = JsonConvert.SerializeObject(operationalObjectPoint, jsonSerializerSettings);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(networkManagerDataWebAPIName + "nearestsegment", operationalObjectPointJson);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<NetworkLinkDTO, DbGeometry> nearestSegment = JsonConvert.DeserializeObject<Tuple<NetworkLinkDTO, DbGeometry>>(result.Content.ReadAsStringAsync().Result, new DbGeometryConverter());

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return new Tuple<NetworkLinkDTO, SqlGeometry>(nearestSegment.Item1, nearestSegment.Item2?.ToSqlGeometry());
            }
        }

        /// <summary>
        /// Get the street DTO for operational object.
        /// </summary>
        /// <param name="networkLinkID">networkLink unique identifier Guid.</param>
        /// <returns>Nearest street and the intersection point.</returns>
        public async Task<NetworkLinkDTO> GetNetworkLink(Guid networkLinkID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetNetworkLink"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(GetNearestNamedRoad);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "networklink/" + networkLinkID);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                NetworkLinkDTO networkLink = JsonConvert.DeserializeObject<NetworkLinkDTO>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return networkLink;
            }
        }

        /// <summary> Gets the name of the reference data categories by category. </summary>
        /// <param name="categoryNames">The category names.</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataNameValuePairs(List<string> categoryNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataNameValuePairs"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(GetReferenceDataNameValuePairs);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();
                List<NameValuePair> nameValuePairs = new List<NameValuePair>();
                foreach (var category in categoryNames)
                {
                    HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "nameValuePairs?appGroupName=" + category);
                    if (!result.IsSuccessStatusCode)
                    {
                        var responseContent = result.ReasonPhrase;
                        throw new ServiceException(responseContent);
                    }

                    Tuple<string, List<NameValuePair>> apiResult = JsonConvert.DeserializeObject<Tuple<string, List<NameValuePair>>>(result.Content.ReadAsStringAsync().Result);
                    nameValuePairs.AddRange(apiResult.Item2);
                }

                listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(nameValuePairs));
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return listReferenceCategories;
            }
        }

        /// <summary> Gets the name of the reference data categories by category. </summary>
        /// <param name="categoryNames">The category names.</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetReferenceDataSimpleLists"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(GetReferenceDataSimpleLists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();

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

        /// <summary> This method is used to get the road links crossing the access link </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLinkCoordinates">access link coordinate array</param> <returns>List<NetworkLinkDTO></returns>
        public async Task<List<NetworkLinkDTO>> GetCrossingNetworkLinks(string boundingBoxCoordinates, DbGeometry accessLinkCoordinates)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetCrossingNetworkLinks"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(GetCrossingNetworkLinks);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var accessLinkCoordinatesJson = JsonConvert.SerializeObject(accessLinkCoordinates, jsonSerializerSettings);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(networkManagerDataWebAPIName + "networklink/" + boundingBoxCoordinates, accessLinkCoordinatesJson);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                List<NetworkLinkDTO> networkLinks = JsonConvert.DeserializeObject<List<NetworkLinkDTO>>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return networkLinks;
            }
        }

        /// <summary>
        /// This method is used to fetch data for OSRoadLink.
        /// </summary>
        /// <param name="toid">toid unique identifier for OSRoadLink</param>
        /// <returns>returns Route Hierarchy as string</returns>
        public async Task<string> GetOSRoadLink(string toid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetOSRoadLink"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(GetOSRoadLink);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.GetAsync(networkManagerDataWebAPIName + "OSRoadLink/" + toid);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                string roadLink = result.Content.ReadAsStringAsync().Result.Equals("[]") ? string.Empty : result.Content.ReadAsStringAsync().Result;
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return roadLink;
            }
        }

        public async Task<DeliveryPointDTO> GetDeliveryPoint(Guid deliveryPointGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPoint"))
            {

                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(GetDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                HttpResponseMessage result = await httpHandler.GetAsync(deliveryPointManagerDataWebAPIName + "deliverypoint/guid:" + deliveryPointGuid);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var deliveryPoint = JsonConvert.DeserializeObject<DeliveryPointDTO>(result.Content.ReadAsStringAsync().Result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return deliveryPoint;
            }
        }

        public async Task<bool> UpdateDeliveryPointAccessLinkCreationStatus(DeliveryPointDTO deliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.UpdateDeliveryPointAccessLinkCreationStatus"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(UpdateDeliveryPointAccessLinkCreationStatus);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var deliveryPointDTOJson = JsonConvert.SerializeObject(deliveryPointDTO, jsonSerializerSettings);

                HttpResponseMessage result = await httpHandler.PutAsJsonAsync(deliveryPointManagerDataWebAPIName + "deliverypoint/accesslinkstatus", deliveryPointDTOJson);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var success = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return success;
            }
        }

        /// <summary> This method is used to get the delivery points crossing an operational object </summary>
        /// <param name="boundingBoxCoordinates">bbox coordinates</param>
        /// <param name="accessLink">access link coordinate array</param>
        /// <returns>List<DeliveryPointDTO></returns>
        public async Task<List<DeliveryPointDTO>> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Integration.GetDeliveryPointsCrossingOperationalObject"))
            {
                string methodName = typeof(AccessLinkIntegrationService) + "." + nameof(UpdateDeliveryPointAccessLinkCreationStatus);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var operationalObjectJson = JsonConvert.SerializeObject(operationalObject, jsonSerializerSettings);

                HttpResponseMessage result = await httpHandler.PostAsJsonAsync(deliveryPointManagerDataWebAPIName + "deliverypoint/crosses/operationalobject/" + boundingBoxCoordinates, operationalObjectJson);

                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                var success = JsonConvert.DeserializeObject<List<DeliveryPointDTO>>(result.Content.ReadAsStringAsync().Result);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return success;
            }
        }

        #endregion Methods
    }
}