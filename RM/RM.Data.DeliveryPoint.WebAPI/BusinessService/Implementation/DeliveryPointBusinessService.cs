﻿namespace RM.DataManagement.DeliveryPoint.WebAPI.BusinessService
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
        /// <returns>Task</returns>
        public async Task<List<DeliveryPointDTO>> GetDeliveryPointsForBasicSearch(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryPointsForBasicSearch"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointsForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var fetchDeliveryPointsForBasicSearch = await deliveryPointsDataService.GetDeliveryPointsForBasicSearch(searchText, userUnit).ConfigureAwait(false);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return ConvertToDTO(fetchDeliveryPointsForBasicSearch);
            }
        }

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery point</returns>
        public async Task<int> GetDeliveryPointsCount(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryPointsCount"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointsCount);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDeliveryPointsCount = await deliveryPointsDataService.GetDeliveryPointsCount(searchText, userUnit).ConfigureAwait(false);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return getDeliveryPointsCount;
            }
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        public async Task<List<DeliveryPointDTO>> GetDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryPointsForAdvanceSearch"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetDeliveryPointsForAdvanceSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var fetchDeliveryPointsForAdvanceSearch = await deliveryPointsDataService.GetDeliveryPointsForAdvanceSearch(searchText, unitGuid).ConfigureAwait(false);
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.AddDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(CreateDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string addDeliveryDtoLogDetails = new JavaScriptSerializer().Serialize(addDeliveryPointDTO);
                string message = string.Empty;
                double? returnXCoordinate = 0;
                double? returnYCoordinate = 0;
                Guid returnGuid = new Guid(DeliveryPointConstants.DEFAULTGUID);
                byte[] rowVersion = null;

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null &&
                    addDeliveryPointDTO.DeliveryPointDTO != null)
                {
                    // TODO: Need to integrate with posal address API

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
                        CreateDeliveryPointModelDTO createDeliveryPointModelDTO = await deliveryPointIntegrationService.CreateAddressAndDeliveryPoint(addDeliveryPointDTO);

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
                            // if the exact location is present
                            deliveryPointdataDTO.NetworkNode.Location.Shape =
                                DbGeometry.PointFromText("POINT(" + createDeliveryPointModelDTO.XCoordinate + "," + createDeliveryPointModelDTO.YCoordinate + ")", DeliveryPointConstants.BNGCOORDINATESYSTEM);

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

                            //TODO: Get approx location based on the postal address Id
                            deliveryPointdataDTO.NetworkNode.Location.Shape = deliveryPointIntegrationService.GetApproxLocation(addDeliveryPointDTO.PostalAddressDTO.Postcode).Result;

                            DeliveryPointStatusDataDTO deliveryPointStatusDataDTO = new DeliveryPointStatusDataDTO();

                            Guid liveWithPendingLocationStatusId = referenceDataCategoryList
                                          .Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.PostalAddressStatus)
                                          .SelectMany(x => x.ReferenceDatas)
                                          .Where(x => x.ReferenceDataValue == DeliveryPointConstants.OperationalStatusGUIDLivePendingLocation).Select(x => x.ID)
                                          .SingleOrDefault();

                            deliveryPointStatusDataDTO.DeliveryPointStatusGUID = liveWithPendingLocationStatusId;

                            deliveryPointdataDTO.DeliveryPointStatus.Add(deliveryPointStatusDataDTO);
                        }

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
            string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(UpdateDeliveryPointLocation);
            loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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

            Guid deliveryPointUseIndicator = referenceDataCategoryList
                                .Where(list => list.CategoryName.Equals(DeliveryPointConstants.DeliveryPointUseIndicator, StringComparison.OrdinalIgnoreCase))
                                .SelectMany(list => list.ReferenceDatas)
                                .Where(item => item.ReferenceDataValue.Equals(DeliveryPointConstants.DeliveryPointUseIndicatorPAF, StringComparison.OrdinalIgnoreCase))
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
                DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,
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

        /// <summary>
        /// This Method fetches Route and DPUse for a single DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>KeyValuePair for Route and DPUse</returns>
        public List<KeyValuePair<string, string>> GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            return null;

            // TODO: Move this method to delivery route manager

            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteForDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointBusinessService) + "." + nameof(GetRouteForDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<KeyValuePair<string, string>> dpDetails = new List<KeyValuePair<string, string>>();
                string routeName = deliveryPointIntegrationService.GetRouteForDeliveryPoint(deliveryPointId).Result;
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

                var deliveryPointId = await deliveryPointsDataService.UpdateDeliveryPointLocationOnID(ConvertToDataDTO((deliveryPointDto)));
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
        /// <param name="id">Delivery point unique identifier.</param>
        /// <returns>Boolean flag indicating success of operation.</returns>
        public Task<bool> DeleteDeliveryPoint(Guid id)
        {
            return deliveryPointsDataService.DeleteDeliveryPoint(id);
        }

        public async Task<DeliveryPointDTO> GetDeliveryPointByPostalAddressWithLocation(Guid addressId)
        {
            return ConvertToDTO(await deliveryPointsDataService.GetDeliveryPointByPostalAddressWithLocation(addressId));
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
        /// <returns>coordinates</returns>
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
                //  Guid deliveryOperationObjectTypeId = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.OperationalObjectType, ReferenceDataValues.OperationalObjectTypeDP);
                Guid operationalObjectTypeForDpOrganisation = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.DeliveryPointUseIndicator, ReferenceDataValues.Organisation);
                Guid operationalObjectTypeForDpResidential = deliveryPointIntegrationService.GetReferenceDataGuId(ReferenceDataCategoryNames.DeliveryPointUseIndicator, ReferenceDataValues.Residential);

                var deiveryPoint = deliveryPointsDataService.GetDeliveryPoint(deliveryPointId);

                if (deiveryPoint.DeliveryPointUseIndicatorGUID == operationalObjectTypeForDpOrganisation)
                {
                    dpUsetype = DPUseType.Organisation.ToString();
                }
                else if (deiveryPoint.DeliveryPointUseIndicatorGUID == operationalObjectTypeForDpResidential)
                {
                    dpUsetype = DPUseType.Organisation.ToString();
                }
            }

            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
            return dpUsetype;
        }

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
                deliveryPointDataDTO.PostalAddressID = deliveryPointDTO.PostalAddress.ID;
                deliveryPointDataDTO.RowCreateDateTime = deliveryPointDTO.RowCreateDateTime;
                deliveryPointDataDTO.RowVersion = deliveryPointDTO.RowVersion;

                DeliveryPointStatusDataDTO deliveryPointStatusDataDTO = new DeliveryPointStatusDataDTO();
                deliveryPointStatusDataDTO.DeliveryPointStatusGUID = deliveryPointDTO.OperationalStatus_GUID.HasValue ? deliveryPointDTO.OperationalStatus_GUID.Value : Guid.Empty;
                deliveryPointDataDTO.DeliveryPointStatus.Add(deliveryPointStatusDataDTO);

                //network node details
                deliveryPointDataDTO.NetworkNode.DataProviderGUID = deliveryPointDTO.LocationProvider_GUID;
                deliveryPointDataDTO.NetworkNode.NetworkNodeType_GUID = deliveryPointDTO.NetworkNodeType_GUID;

                // location details
                deliveryPointDataDTO.NetworkNode.Location.Shape = deliveryPointDTO.LocationXY;
            }

            return deliveryPointDataDTO;
        }

        private List<DeliveryPointDataDTO> ConvertToDataDTO(List<DeliveryPointDTO> deliveryPointDTOList)
        {
            List<DeliveryPointDataDTO> deliveryPointDataDTO = new List<DeliveryPointDataDTO>();

            foreach (var deliveryPointDTO in deliveryPointDTOList)
            {
                deliveryPointDataDTO.Add(ConvertToDataDTO(deliveryPointDTO));
            }

            return deliveryPointDataDTO;
        }

        private DeliveryPointDTO ConvertToDTO(DeliveryPointDataDTO deliveryPointDataDTO)
        {
            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO();

            if (deliveryPointDTO != null)
            {
                deliveryPointDTO.DeliveryPointUseIndicator_GUID = deliveryPointDataDTO.DeliveryPointUseIndicatorGUID;
                deliveryPointDTO.ID = deliveryPointDataDTO.ID;
                deliveryPointDTO.MailVolume = deliveryPointDataDTO.MailVolume;
                deliveryPointDTO.MultipleOccupancyCount = deliveryPointDataDTO.MultipleOccupancyCount;
                deliveryPointDTO.Address_GUID = deliveryPointDataDTO.PostalAddressID;
                deliveryPointDTO.RowCreateDateTime = deliveryPointDataDTO.RowCreateDateTime;
                deliveryPointDTO.RowVersion = deliveryPointDataDTO.RowVersion;

                if (deliveryPointDataDTO.DeliveryPointStatus != null)
                {
                    deliveryPointDTO.OperationalStatus_GUID = deliveryPointDataDTO.DeliveryPointStatus.First().DeliveryPointStatusGUID;
                }

                if (deliveryPointDataDTO.NetworkNode != null)
                {
                    //network node details
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
                    //postal address details
                    deliveryPointDTO.PostalAddress.ID = deliveryPointDataDTO.PostalAddress.ID;
                    deliveryPointDTO.PostalAddress.OrganisationName = deliveryPointDataDTO.PostalAddress.OrganisationName;
                    deliveryPointDTO.PostalAddress.BuildingName = deliveryPointDataDTO.PostalAddress.BuildingName;
                    deliveryPointDTO.PostalAddress.SubBuildingName = deliveryPointDataDTO.PostalAddress.SubBuildingName;
                    deliveryPointDTO.PostalAddress.BuildingNumber = deliveryPointDataDTO.PostalAddress.BuildingNumber;
                    deliveryPointDTO.PostalAddress.Thoroughfare = deliveryPointDataDTO.PostalAddress.Thoroughfare;
                    deliveryPointDTO.PostalAddress.DependentLocality = deliveryPointDataDTO.PostalAddress.DependentLocality;
                    deliveryPointDTO.PostalAddress.UDPRN = deliveryPointDataDTO.PostalAddress.UDPRN;
                }
            }

            return deliveryPointDTO;
        }

        private List<DeliveryPointDTO> ConvertToDTO(List<DeliveryPointDataDTO> deliveryPointDataDTOList)
        {
            List<DeliveryPointDTO> deliveryPointDTO = new List<DeliveryPointDTO>();

            foreach (var deliveryPointDataDTO in deliveryPointDataDTOList)
            {
                deliveryPointDTO.Add(ConvertToDTO(deliveryPointDataDTO));
            }

            return deliveryPointDTO;
        }

        #endregion Private Methods
    }
}