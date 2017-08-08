namespace RM.DataManagement.DeliveryPoint.WebAPI.BusinessService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json.Linq;
    using RM.CommonLibrary.ConfigurationMiddleware;
    using RM.CommonLibrary.HelperMiddleware;
    using RM.CommonLibrary.LoggingMiddleware;
    using RM.Data.DeliveryPoint.WebAPI.DataDTO;
    using RM.Data.DeliveryPoint.WebAPI.DTO;
    using RM.Data.DeliveryPoint.WebAPI.DTO.Model;
    using RM.DataManagement.DeliveryPoint.WebAPI.DataService;
    using RM.DataManagement.DeliveryPoint.WebAPI.Integration;
    using Utils;
    using AutoMapper;

    public class DeliveryPointBusinessService : IDeliveryPointBusinessService
    {
        #region Member Variables

        private IDeliveryPointsDataService deliveryPointsDataService = default(IDeliveryPointsDataService);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IDeliveryPointIntegrationService deliveryPointIntegrationService = default(IDeliveryPointIntegrationService);

        private int priority = LoggerTraceConstants.DeliveryPointAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId;

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
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPoints"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPoints);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var coordinates = GetDeliveryPointsCoordinatesDatabyBoundingBox(boundaryBox.Split(DeliveryPointConstants.Comma[0]));
                    List<DeliveryPointDTO> deliveryPointDtos = ConvertToDTO(deliveryPointsDataService.GetDeliveryPoints(coordinates, unitGuid));

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return GetDeliveryPointsJsonData(deliveryPointDtos);
                }
                else
                {
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return null;
                }
            }
        }

        /// <summary>
        /// Get coordinates of the delivery point by Guid
        /// </summary>
        /// <param name="Guid">The Guid </param>
        /// <returns>The coordinates of the delivery point</returns>
        public object GetDeliveryPointByGuid(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPointByGuid"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointByGuid);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDeliveryPointsJsonData = GetDeliveryPointsJsonData(new List<DeliveryPointDTO> { ConvertToDTO(deliveryPointsDataService.GetDeliveryPoint(id)) });
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getDeliveryPointsJsonData;
            }
        }

        /// <summary>
        /// This method is used to fetch delivery point details.
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        public RM.Data.DeliveryPoint.WebAPI.DTO.AddDeliveryPointDTO GetDetailDeliveryPointByUDPRN(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDetailDeliveryPointByUDPRN"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDetailDeliveryPointByUDPRN);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDetailDeliveryPointByUDPRN = deliveryPointsDataService.GetDetailDeliveryPointByUDPRN(udprn);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getDetailDeliveryPointByUDPRN;
            }
        }

        /// <summary>
        /// Fetch the Delivery point for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>Task</returns>
        public async Task<List<DeliveryPointDTO>> GetDeliveryPointsForBasicSearch(string searchText, Guid userUnit, string currentUserUnitType)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryPointsForBasicSearch"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointsForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                int recordTakeCount = Convert.ToInt32(configurationHelper.ReadAppSettingsConfigurationValues(DeliveryPointConstants.SearchResultCount));
                var fetchDeliveryPointsForBasicSearch = await deliveryPointsDataService.GetDeliveryPointsForBasicSearch(searchText, recordTakeCount, userUnit, currentUserUnitType).ConfigureAwait(false);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return ConvertToDTO(fetchDeliveryPointsForBasicSearch);
            }
        }

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>The total count of delivery point</returns>
        public async Task<int> GetDeliveryPointsCount(string searchText, Guid userUnit, string currentUserUnitType)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPointsCount"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointsCount);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDeliveryPointsCount = await deliveryPointsDataService.GetDeliveryPointsCount(searchText, userUnit, currentUserUnitType).ConfigureAwait(false);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getDeliveryPointsCount;
            }
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <param name="currentUserUnitType">The current user unit type.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        public async Task<List<DeliveryPointDTO>> GetDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid, string currentUserUnitType)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryPointsForAdvanceSearch"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointsForAdvanceSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var fetchDeliveryPointsForAdvanceSearch = await deliveryPointsDataService.GetDeliveryPointsForAdvanceSearch(searchText, unitGuid, currentUserUnitType).ConfigureAwait(false);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return ConvertToDTO(fetchDeliveryPointsForAdvanceSearch);
            }
        }

        /// <summary>
        /// This method is used to insert delivery point.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>Unique identifier of delivery point.</returns>
        public async Task<Guid> InsertDeliveryPoint(DeliveryPointDTO objDeliveryPoint)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.InsertDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(InsertDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (objDeliveryPoint == null)
                {
                    throw new ArgumentNullException(nameof(objDeliveryPoint), string.Format(ErrorConstants.Err_ArgumentmentNullException, objDeliveryPoint));
                }

                var deliveryPointId = await deliveryPointsDataService.InsertDeliveryPoint(ConvertToDataDTO(objDeliveryPoint));
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return deliveryPointId;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>string</returns>
        public async Task<CreateDeliveryPointModelDTO> CreateDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(CreateDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (addDeliveryPointDTO == null)
                {
                    throw new ArgumentNullException(nameof(addDeliveryPointDTO), string.Format(ErrorConstants.Err_ArgumentmentNullException, addDeliveryPointDTO));
                }

                string addDeliveryDtoLogDetails = new JavaScriptSerializer().Serialize(addDeliveryPointDTO);
                string message = string.Empty;
                double? returnXCoordinate = 0;
                double? returnYCoordinate = 0;
                Guid returnGuid = new Guid(DeliveryPointConstants.DEFAULTGUID);
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
                    }
                    else if (addDeliveryPointDTO.PostalAddressDTO.ID == Guid.Empty && !string.IsNullOrEmpty(postCode))
                    {
                        message = DeliveryPointConstants.DUPLICATENYBRECORDS + postCode;
                    }
                    else
                    {
                        // Call Postal Address integration API
                        CreateDeliveryPointModelDTO createDeliveryPointModelDTO = await deliveryPointIntegrationService.CreateAddressForDeliveryPoint(addDeliveryPointDTO);

                        List<string> listNames = new List<string> { ReferenceDataCategoryNames.DeliveryPointOperationalStatus, ReferenceDataCategoryNames.DataProvider, ReferenceDataCategoryNames.NetworkNodeType };

                        var referenceDataCategoryList = deliveryPointIntegrationService.GetReferenceDataSimpleLists(listNames).Result;

                        // create deliverypoint
                        DeliveryPointDataDTO deliveryPointdataDTO = new DeliveryPointDataDTO();
                        deliveryPointdataDTO.PostalAddressID = createDeliveryPointModelDTO.ID;

                        deliveryPointdataDTO.NetworkNode.DataProviderGUID = referenceDataCategoryList
                                                 .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DataProvider)
                                                 .SelectMany(x => x.ReferenceDatas)
                                                 .Where(x => x.ReferenceDataValue == DeliveryPointConstants.EXTERNAL).Select(x => x.ID)
                                                 .SingleOrDefault();

                        deliveryPointdataDTO.NetworkNode.NetworkNodeType_GUID = referenceDataCategoryList
                                                 .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkNodeType)
                                                 .SelectMany(x => x.ReferenceDatas)
                                                 .Where(x => x.ReferenceDataValue == DeliveryPointConstants.NetworkNodeTypeRMGServiceNode).Select(x => x.ID)
                                                 .SingleOrDefault();

                        // check for exact and approx location
                        if (createDeliveryPointModelDTO.IsAddressLocationAvailable)
                        {
                            string sbLocationXY = string.Format(
                                                   DeliveryPointConstants.USRGEOMETRYPOINT,
                                                   Convert.ToString(createDeliveryPointModelDTO.XCoordinate),
                                                   Convert.ToString(createDeliveryPointModelDTO.YCoordinate));

                            // if the exact location is present
                            deliveryPointdataDTO.NetworkNode.Location.Shape =
                                DbGeometry.PointFromText(sbLocationXY.ToString(), DeliveryPointConstants.BNGCOORDINATESYSTEM);

                            DeliveryPointStatusDataDTO deliveryPointStatusDataDTO = new DeliveryPointStatusDataDTO();

                            Guid liveWithLocationStatusId = referenceDataCategoryList
                                              .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointOperationalStatus)
                                              .SelectMany(x => x.ReferenceDatas)
                                              .Where(x => x.ReferenceDataValue == DeliveryPointConstants.OperationalStatusGUIDLive).Select(x => x.ID)
                                              .SingleOrDefault();

                            deliveryPointStatusDataDTO.DeliveryPointStatusGUID = liveWithLocationStatusId;
                        }
                        else
                        {
                            // if the exact location is not present
                            deliveryPointdataDTO.NetworkNode.Location.Shape = deliveryPointIntegrationService.GetApproxLocation(addDeliveryPointDTO.PostalAddressDTO.Postcode).Result;

                            DeliveryPointStatusDataDTO deliveryPointStatusDataDTO = new DeliveryPointStatusDataDTO();

                            Guid liveWithPendingLocationStatusId = referenceDataCategoryList
                                          .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointOperationalStatus)
                                          .SelectMany(x => x.ReferenceDatas)
                                          .Where(x => x.ReferenceDataValue == DeliveryPointConstants.OperationalStatusGUIDLivePendingLocation).Select(x => x.ID)
                                          .SingleOrDefault();

                            deliveryPointStatusDataDTO.DeliveryPointStatusGUID = liveWithPendingLocationStatusId;

                            deliveryPointdataDTO.DeliveryPointStatus.Add(deliveryPointStatusDataDTO);
                        }

                        deliveryPointdataDTO.DeliveryPointUseIndicatorGUID = addDeliveryPointDTO.DeliveryPointDTO.DeliveryPointUseIndicator_GUID;
                        deliveryPointdataDTO.MultipleOccupancyCount = addDeliveryPointDTO.DeliveryPointDTO.MultipleOccupancyCount;
                        deliveryPointdataDTO.MailVolume = addDeliveryPointDTO.DeliveryPointDTO.MailVolume;

                        // create delivery point with real/approx location
                        var newDeliveryPointId = await deliveryPointsDataService.InsertDeliveryPoint(deliveryPointdataDTO);

                        rowVersion = deliveryPointsDataService.GetDeliveryPointRowVersion(newDeliveryPointId);
                        returnGuid = newDeliveryPointId;

                        // Call Route log integration API
                        await deliveryPointIntegrationService.MapRouteForDeliveryPoint(addDeliveryPointDTO.DeliveryPointDTO.DeliveryRoute_Guid, returnGuid);
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

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(UpdateDeliveryPointLocation);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointModelDTO == null)
                {
                    throw new ArgumentNullException(nameof(deliveryPointModelDTO), string.Format(ErrorConstants.Err_ArgumentmentNullException, deliveryPointModelDTO));
                }

                List<string> categoryNamesSimpleLists = new List<string>
                    {
                        DeliveryPointConstants.TASKNOTIFICATION,
                        DeliveryPointConstants.NETWORKLINKDATAPROVIDER,
                        DeliveryPointConstants.DeliveryPointUseIndicator,
                        ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                        ReferenceDataCategoryNames.NetworkNodeType
                    };
                var referenceDataCategoryList = deliveryPointIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;

                Guid operationalStatusGUIDLive = referenceDataCategoryList
                                .Where(list => list.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointOperationalStatus)
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(DeliveryPointConstants.OperationalStatusGUIDLive, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();

                Guid networkNodeTypeRMGServiceNode = referenceDataCategoryList
                                .Where(list => list.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkNodeType)
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(DeliveryPointConstants.NetworkNodeTypeRMGServiceNode, StringComparison.OrdinalIgnoreCase))
                                .Select(s => s.ID).SingleOrDefault();

                Guid locationProviderId = referenceDataCategoryList
                                        .Where(list => list.CategoryName.Equals(DeliveryPointConstants.NETWORKLINKDATAPROVIDER, StringComparison.OrdinalIgnoreCase))
                                        .SelectMany(list => list.ReferenceDatas)
                                        .Where(item => item.ReferenceDataValue.Equals(DeliveryPointConstants.EXTERNAL, StringComparison.OrdinalIgnoreCase))
                                        .Select(s => s.ID).SingleOrDefault();

                string sbLocationXY = string.Format(
                                                        DeliveryPointConstants.USRGEOMETRYPOINT,
                                                        Convert.ToString(deliveryPointModelDTO.XCoordinate),
                                                        Convert.ToString(deliveryPointModelDTO.YCoordinate));

                DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), DeliveryPointConstants.BNGCOORDINATESYSTEM);

                DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO
                {
                    ID = deliveryPointModelDTO.ID,
                    Address_GUID = deliveryPointModelDTO.ID,
                    OperationalStatus_GUID = operationalStatusGUIDLive,
                    LocationProvider_GUID = locationProviderId,
                    NetworkNodeType_GUID = networkNodeTypeRMGServiceNode,
                    RowVersion = deliveryPointModelDTO.RowVersion,
                    LocationXY = spatialLocationXY
                };

                Guid returnGuid = Guid.Empty;

                await deliveryPointsDataService.UpdateDeliveryPointLocationOnID(ConvertToDataDTO(deliveryPointDTO)).ContinueWith(t =>
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
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return new UpdateDeliveryPointModelDTO { XCoordinate = deliveryPointModelDTO.XCoordinate, YCoordinate = deliveryPointModelDTO.YCoordinate, ID = deliveryPointDTO.ID };
            }
        }

        /// <summary>
        /// This Method fetches Route and DPUse for a single DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>KeyValuePair for Route and DPUse</returns>
        public List<KeyValuePair<string, string>> GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteForDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetRouteForDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<KeyValuePair<string, string>> dpDetails = new List<KeyValuePair<string, string>>();

                string routeName = deliveryPointIntegrationService.GetRouteForDeliveryPoint(deliveryPointId).Result.RouteName;
                string dpUse = GetDPUse(deliveryPointId);
                if (routeName != null)
                {
                    dpDetails.Add(new KeyValuePair<string, string>(DeliveryPointConstants.RouteName, routeName));
                }

                dpDetails.Add(new KeyValuePair<string, string>(DeliveryPointConstants.DpUse, dpUse));

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return dpDetails;
            }
        }

        /// <summary>
        /// Check if the delivery point exists for a given UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        public async Task<bool> DeliveryPointExists(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.DeliveryPointExists"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(DeliveryPointExists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var deliveryPointExists = await deliveryPointsDataService.DeliveryPointExists(udprn);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return deliveryPointExists;
            }
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
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointByUDPRNforBatch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDeliveryPointByUDPRN = await deliveryPointsDataService.GetDeliveryPointByUDPRN(udprn);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return ConvertToDTO(getDeliveryPointByUDPRN);
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
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(UpdateDeliveryPointLocationOnUDPRN);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointDto == null)
                {
                    throw new ArgumentNullException(nameof(deliveryPointDto), string.Format(ErrorConstants.Err_ArgumentmentNullException, deliveryPointDto));
                }

                var status = await deliveryPointsDataService.UpdateDeliveryPointLocationOnUDPRN(ConvertToDataDTO(deliveryPointDto));
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(UpdateDeliveryPointLocationOnID);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointDto == null)
                {
                    throw new ArgumentNullException(nameof(deliveryPointDto), string.Format(ErrorConstants.Err_ArgumentmentNullException, deliveryPointDto));
                }

                var deliveryPointId = await deliveryPointsDataService.UpdateDeliveryPointLocationOnID(ConvertToDataDTO(deliveryPointDto));
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

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
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointByPostalAddress);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDeliveryPointByPostalAddress = deliveryPointsDataService.GetDeliveryPointByPostalAddress(addressId);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return ConvertToDTO(getDeliveryPointByPostalAddress);
            }
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>DeliveryPointDTO</returns>
        public DeliveryPointDTO GetDeliveryPoint(Guid deliveryPointGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDeliveryPoint = deliveryPointsDataService.GetDeliveryPoint(deliveryPointGuid);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return ConvertToDTO(getDeliveryPoint);
            }
        }

        /// <summary> This method is used to get the delivery points crossing the operational object.
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="operationalObject">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        public List<DeliveryPointDTO> GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, DbGeometry operationalObject)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPointsCrossingOperationalObject"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointsCrossingOperationalObject);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDeliveryPointsCrossingOperationalObject = deliveryPointsDataService.GetDeliveryPointsCrossingOperationalObject(boundingBoxCoordinates, operationalObject);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return ConvertToDTO(getDeliveryPointsCrossingOperationalObject);
            }
        }

        /// <summary>
        ///  Updates Paf indicator for an delivery point of an address.
        /// </summary>
        /// <param name="addressGuid">Address unique identifier.</param>
        /// <param name="pafIndicator">Paf indicator.</param>
        /// <returns>Boolean flag indicating success of operation.</returns>
        public Task<bool> UpdatePAFIndicator(Guid addressGuid, Guid pafIndicator)
        {
            return deliveryPointsDataService.UpdatePAFIndicator(addressGuid, pafIndicator);
        }

        /// <summary>
        /// Delete delivery point.
        /// </summary>
        /// <param name="deliveryPointid">Delivery point unique identifier.</param>
        /// <returns>Boolean flag indicating success of operation.</returns>
        public async Task<bool> DeleteDeliveryPoint(Guid deliveryPointid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.DeleteDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(DeleteDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                await deliveryPointIntegrationService.DeleteDeliveryPointRouteMapping(deliveryPointid);

                await deliveryPointIntegrationService.DeleteAccesslink(deliveryPointid);

                bool deliveryPointDeleted = await deliveryPointsDataService.DeleteDeliveryPoint(deliveryPointid);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return deliveryPointDeleted;
            }
        }

        public async Task<DeliveryPointDTO> GetDeliveryPointByPostalAddressWithLocation(Guid addressId)
        {
            return ConvertToDTO(await deliveryPointsDataService.GetDeliveryPointByPostalAddressWithLocation(addressId));
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>string</returns>
        public async Task<CreateDeliveryPointForRangeModelDTO> CheckDeliveryPointForRange(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CheckDeliveryPointForRange"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(CheckDeliveryPointForRange);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                CreateDeliveryPointForRangeModelDTO createDeliveryPointForRangeModelDTO = null;

                if (addDeliveryPointDTO == null)
                {
                    throw new ArgumentNullException(nameof(addDeliveryPointDTO), string.Format(ErrorConstants.Err_ArgumentmentNullException, addDeliveryPointDTO));
                }

                string addDeliveryDtoLogDetails = new JavaScriptSerializer().Serialize(addDeliveryPointDTO);
                string message = string.Empty;
                Guid returnGuid = new Guid(DeliveryPointConstants.DEFAULTGUID);
                createDeliveryPointForRangeModelDTO = new CreateDeliveryPointForRangeModelDTO();

                List<PostalAddressDTO> postalAddressDTOs = GetMultipleAddressesForDeliveryPoint(addDeliveryPointDTO);

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null &&
                    addDeliveryPointDTO.DeliveryPointDTO != null)
                {
                    // Call Postal Address integration API
                    DuplicateDeliveryPointDTO duplicateNybRecords = deliveryPointIntegrationService.CheckForDuplicateNybRecordsForRange(postalAddressDTOs).Result;
                    DuplicateDeliveryPointDTO duplicatePostalAddressRecords = deliveryPointIntegrationService.CheckForDuplicateAddressWithDeliveryPointsForRange(postalAddressDTOs).Result;

                    // Call Postal Address integration API
                    if (addDeliveryPointDTO.PostalAddressDTO.ID == Guid.Empty && duplicatePostalAddressRecords != null && duplicatePostalAddressRecords.PostalAddressDTO != null && duplicatePostalAddressRecords.PostalAddressDTO.Count > 0)
                    {
                        RemoveDuplicateAddresses(postalAddressDTOs, duplicatePostalAddressRecords.PostalAddressDTO);
                        if (postalAddressDTOs.Count == 0)
                        {
                            createDeliveryPointForRangeModelDTO.HasAllDuplicates = true;
                            createDeliveryPointForRangeModelDTO.Message = DeliveryPointConstants.DUPLICATEDELIVERYPOINTRANGEALLDUPLICATES;
                        }
                        else
                        {
                            createDeliveryPointForRangeModelDTO.HasDuplicates = true;
                            createDeliveryPointForRangeModelDTO.PostalAddressDTOs = postalAddressDTOs;
                            createDeliveryPointForRangeModelDTO.Message = DeliveryPointConstants.DUPLICATEDELIVERYPOINTRANGE;
                        }
                    }
                    else if (addDeliveryPointDTO.PostalAddressDTO.ID == Guid.Empty && duplicateNybRecords != null && duplicateNybRecords.PostalAddressDTO != null && duplicateNybRecords.PostalAddressDTO.Count > 0)
                    {
                        RemoveDuplicateAddresses(postalAddressDTOs, duplicateNybRecords.PostalAddressDTO);
                        if (postalAddressDTOs.Count == 0)
                        {
                            createDeliveryPointForRangeModelDTO.HasAllDuplicates = true;
                            createDeliveryPointForRangeModelDTO.Message = DeliveryPointConstants.DUPLICATEDELIVERYPOINTRANGEALLDUPLICATES;
                        }
                        else
                        {
                            createDeliveryPointForRangeModelDTO.HasDuplicates = true;
                            createDeliveryPointForRangeModelDTO.PostalAddressDTOs = postalAddressDTOs;
                            createDeliveryPointForRangeModelDTO.Message = DeliveryPointConstants.DUPLICATEDELIVERYPOINTRANGE;
                        }
                    }
                    else
                    {
                        createDeliveryPointForRangeModelDTO = await CreateDeliveryPointForRange(postalAddressDTOs);
                    }
                }
                return createDeliveryPointForRangeModelDTO;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>string</returns>
        public async Task<CreateDeliveryPointForRangeModelDTO> CreateDeliveryPointForRange(List<PostalAddressDTO> postalAddressDTOs)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateDeliveryPointForRange"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(CreateDeliveryPointForRange);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                List<CreateDeliveryPointModelDTO> returnCreateDeliveryPointModelDTOs = null;
                List<Guid> deliveryPointIds = null;
                Guid returnGuid = Guid.Empty;
                Guid deliveryRouteId = Guid.Empty;
                byte[] rowVersion = null;
                double? returnXCoordinate = 0;
                double? returnYCoordinate = 0;
                string message = string.Empty;

                returnCreateDeliveryPointModelDTOs = new List<CreateDeliveryPointModelDTO>();
                deliveryPointIds = new List<Guid>();
                // Call Postal Address integration API
                List<CreateDeliveryPointModelDTO> createDeliveryPointModelDTOs = await deliveryPointIntegrationService.CreateAddressForDeliveryPointForRange(postalAddressDTOs);

                List<string> listNames = new List<string> { ReferenceDataCategoryNames.DeliveryPointOperationalStatus, ReferenceDataCategoryNames.DataProvider, ReferenceDataCategoryNames.NetworkNodeType };

                var referenceDataCategoryList = deliveryPointIntegrationService.GetReferenceDataSimpleLists(listNames).Result;

                foreach (CreateDeliveryPointModelDTO createDeliveryPointModelDTO in createDeliveryPointModelDTOs)
                {
                    // create deliverypoint
                    DeliveryPointDataDTO deliveryPointdataDTO = new DeliveryPointDataDTO();
                    deliveryPointdataDTO.PostalAddressID = createDeliveryPointModelDTO.ID;

                    deliveryPointdataDTO.NetworkNode.DataProviderGUID = referenceDataCategoryList
                                             .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DataProvider)
                                             .SelectMany(x => x.ReferenceDatas)
                                             .Where(x => x.ReferenceDataValue == DeliveryPointConstants.EXTERNAL).Select(x => x.ID)
                                             .SingleOrDefault();

                    deliveryPointdataDTO.NetworkNode.NetworkNodeType_GUID = referenceDataCategoryList
                                             .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkNodeType)
                                             .SelectMany(x => x.ReferenceDatas)
                                             .Where(x => x.ReferenceDataValue == DeliveryPointConstants.NetworkNodeTypeRMGServiceNode).Select(x => x.ID)
                                             .SingleOrDefault();

                    PostalAddressDTO postalAddressDTO = postalAddressDTOs.Where(pa => pa.ID == createDeliveryPointModelDTO.ID).FirstOrDefault();

                    // check for exact and approx location
                    if (postalAddressDTO != null)
                    {
                        if (createDeliveryPointModelDTO.IsAddressLocationAvailable)
                        {
                            string sbLocationXY = string.Format(
                                                   DeliveryPointConstants.USRGEOMETRYPOINT,
                                                   Convert.ToString(createDeliveryPointModelDTO.XCoordinate),
                                                   Convert.ToString(createDeliveryPointModelDTO.YCoordinate));

                            // if the exact location is present
                            deliveryPointdataDTO.NetworkNode.Location.Shape =
                                DbGeometry.PointFromText(sbLocationXY.ToString(), DeliveryPointConstants.BNGCOORDINATESYSTEM);

                            DeliveryPointStatusDataDTO deliveryPointStatusDataDTO = new DeliveryPointStatusDataDTO();

                            Guid liveWithLocationStatusId = referenceDataCategoryList
                                              .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointOperationalStatus)
                                              .SelectMany(x => x.ReferenceDatas)
                                              .Where(x => x.ReferenceDataValue == DeliveryPointConstants.OperationalStatusGUIDLive).Select(x => x.ID)
                                              .SingleOrDefault();

                            deliveryPointStatusDataDTO.DeliveryPointStatusGUID = liveWithLocationStatusId;
                        }
                        else
                        {
                            // if the exact location is not present
                            deliveryPointdataDTO.NetworkNode.Location.Shape = deliveryPointIntegrationService.GetApproxLocation(postalAddressDTO.Postcode).Result;
                            SqlGeometry approxLocation = SqlGeometry.STGeomFromWKB(new SqlBytes(deliveryPointdataDTO.NetworkNode.Location.Shape.AsBinary()), DeliveryPointConstants.BNGCOORDINATESYSTEM);

                            createDeliveryPointModelDTO.XCoordinate = approxLocation.STX.Value;
                            createDeliveryPointModelDTO.YCoordinate = approxLocation.STY.Value;

                            DeliveryPointStatusDataDTO deliveryPointStatusDataDTO = new DeliveryPointStatusDataDTO();

                            Guid liveWithPendingLocationStatusId = referenceDataCategoryList
                                          .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointOperationalStatus)
                                          .SelectMany(x => x.ReferenceDatas)
                                          .Where(x => x.ReferenceDataValue == DeliveryPointConstants.OperationalStatusGUIDLivePendingLocation).Select(x => x.ID)
                                          .SingleOrDefault();

                            deliveryPointStatusDataDTO.DeliveryPointStatusGUID = liveWithPendingLocationStatusId;

                            deliveryPointdataDTO.DeliveryPointStatus.Add(deliveryPointStatusDataDTO);
                        }

                        deliveryPointdataDTO.DeliveryPointUseIndicatorGUID = postalAddressDTO.DeliveryPointUseIndicator_GUID;

                        // create delivery point with real/approx location
                        var newDeliveryPointId = await deliveryPointsDataService.InsertDeliveryPoint(deliveryPointdataDTO);

                        deliveryPointIds.Add(newDeliveryPointId);

                        rowVersion = deliveryPointsDataService.GetDeliveryPointRowVersion(newDeliveryPointId);
                        returnGuid = newDeliveryPointId;

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

                        createDeliveryPointModelDTO.ID = returnGuid;
                        createDeliveryPointModelDTO.Message = message;
                        createDeliveryPointModelDTO.RowVersion = rowVersion;
                        createDeliveryPointModelDTO.XCoordinate = returnXCoordinate;
                        createDeliveryPointModelDTO.YCoordinate = returnYCoordinate;

                        returnCreateDeliveryPointModelDTOs.Add(createDeliveryPointModelDTO);

                        if (deliveryRouteId == Guid.Empty)
                        {
                            deliveryRouteId = postalAddressDTO.DeliveryRoute_Guid;
                        }
                    }

                    // Call Route log integration API
                    await deliveryPointIntegrationService.MapRouteForDeliveryPointForRange(deliveryRouteId, deliveryPointIds);
                }
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return new CreateDeliveryPointForRangeModelDTO { CreateDeliveryPointModelDTOs = returnCreateDeliveryPointModelDTOs };
            }
        }

        /// <summary>
        /// Update DPUse in delivery point for matching UDPRN
        /// </summary>
        /// <param name="postalAddressDetails">postal address record in PAF</param>
        /// <returns>Flag to indicate DPUse updated or not</returns>
        public async Task<bool> UpdateDPUse(PostalAddressDTO postalAddressDetails)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.UpdateDPUse"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(UpdateDPUse);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                if (postalAddressDetails == null)
                {
                    throw new ArgumentNullException(nameof(postalAddressDetails));
                }

                if (postalAddressDetails.UDPRN == null)
                {
                    throw new ArgumentNullException(nameof(postalAddressDetails));
                }

                bool dpUseStatusUpdated = false;
                Guid deliveryPointUseIndicatorGUID = Guid.Empty;
                List<string> listNames = new List<string> { ReferenceDataCategoryNames.DeliveryPointUseIndicator };

                // Get IDs for DeliveryPointUseIndicatorGUID from reference data
                var referenceDataCategoryList = deliveryPointIntegrationService.GetReferenceDataSimpleLists(listNames).Result;
                if (referenceDataCategoryList != null && referenceDataCategoryList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(postalAddressDetails.OrganisationName))
                    {
                        deliveryPointUseIndicatorGUID = referenceDataCategoryList.Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointUseIndicator)
                                                        .SelectMany(x => x.ReferenceDatas)
                                                        .Where(x => x.ReferenceDataValue == ReferenceDataValues.Organisation).Select(x => x.ID)
                                                        .SingleOrDefault();
                    }
                    else
                    {
                        deliveryPointUseIndicatorGUID = referenceDataCategoryList.Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.DeliveryPointUseIndicator)
                                                        .SelectMany(x => x.ReferenceDatas)
                                                        .Where(x => x.ReferenceDataValue == ReferenceDataValues.Residential).Select(x => x.ID)
                                                        .SingleOrDefault();
                    }
                }

                dpUseStatusUpdated = await deliveryPointsDataService.UpdateDPUse(postalAddressDetails.UDPRN.Value, deliveryPointUseIndicatorGUID);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return dpUseStatusUpdated;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// This method is used to fetch GeoJson data for Delivery Point.
        /// </summary>
        /// <param name="lstDeliveryPointDTO">List of Delivery Point Dto</param>
        /// <returns>Delivery point json collection</returns>
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
                        { DeliveryPointConstants.BuildingName, point.PostalAddress.BuildingName },
                        { DeliveryPointConstants.BuildingNumber, point.PostalAddress.BuildingNumber },
                        { DeliveryPointConstants.Postcode, point.PostalAddress.Postcode },
                        { DeliveryPointConstants.StreetName, point.PostalAddress.BuildingName },
                        { DeliveryPointConstants.LayerType, Convert.ToString(OtherLayersType.DeliveryPoint.GetDescription()) },
                        { DeliveryPointConstants.OrganisationName, point.PostalAddress.OrganisationName },
                        { DeliveryPointConstants.DepartmentName, point.PostalAddress.DepartmentName },
                        { DeliveryPointConstants.MailVolume, point.MailVolume },
                        { DeliveryPointConstants.MultipleOccupancyCount, point.MultipleOccupancyCount },
                        { DeliveryPointConstants.Locality, (point.PostalAddress.DependentLocality + DeliveryPointConstants.Space + point.PostalAddress.DoubleDependentLocality).Trim() },
                        { DeliveryPointConstants.DeliveryPointId, point.ID },
                        { DeliveryPointConstants.Street, (point.PostalAddress.Thoroughfare + DeliveryPointConstants.Space + point.PostalAddress.DependentThoroughfare).Trim() },
                        { DeliveryPointConstants.SubBuildingName, point.PostalAddress.SubBuildingName }
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
        /// <returns>Polygon text for given coordinates.</returns>
        private static string GetDeliveryPointsCoordinatesDatabyBoundingBox(params object[] parameters)
        {
            string coordinates = string.Empty;
            if (parameters != null && parameters.Length == 4)
            {
                coordinates = string.Format(
                                     DeliveryPointConstants.Polygon,
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
            string dpUsetype = string.Empty;

            string methodName = MethodBase.GetCurrentMethod().Name;

            using (loggingHelper.RMTraceManager.StartTrace(LoggerTraceConstants.BusinessLayer + methodName))
            {
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                // Guid deliveryOperationObjectTypeId = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.OperationalObjectType, ReferenceDataValues.OperationalObjectTypeDP);
                Guid operationalObjectTypeForDpOrganisation = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.DeliveryPointUseIndicator, ReferenceDataValues.Organisation);
                Guid operationalObjectTypeForDpResidential = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.DeliveryPointUseIndicator, ReferenceDataValues.Residential);

                var deiveryPoint = deliveryPointsDataService.GetDeliveryPoint(deliveryPointId);

                if (deiveryPoint.DeliveryPointUseIndicatorGUID == operationalObjectTypeForDpOrganisation)
                {
                    dpUsetype = DPUseType.Organisation.ToString();
                }
                else if (deiveryPoint.DeliveryPointUseIndicatorGUID == operationalObjectTypeForDpResidential)
                {
                    dpUsetype = DPUseType.Residential.ToString();
                }
            }

            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
            return dpUsetype;
        }

        /// <summary>
        /// Covert delivery point public DTO to data DTO.
        /// </summary>
        /// <param name="deliveryPointDTO">Delivery point data DTO.</param>
        /// <returns>Delivery point public DTO.</returns>
        private DeliveryPointDataDTO ConvertToDataDTO(DeliveryPointDTO deliveryPointDTO)
        {
            DeliveryPointDataDTO deliveryPointDataDTO = new DeliveryPointDataDTO();
            if (deliveryPointDTO != null)
            {
                deliveryPointDataDTO.DeliveryPointUseIndicatorGUID = deliveryPointDTO.DeliveryPointUseIndicator_GUID;
                deliveryPointDataDTO.ID = deliveryPointDTO.ID;
                deliveryPointDataDTO.PostalAddressID = deliveryPointDTO.Address_GUID;
                deliveryPointDataDTO.MailVolume = deliveryPointDTO.MailVolume;
                deliveryPointDataDTO.MultipleOccupancyCount = deliveryPointDTO.MultipleOccupancyCount;
                deliveryPointDataDTO.RowCreateDateTime = deliveryPointDTO.RowCreateDateTime;
                deliveryPointDataDTO.RowVersion = deliveryPointDTO.RowVersion;

                DeliveryPointStatusDataDTO deliveryPointStatusDataDTO = new DeliveryPointStatusDataDTO();
                deliveryPointStatusDataDTO.DeliveryPointStatusGUID = deliveryPointDTO.OperationalStatus_GUID.HasValue ? deliveryPointDTO.OperationalStatus_GUID.Value : Guid.Empty;
                deliveryPointDataDTO.DeliveryPointStatus.Add(deliveryPointStatusDataDTO);

                // network node details
                NetworkNodeDataDTO networkNodeDataDTO = new NetworkNodeDataDTO();
                networkNodeDataDTO.DataProviderGUID = deliveryPointDTO.LocationProvider_GUID;
                networkNodeDataDTO.NetworkNodeType_GUID = deliveryPointDTO.NetworkNodeType_GUID;
                deliveryPointDataDTO.NetworkNode = networkNodeDataDTO;

                // location details
                LocationDataDTO locationDataDTO = new LocationDataDTO();
                locationDataDTO.Shape = deliveryPointDTO.LocationXY;
                networkNodeDataDTO.Location = locationDataDTO;

                if (deliveryPointDTO.PostalAddress != null)
                {
                    // postal address details
                    deliveryPointDataDTO.PostalAddress.ID = deliveryPointDTO.PostalAddress.ID;
                    deliveryPointDataDTO.PostalAddress.OrganisationName = deliveryPointDTO.PostalAddress.OrganisationName;
                    deliveryPointDataDTO.PostalAddress.BuildingName = deliveryPointDTO.PostalAddress.BuildingName;
                    deliveryPointDataDTO.PostalAddress.SubBuildingName = deliveryPointDTO.PostalAddress.SubBuildingName;
                    deliveryPointDataDTO.PostalAddress.BuildingNumber = deliveryPointDTO.PostalAddress.BuildingNumber;
                    deliveryPointDataDTO.PostalAddress.Thoroughfare = deliveryPointDTO.PostalAddress.Thoroughfare;
                    deliveryPointDataDTO.PostalAddress.DependentLocality = deliveryPointDTO.PostalAddress.DependentLocality;
                    deliveryPointDataDTO.PostalAddress.UDPRN = deliveryPointDTO.PostalAddress.UDPRN;
                    deliveryPointDataDTO.PostalAddress.Postcode = deliveryPointDTO.PostalAddress.Postcode;
                    deliveryPointDataDTO.PostalAddress.DeliveryPointSuffix = deliveryPointDTO.PostalAddress.DeliveryPointSuffix;
                    deliveryPointDataDTO.PostalAddress.DependentThoroughfare = deliveryPointDTO.PostalAddress.DependentThoroughfare;
                    deliveryPointDataDTO.PostalAddress.DoubleDependentLocality = deliveryPointDTO.PostalAddress.DoubleDependentLocality;
                    deliveryPointDataDTO.PostalAddress.DepartmentName = deliveryPointDTO.PostalAddress.DepartmentName;
                }
            }

            return deliveryPointDataDTO;
        }

        /// <summary>
        /// Covert delivery point data DTO to public DTO.
        /// </summary>
        /// <param name="deliveryPointDTO">Collection of delivery point public DTO.</param>
        /// <returns>Collection of delivery point data DTO.</returns>
        private List<DeliveryPointDataDTO> ConvertToDataDTO(List<DeliveryPointDTO> deliveryPointDTOList)
        {
            List<DeliveryPointDataDTO> deliveryPointDataDTO = new List<DeliveryPointDataDTO>();

            foreach (var deliveryPointDTO in deliveryPointDTOList)
            {
                deliveryPointDataDTO.Add(ConvertToDataDTO(deliveryPointDTO));
            }

            return deliveryPointDataDTO;
        }

        /// <summary>
        /// Covert data dto to public dto.
        /// </summary>
        /// <param name="deliveryPointDTO">Delivery point data DTO.</param>
        /// <returns>Delivery point public DTO.</returns>
        private DeliveryPointDTO ConvertToDTO(DeliveryPointDataDTO deliveryPointDataDTO)
        {
            DeliveryPointDTO deliveryPointDTO = null;

            if (deliveryPointDataDTO != null)
            {
                deliveryPointDTO = new DeliveryPointDTO();
                deliveryPointDTO.DeliveryPointUseIndicator_GUID = deliveryPointDataDTO.DeliveryPointUseIndicatorGUID;
                deliveryPointDTO.ID = deliveryPointDataDTO.ID;
                deliveryPointDTO.MailVolume = deliveryPointDataDTO.MailVolume;
                deliveryPointDTO.MultipleOccupancyCount = deliveryPointDataDTO.MultipleOccupancyCount;
                deliveryPointDTO.Address_GUID = deliveryPointDataDTO.PostalAddressID;
                deliveryPointDTO.RowCreateDateTime = deliveryPointDataDTO.RowCreateDateTime;
                deliveryPointDTO.RowVersion = deliveryPointDataDTO.RowVersion;

                if (deliveryPointDataDTO.DeliveryPointStatus != null && deliveryPointDataDTO.DeliveryPointStatus.Count > 0)
                {
                    deliveryPointDTO.OperationalStatus_GUID = deliveryPointDataDTO.DeliveryPointStatus.First().DeliveryPointStatusGUID;
                }

                if (deliveryPointDataDTO.NetworkNode != null)
                {
                    // network node details
                    deliveryPointDTO.LocationProvider_GUID = deliveryPointDataDTO.NetworkNode.DataProviderGUID;
                    deliveryPointDTO.NetworkNodeType_GUID = deliveryPointDataDTO.NetworkNode.NetworkNodeType_GUID;

                    if (deliveryPointDataDTO.NetworkNode.Location != null)
                    {
                        // location details
                        deliveryPointDTO.LocationXY = deliveryPointDataDTO.NetworkNode.Location.Shape;
                    }
                }

                if (deliveryPointDataDTO.PostalAddress != null)
                {
                    // postal address details
                    deliveryPointDTO.PostalAddress.ID = deliveryPointDataDTO.PostalAddress.ID;
                    deliveryPointDTO.PostalAddress.OrganisationName = deliveryPointDataDTO.PostalAddress.OrganisationName;
                    deliveryPointDTO.PostalAddress.BuildingName = deliveryPointDataDTO.PostalAddress.BuildingName;
                    deliveryPointDTO.PostalAddress.SubBuildingName = deliveryPointDataDTO.PostalAddress.SubBuildingName;
                    deliveryPointDTO.PostalAddress.BuildingNumber = deliveryPointDataDTO.PostalAddress.BuildingNumber;
                    deliveryPointDTO.PostalAddress.Thoroughfare = deliveryPointDataDTO.PostalAddress.Thoroughfare;
                    deliveryPointDTO.PostalAddress.DependentLocality = deliveryPointDataDTO.PostalAddress.DependentLocality;
                    deliveryPointDTO.PostalAddress.UDPRN = deliveryPointDataDTO.PostalAddress.UDPRN;
                    deliveryPointDTO.PostalAddress.Postcode = deliveryPointDataDTO.PostalAddress.Postcode;
                    deliveryPointDTO.PostalAddress.DeliveryPointSuffix = deliveryPointDataDTO.PostalAddress.DeliveryPointSuffix;
                    deliveryPointDTO.PostalAddress.DependentThoroughfare = deliveryPointDataDTO.PostalAddress.DependentThoroughfare;
                    deliveryPointDTO.PostalAddress.DoubleDependentLocality = deliveryPointDataDTO.PostalAddress.DoubleDependentLocality;
                    deliveryPointDTO.PostalAddress.DepartmentName = deliveryPointDataDTO.PostalAddress.DepartmentName;
                }
            }

            return deliveryPointDTO;
        }

        /// <summary>
        /// Covert collection of delivery point data DTO to public DTO.
        /// </summary>
        /// <param name="deliveryPointDataDTOList">Collection of delivery point data DTO.</param>
        /// <returns>Collection of delivery point public dtDTOo.</returns>
        private List<DeliveryPointDTO> ConvertToDTO(List<DeliveryPointDataDTO> deliveryPointDataDTOList)
        {
            List<DeliveryPointDTO> deliveryPointDTO = new List<DeliveryPointDTO>();

            foreach (var deliveryPointDataDTO in deliveryPointDataDTOList)
            {
                deliveryPointDTO.Add(ConvertToDTO(deliveryPointDataDTO));
            }

            return deliveryPointDTO;
        }

        private string GetValuesOnRangeType(string rangeType, int fromRange, int toRange)
        {
            string returnValue = string.Empty;
            int count = fromRange;

            IEnumerable<int> rangeValues = null;

            if (rangeType.Equals(DeliveryPointConstants.RangeTypeOdds))
            {
                rangeValues = Enumerable.Range(fromRange, (toRange - fromRange) + 1).Where(x => x % 2 == 1);
            }
            else if (rangeType.Equals(DeliveryPointConstants.RangeTypeEvens))
            {
                rangeValues = Enumerable.Range(fromRange, (toRange - fromRange) + 1).Where(x => x % 2 == 0);
            }
            else if (rangeType.Equals(DeliveryPointConstants.RangeTypeConsecutive))
            {
                rangeValues = Enumerable.Range(fromRange, (toRange - fromRange) + 1);
            }
            else
            {
                throw new NotImplementedException("Condition is not implemented");
            }

            rangeValues.ToList().ForEach(range =>
            {
                if (string.IsNullOrEmpty(returnValue))
                {
                    returnValue += range.ToString();
                }
                else
                {
                    returnValue += "," + range.ToString();
                }
            });

            return returnValue;
        }

        /// <summary>
        /// Add multiple postal addresses and delivery points
        /// </summary>
        /// <param name="addDeliveryPointDTO">Add delivery point DTO</param>
        /// <returns>Create Delivery point return object</returns>
        private List<PostalAddressDTO> GetMultipleAddressesForDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            string[] ranges = GetValuesOnRangeType(addDeliveryPointDTO.RangeType, addDeliveryPointDTO.FromRange, addDeliveryPointDTO.ToRange).Split(',');
            List<PostalAddressDTO> postalAddressDTOs = new List<PostalAddressDTO>();

            if (addDeliveryPointDTO.DeliveryPointType.Equals(DeliveryPointConstants.DeliveryPointTypeRange))
            {
                foreach (string range in ranges)
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<PostalAddressDTO, PostalAddressDTO>());
                    PostalAddressDTO postalAddressDTO = Mapper.Map<PostalAddressDTO>(addDeliveryPointDTO.PostalAddressDTO);
                    postalAddressDTO.ID = Guid.NewGuid();
                    postalAddressDTO.BuildingNumber = !string.IsNullOrEmpty(range) ? (short?)Convert.ToInt16(range) : null;
                    postalAddressDTO.DeliveryPointUseIndicator_GUID = addDeliveryPointDTO.DeliveryPointDTO.DeliveryPointUseIndicator_GUID;
                    postalAddressDTO.DeliveryRoute_Guid = addDeliveryPointDTO.DeliveryPointDTO.DeliveryRoute_Guid;
                    postalAddressDTO.BuildingName = null;
                    postalAddressDTO.SubBuildingName = null;
                    postalAddressDTO.OrganisationName = null;
                    postalAddressDTO.DepartmentName = null;
                    postalAddressDTOs.Add(postalAddressDTO);
                }
            }
            else if (addDeliveryPointDTO.DeliveryPointType.Equals(DeliveryPointConstants.DeliveryPointTypeSubBuildingRange))
            {
                foreach (string range in ranges)
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<PostalAddressDTO, PostalAddressDTO>());
                    PostalAddressDTO postalAddressDTO = Mapper.Map<PostalAddressDTO>(addDeliveryPointDTO.PostalAddressDTO);
                    postalAddressDTO.ID = Guid.NewGuid();
                    postalAddressDTO.SubBuildingName = !string.IsNullOrEmpty(addDeliveryPointDTO.SubBuildingType) ? $"{addDeliveryPointDTO.SubBuildingType} {range}" : range;
                    postalAddressDTO.DeliveryPointUseIndicator_GUID = addDeliveryPointDTO.DeliveryPointDTO.DeliveryPointUseIndicator_GUID;
                    postalAddressDTO.DeliveryRoute_Guid = addDeliveryPointDTO.DeliveryPointDTO.DeliveryRoute_Guid;
                    postalAddressDTO.OrganisationName = null;
                    postalAddressDTO.DepartmentName = null;
                    postalAddressDTOs.Add(postalAddressDTO);
                }
            }
            else
            {
                foreach (string range in ranges)
                {
                    Mapper.Initialize(cfg => cfg.CreateMap<PostalAddressDTO, PostalAddressDTO>());
                    PostalAddressDTO postalAddressDTO = Mapper.Map<PostalAddressDTO>(addDeliveryPointDTO.PostalAddressDTO);
                    postalAddressDTO.ID = Guid.NewGuid();
                    postalAddressDTO.BuildingNumber = short.Parse(range);
                    postalAddressDTO.DeliveryPointUseIndicator_GUID = addDeliveryPointDTO.DeliveryPointDTO.DeliveryPointUseIndicator_GUID;
                    postalAddressDTO.DeliveryRoute_Guid = addDeliveryPointDTO.DeliveryPointDTO.DeliveryRoute_Guid;
                    postalAddressDTO.SubBuildingName = null;
                    postalAddressDTO.OrganisationName = null;
                    postalAddressDTO.DepartmentName = null;
                    postalAddressDTOs.Add(postalAddressDTO);
                }
            }

            return postalAddressDTOs;
        }

        private void RemoveDuplicateAddresses(List<PostalAddressDTO> postalAddresses, List<PostalAddressDTO> duplicatePostalAddresses)
        {
            foreach (PostalAddressDTO postalAddress in duplicatePostalAddresses)
            {
                IQueryable<PostalAddressDTO> postalAddressDTO = postalAddresses.AsQueryable();

                if (!string.IsNullOrEmpty(postalAddress.BuildingName))
                {
                    postalAddressDTO = postalAddressDTO.Where(n => n.BuildingName.Equals(postalAddress.BuildingName, StringComparison.OrdinalIgnoreCase));
                }

                if (postalAddress.BuildingNumber != null)
                {
                    postalAddressDTO = postalAddressDTO.Where(n => n.BuildingNumber == postalAddress.BuildingNumber);
                }

                if (!string.IsNullOrEmpty(postalAddress.SubBuildingName))
                {
                    postalAddressDTO = postalAddressDTO.Where(n => n.SubBuildingName.Equals(postalAddress.SubBuildingName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(postalAddress.OrganisationName))
                {
                    postalAddressDTO = postalAddressDTO.Where(n => n.OrganisationName.Equals(postalAddress.OrganisationName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(postalAddress.DepartmentName))
                {
                    postalAddressDTO = postalAddressDTO.Where(n => n.DepartmentName.Equals(postalAddress.DepartmentName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(postalAddress.Thoroughfare))
                {
                    postalAddressDTO = postalAddressDTO.Where(n => n.Thoroughfare.Equals(postalAddress.Thoroughfare, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(postalAddress.DependentThoroughfare))
                {
                    postalAddressDTO = postalAddressDTO.Where(n => n.DependentThoroughfare.Equals(postalAddress.DependentThoroughfare, StringComparison.OrdinalIgnoreCase));
                }

                //PostalAddressDTO duplicatePostalAddress = postalAddresses.Where(pa => (pa.BuildingName != null && pa.BuildingName.ToUpper() == postalAddress.BuildingName.ToUpper())
                //                                                                        && (pa.BuildingNumber != null && pa.BuildingNumber == postalAddress.BuildingNumber)
                //                                                                        && (pa.SubBuildingName != null && pa.SubBuildingName.ToUpper() == postalAddress.SubBuildingName.ToUpper())
                //                                                                        && (pa.OrganisationName != null && pa.OrganisationName.ToUpper() == postalAddress.OrganisationName.ToUpper())
                //                                                                        && (pa.DepartmentName != null && pa.DepartmentName.ToUpper() == postalAddress.DepartmentName.ToUpper())
                //                                                                        && (pa.Thoroughfare != null && pa.Thoroughfare.ToUpper() == postalAddress.Thoroughfare.ToUpper())
                //                                                                        && (pa.DependentThoroughfare != null && pa.DependentThoroughfare.ToUpper() == postalAddress.DependentThoroughfare.ToUpper()))
                //                                                                        .FirstOrDefault();

                postalAddresses.Remove(postalAddressDTO.FirstOrDefault());
            }

            //duplicatePostalAddresses.ForEach(duplicatePA => postalAddresses.Remove(postalAddresses.Where(pa => (pa.BuildingName != null && pa.BuildingName.ToUpper() == duplicatePA.BuildingName.ToUpper())
            //                                                                            && (pa.BuildingNumber != null && pa.BuildingNumber == duplicatePA.BuildingNumber)
            //                                                                            && (pa.SubBuildingName != null && pa.SubBuildingName.ToUpper() == duplicatePA.SubBuildingName.ToUpper())
            //                                                                            && (pa.OrganisationName != null && pa.OrganisationName.ToUpper() == duplicatePA.OrganisationName.ToUpper())
            //                                                                            && (pa.DepartmentName != null && pa.DepartmentName.ToUpper() == duplicatePA.DepartmentName.ToUpper())
            //                                                                            && (pa.Thoroughfare != null && pa.Thoroughfare.ToUpper() == duplicatePA.Thoroughfare.ToUpper())
            //                                                                            && (pa.DependentThoroughfare != null && pa.DependentThoroughfare.ToUpper() == duplicatePA.DependentThoroughfare.ToUpper()))
            //                                                                            .FirstOrDefault()));
        }

        #endregion Private Methods
    }
}