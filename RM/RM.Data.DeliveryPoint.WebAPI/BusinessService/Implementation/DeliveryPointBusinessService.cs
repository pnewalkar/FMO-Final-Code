namespace RM.DataManagement.DeliveryPoint.WebAPI.BusinessService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using CommonLibrary.ExceptionMiddleware;
    using CommonLibrary.Utilities.HelperMiddleware;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json.Linq;
    using RM.CommonLibrary.ConfigurationMiddleware;
    using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
    using RM.CommonLibrary.EntityFramework.DTO;
    using RM.CommonLibrary.EntityFramework.DTO.Model;
    using RM.CommonLibrary.HelperMiddleware;
    using RM.CommonLibrary.LoggingMiddleware;
    using RM.DataManagement.DeliveryPoint.WebAPI.Integration;
    using Utils;

    public class DeliveryPointBusinessService : IDeliveryPointBusinessService
    {
        #region Member Variables

        private IDeliveryPointsDataService deliveryPointsDataService = default(IDeliveryPointsDataService);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IDeliveryPointIntegrationService deliveryPointIntegrationService = default(IDeliveryPointIntegrationService);

        #endregion Member Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryPointBusinessService"/> class.
        /// </summary>
        /// <param name="deliveryPointsRepository">The delivery points repository.</param>
        /// <param name="postalAddressBusinessService">The postal address business service.</param>
        /// <param name="loggingHelper">The logging helper.</param>
        /// <param name="configurationHelper">The configuration helper.</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        /// <param name="accessLinkBusinessService">The access link business service.</param>
        /// <param name="blockSequenceBusinessService">blockSequenceBusinessService business service</param>
        public DeliveryPointBusinessService(
            IDeliveryPointsDataService deliveryPointsDataService,
            ILoggingHelper loggingHelper,
            IConfigurationHelper configurationHelper,
            IDeliveryPointIntegrationService deliveryPointIntegrationService)
        {
            this.deliveryPointsDataService = deliveryPointsDataService;
            this.loggingHelper = loggingHelper;
            this.configurationHelper = configurationHelper;
            this.deliveryPointIntegrationService = deliveryPointIntegrationService;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// This Method is used to fetch Delivery Points Co-ordinates.
        /// </summary>
        /// <param name="boundaryBox">Boundarybox as string</param>
        /// <param name="unitGuid">Unit unique identifier.</param>
        /// <returns>Object</returns>
        public object GetDeliveryPoints(string boundaryBox, Guid unitGuid)
        {
            object deliveryPoints = null;
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPoints"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var coordinates = GetDeliveryPointsCoordinatesDatabyBoundingBox(boundaryBox.Split(Constants.Comma[0]));
                    deliveryPoints = GetDeliveryPointsJsonData(deliveryPointsDataService.GetDeliveryPoints(coordinates, unitGuid));
                }

                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                return deliveryPoints;
            }
        }

        /// <summary>
        /// Get coordinates of the delivery point by Guid
        /// </summary>
        /// <param name="Guid">The Guid </param>
        /// <returns>The coordinates of the delivery point</returns>
        public object GetDeliveryPointByGuid(Guid id)
        {
            return GetDeliveryPointsJsonData(deliveryPointsDataService.GetDeliveryPointListByGuid(id));
        }

        /// <summary>
        /// This method is used to fetch .......
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        public AddDeliveryPointDTO GetDetailDeliveryPointByUDPRN(int udprn)
        {
            return deliveryPointsDataService.GetDetailDeliveryPointByUDPRN(udprn);
        }

        /// <summary>
        /// Fetch the Delivery point for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>Task</returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid userUnit)
        {
            return await deliveryPointsDataService.FetchDeliveryPointsForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery point</returns>
        public async Task<int> GetDeliveryPointsCount(string searchText, Guid userUnit)
        {
            return await deliveryPointsDataService.GetDeliveryPointsCount(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid)
        {
            return await deliveryPointsDataService.FetchDeliveryPointsForAdvanceSearch(searchText, unitGuid).ConfigureAwait(false);
        }

        /// <summary>
        /// This method is used to insert delivery point.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        public async Task<bool> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.InsertDeliveryPoint"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var isInserted = await deliveryPointsDataService.InsertDeliveryPoint(objDeliveryPoint);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return isInserted;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>string</returns>
        public async Task<CreateDeliveryPointModelDTO> CreateDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            string methodName = MethodHelper.GetActualAsyncMethodName();
            using (loggingHelper.RMTraceManager.StartTrace(methodName))
            {
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string addDeliveryDtoLogDetails = new JavaScriptSerializer().Serialize(addDeliveryPointDTO);
                string message = string.Empty;
                double? returnXCoordinate = 0;
                double? returnYCoordinate = 0;
                Guid returnGuid = new Guid(Constants.DEFAULTGUID);
                byte[] rowVersion = null;

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null &&
                    addDeliveryPointDTO.DeliveryPointDTO != null)
                {
                    // Call Postal Address integration API
                    string postCode = deliveryPointIntegrationService.CheckForDuplicateNybRecords(addDeliveryPointDTO.PostalAddressDTO).Result;

                    // Call Postal Address integration API
                    if (addDeliveryPointDTO.PostalAddressDTO.ID == Guid.Empty && deliveryPointIntegrationService.CheckForDuplicateAddressWithDeliveryPoints(addDeliveryPointDTO.PostalAddressDTO).Result)
                    {
                        message = DeliveryPointConstants.DUPLICATEDELIVERYPOINT;
                        return new CreateDeliveryPointModelDTO { ID = returnGuid, Message = message, RowVersion = rowVersion, XCoordinate = returnXCoordinate, YCoordinate = returnYCoordinate };
                    }
                    else if (addDeliveryPointDTO.PostalAddressDTO.ID == Guid.Empty && !string.IsNullOrEmpty(postCode))
                    {
                        message = DeliveryPointConstants.DUPLICATENYBRECORDS + postCode;
                        return new CreateDeliveryPointModelDTO { ID = returnGuid, Message = message, RowVersion = rowVersion, XCoordinate = returnXCoordinate, YCoordinate = returnYCoordinate };
                    }
                    else
                    {
                        // Call Postal Address integration API
                        CreateDeliveryPointModelDTO createDeliveryPointModelDTO = await deliveryPointIntegrationService.CreateAddressAndDeliveryPoint(addDeliveryPointDTO);

                        if (createDeliveryPointModelDTO == null)
                        {
                            throw new EntityNotFoundException(ErrorConstants.Err_EntityNotFoundException + ": PostalAddressId - " + addDeliveryPointDTO.PostalAddressDTO.ID);
                        }

                        rowVersion = deliveryPointsDataService.GetDeliveryPointRowVersion(createDeliveryPointModelDTO.ID);
                        returnGuid = createDeliveryPointModelDTO.ID;

                        // Call Route log integration API
                        await deliveryPointIntegrationService.CreateBlockSequenceForDeliveryPoint(addDeliveryPointDTO.DeliveryPointDTO.DeliveryRoute_Guid, returnGuid);
                        returnXCoordinate = createDeliveryPointModelDTO.XCoordinate;
                        returnYCoordinate = createDeliveryPointModelDTO.YCoordinate;

                        if (createDeliveryPointModelDTO.IsAddressLocationAvailable)
                        {
                            // Call reference data integration API
                            Guid deliveryOperationObjectTypeId = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.OperationalObjectType, ReferenceDataValues.OperationalObjectTypeDP);
                            var isAccessLinkCreated =
                                deliveryPointIntegrationService.CreateAccessLink(
                                    createDeliveryPointModelDTO.ID,
                                    deliveryOperationObjectTypeId);
                            message = isAccessLinkCreated
                                ? DeliveryPointConstants.DELIVERYPOINTCREATED
                                : DeliveryPointConstants.DELIVERYPOINTCREATEDWITHOUTACCESSLINK;
                        }
                        else
                        {
                            message = DeliveryPointConstants.DELIVERYPOINTCREATEDWITHOUTLOCATION;
                        }
                    }
                }

                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return new CreateDeliveryPointModelDTO { ID = returnGuid, Message = message, RowVersion = rowVersion, XCoordinate = returnXCoordinate, YCoordinate = returnYCoordinate };
            }
        }

        /// <summary>
        /// This Method is used to Update Delivery Points Co-ordinates.
        /// </summary>
        /// <param name="deliveryPointModelDTO">DeliveryPointModelDTO</param>
        /// <returns>message</returns>
        public async Task<UpdateDeliveryPointModelDTO> UpdateDeliveryPointLocation(DeliveryPointModelDTO deliveryPointModelDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.UpdateDeliveryPointLocation"))
            {
                Guid returnGuid = Guid.Empty;
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);
                string sbLocationXY = string.Format(
                                                        DeliveryPointConstants.USRGEOMETRYPOINT,
                                                        Convert.ToString(deliveryPointModelDTO.XCoordinate),
                                                        Convert.ToString(deliveryPointModelDTO.YCoordinate));

                DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), Constants.BNGCOORDINATESYSTEM);
                Guid locationProviderId = deliveryPointIntegrationService.GetReferenceDataGuId(Constants.NETWORKLINKDATAPROVIDER, DeliveryPointConstants.INTERNAL);

                DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO
                {
                    ID = deliveryPointModelDTO.ID,
                    UDPRN = deliveryPointModelDTO.UDPRN,
                    Latitude = deliveryPointModelDTO.Latitude,
                    Longitude = deliveryPointModelDTO.Longitude,
                    LocationXY = spatialLocationXY,
                    LocationProvider_GUID = locationProviderId,
                    RowVersion = deliveryPointModelDTO.RowVersion,
                    Positioned = true
                };

                await deliveryPointsDataService.UpdateDeliveryPointLocationOnID(deliveryPointDTO).ContinueWith(t =>
                {
                    if (t.IsFaulted && t.Exception != null)
                    {
                        throw t.Exception;
                    }

                    if (t.Result != Guid.Empty)
                    {
                        returnGuid = t.Result;

                        // Call reference data integration api
                        Guid deliveryOperationObjectTypeId = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.OperationalObjectType, ReferenceDataValues.OperationalObjectTypeDP);

                        // Call access link integration api
                        deliveryPointIntegrationService.CreateAccessLink(deliveryPointModelDTO.ID, deliveryOperationObjectTypeId);
                    }
                });
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return new UpdateDeliveryPointModelDTO { XCoordinate = deliveryPointModelDTO.XCoordinate, YCoordinate = deliveryPointModelDTO.YCoordinate, ID = returnGuid };
            }
        }

        /// <summary>
        /// This Method fetches Route and DPUse for a single DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>KeyValuePair for Route and DPUse</returns>
        public List<KeyValuePair<string, string>> GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            // using (loggingHelper.RMTraceManager.StartTrace(LoggerTraceConstants.BusinessLayer + methodName))
            // {
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetRouteForDeliveryPointPriority, LoggerTraceConstants.GetRouteForDeliveryPointBusinessMethodEntryEventId, LoggerTraceConstants.Title);

            List<KeyValuePair<string, string>> dpDetails = new List<KeyValuePair<string, string>>();
            string routeName = deliveryPointsDataService.GetRouteForDeliveryPoint(deliveryPointId);
            string dpUse = GetDPUse(deliveryPointId);
            if (routeName != null)
            {
                dpDetails.Add(new KeyValuePair<string, string>(Constants.RouteName, routeName));
            }

            dpDetails.Add(new KeyValuePair<string, string>(Constants.DpUse, dpUse));
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetRouteForDeliveryPointPriority, LoggerTraceConstants.GetRouteForDeliveryPointBusinessMethodExitEventId, LoggerTraceConstants.Title);
            return dpDetails;

            // }
        }

        /// <summary>
        /// Check if the delivery point exists for a given UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        public async Task<bool> DeliveryPointExists(int udprn)
        {
            return await deliveryPointsDataService.DeliveryPointExists(udprn);
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        public async Task<DeliveryPointDTO> GetDeliveryPointByUDPRNforBatch(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPointByUDPRNforBatch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);
                var getDeliveryPointByUDPRN = await deliveryPointsDataService.GetDeliveryPointByUDPRN(udprn);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return getDeliveryPointByUDPRN;
            }
        }

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        public async Task<int> UpdateDeliveryPointLocationOnUDPRN(DeliveryPointDTO deliveryPointDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.UpdateDeliveryPointLocationOnUDPRN"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var status = await deliveryPointsDataService.UpdateDeliveryPointLocationOnUDPRN(deliveryPointDto);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return status;
            }
        }

        /// <summary>
        /// This method updates delivery point location using ID
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        public async Task<Guid> UpdateDeliveryPointLocationOnID(DeliveryPointDTO deliveryPointDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.UpdateDeliveryPointLocationOnID"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var deliveryPointId = await deliveryPointsDataService.UpdateDeliveryPointLocationOnID(deliveryPointDto);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return deliveryPointId;
            }
        }

        /// <summary>
        /// Get the delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        public DeliveryPointDTO GetDeliveryPointByPostalAddress(Guid addressId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPointByPostalAddress"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var getDeliveryPointByPostalAddress = deliveryPointsDataService.GetDeliveryPointByPostalAddress(addressId);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return getDeliveryPointByPostalAddress;
            }
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>DeliveryPointDTO</returns>
        public DeliveryPointDTO GetDeliveryPoint(Guid deliveryPointGuid)
        {
            return deliveryPointsDataService.GetDeliveryPoint(deliveryPointGuid);
        }

        /// <summary>
        /// This method updates delivery point access link status
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        public bool UpdateDeliveryPointAccessLinkCreationStatus(DeliveryPointDTO deliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.UpdateDeliveryPointLocationOnID"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var isDeliveryPointUpdated = deliveryPointsDataService.UpdateDeliveryPointAccessLinkCreationStatus(deliveryPointDTO);
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return isDeliveryPointUpdated;
            }
        }

        /// <summary> This method is used to get the delivery points crossing the operational object.
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="operationalObject">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        public List<DeliveryPointDTO> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject)
        {
            return deliveryPointsDataService.GetDeliveryPointsCrossingOperationalObject(boundingBoxCoordinates, operationalObject);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// This method is used to fetch GeoJson data for Delivery Point.
        /// </summary>
        /// <param name="lstDeliveryPointDTO">List of Delivery Point Dto</param>
        /// <returns>lstDeliveryPointDTO</returns>
        private static object GetDeliveryPointsJsonData(List<DeliveryPointDTO> lstDeliveryPointDTO)
        {
            var deliveryPointGeoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (lstDeliveryPointDTO != null && lstDeliveryPointDTO.Count > 0)
            {
                foreach (var point in lstDeliveryPointDTO)
                {
                    SqlGeometry deliveryPointSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(point.LocationXY.AsBinary()), 0);

                    var feature = new Feature
                    {
                        id = point.ID.ToString(),
                        properties = new Dictionary<string, JToken>
                    {
                        { Constants.BuildingName, point.PostalAddress.BuildingName },
                        { Constants.BuildingNumber, point.PostalAddress.BuildingNumber },
                        { Constants.Postcode, point.PostalAddress.Postcode },
                        { Constants.StreetName, point.PostalAddress.BuildingName },
                        { Constants.LayerType, Convert.ToString(OtherLayersType.DeliveryPoint.GetDescription()) },
                        { Constants.OrganisationName, point.PostalAddress.OrganisationName },
                        { Constants.DepartmentName, point.PostalAddress.DepartmentName },
                        { Constants.MailVolume, point.MailVolume },
                        { Constants.MultipleOccupancyCount, point.MultipleOccupancyCount },
                        { Constants.Locality, (point.PostalAddress.DependentLocality + Constants.Space + point.PostalAddress.DoubleDependentLocality).Trim() },
                        { Constants.DeliveryPointId, point.ID },
                        { Constants.Street, (point.PostalAddress.Thoroughfare + Constants.Space + point.PostalAddress.DependentThoroughfare).Trim() },
                        { Constants.SubBuildingName, point.PostalAddress.SubBuildingName }
                    },
                        geometry = new Geometry
                        {
                            coordinates = new double[] { deliveryPointSqlGeometry.STX.Value, deliveryPointSqlGeometry.STY.Value }
                        }
                    };
                    deliveryPointGeoJson.features.Add(feature);
                }
            }

            return deliveryPointGeoJson;
        }

        /// <summary>
        /// This Method is used to fetch Delivery Points Co-ordinates.
        /// </summary>
        /// <param name="parameters">parameters as object</param>
        /// <returns>coordinates</returns>
        private static string GetDeliveryPointsCoordinatesDatabyBoundingBox(params object[] parameters)
        {
            string coordinates = string.Empty;
            if (parameters != null && parameters.Length == 4)
            {
                coordinates = string.Format(
                                     Constants.Polygon,
                                     Convert.ToString(parameters[0]),
                                     Convert.ToString(parameters[1]),
                                     Convert.ToString(parameters[0]),
                                     Convert.ToString(parameters[3]),
                                     Convert.ToString(parameters[2]),
                                     Convert.ToString(parameters[3]),
                                     Convert.ToString(parameters[2]),
                                     Convert.ToString(parameters[1]),
                                     Convert.ToString(parameters[0]),
                                     Convert.ToString(parameters[1]));
            }

            return coordinates;
        }

        /// <summary>
        /// This Method fetches DPUse value for the DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>DPUse value as string</returns>
        private string GetDPUse(Guid deliveryPointId)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            // using (loggingHelper.RMTraceManager.StartTrace(LoggerTraceConstants.BusinessLayer + methodName))
            // {
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetDPUsePriority, LoggerTraceConstants.GetDPUseBusinessMethodEntryEventId, LoggerTraceConstants.Title);
            Guid deliveryOperationObjectTypeId = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.OperationalObjectType, ReferenceDataValues.OperationalObjectTypeDP);
            Guid operationalObjectTypeForDpOrganisation = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.DeliveryPointUseIndicator, ReferenceDataValues.Organisation);
            Guid operationalObjectTypeForDpResidential = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.DeliveryPointUseIndicator, ReferenceDataValues.Residential);
            string dpUsetype = deliveryPointsDataService.GetDPUse(deliveryPointId, operationalObjectTypeForDpOrganisation, operationalObjectTypeForDpResidential);
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetDPUsePriority, LoggerTraceConstants.GetDPUseBusinessMethodExitEventId, LoggerTraceConstants.Title);
            return dpUsetype;

            // }
        }

        #endregion Private Methods
    }
}