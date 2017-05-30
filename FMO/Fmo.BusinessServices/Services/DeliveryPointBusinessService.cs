﻿namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common;
    using Fmo.Common.Constants;
    using Fmo.Common.Enums;
    using Fmo.Common.Interface;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.DTO.Model;
    using Fmo.Helpers;
    using Microsoft.SqlServer.Types;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// This class contains methods for fetching Delivery Points data.
    /// </summary>
    public class DeliveryPointBusinessService : IDeliveryPointBusinessService
    {
        #region Member Variables

        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private IPostalAddressBusinessService postalAddressBusinessService = default(IPostalAddressBusinessService);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IReferenceDataBusinessService referenceDataBusinessService = default(IReferenceDataBusinessService);
        private IAccessLinkBusinessService accessLinkBusinessService = default(IAccessLinkBusinessService);
        private IBlockSequenceBusinessService blockSequenceBusinessService = default(IBlockSequenceBusinessService);

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
            IDeliveryPointsRepository deliveryPointsRepository,
            IPostalAddressBusinessService postalAddressBusinessService,
            ILoggingHelper loggingHelper,
            IConfigurationHelper configurationHelper,
            IReferenceDataBusinessService referenceDataBusinessService,
            IAccessLinkBusinessService accessLinkBusinessService,
            IBlockSequenceBusinessService blockSequenceBusinessService)
        {
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.loggingHelper = loggingHelper;
            this.configurationHelper = configurationHelper;
            this.postalAddressBusinessService = postalAddressBusinessService;
            this.referenceDataBusinessService = referenceDataBusinessService;
            this.accessLinkBusinessService = accessLinkBusinessService;
            this.blockSequenceBusinessService = blockSequenceBusinessService;
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
            if (!string.IsNullOrEmpty(boundaryBox))
            {
                var coordinates = GetDeliveryPointsCoordinatesDatabyBoundingBox(boundaryBox.Split(Constants.Comma[0]));
                return GetDeliveryPointsJsonData(deliveryPointsRepository.GetDeliveryPoints(coordinates, unitGuid));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get coordinates of the delivery point by UDPRN
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        public object GetDeliveryPointByUDPRN(int udprn)
        {
            return GetDeliveryPointsJsonData(deliveryPointsRepository.GetDeliveryPointListByUDPRN(udprn));
        }

        /// <summary>
        /// This method is used to fetch .......
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        public AddDeliveryPointDTO GetDetailDeliveryPointByUDPRN(int udprn)
        {
            return deliveryPointsRepository.GetDetailDeliveryPointByUDPRN(udprn);
        }

        /// <summary>
        /// Fetch the Delivery point for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForBasicSearch(string searchText, Guid userUnit)
        {
            return await deliveryPointsRepository.FetchDeliveryPointsForBasicSearch(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of delivery point</returns>
        public async Task<int> GetDeliveryPointsCount(string searchText, Guid userUnit)
        {
            return await deliveryPointsRepository.GetDeliveryPointsCount(searchText, userUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// This method is used to fetch delivery points for advance search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// Task List of Delivery Point Dto
        /// </returns>
        public async Task<List<DeliveryPointDTO>> FetchDeliveryPointsForAdvanceSearch(string searchText, Guid unitGuid)
        {
            return await deliveryPointsRepository.FetchDeliveryPointsForAdvanceSearch(searchText, unitGuid);
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>string</returns>
        public CreateDeliveryPointModelDTO CreateDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            using (loggingHelper.FmoTraceManager.StartTrace("Business.AddDeliveryPoint"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.LogInfo(methodName + Constants.COLON + Constants.MethodExecutionStarted, LoggerTraceConstants.Category, LoggerTraceConstants.CreateDeliveryPointPriority, LoggerTraceConstants.CreateDeliveryPoinBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                string addDeliveryDtoLogDetails = new JavaScriptSerializer().Serialize(addDeliveryPointDTO);
                string message = string.Empty;
                double? returnXCoordinate = 0;
                double? returnYCoordinate = 0;
                Guid returnGuid = new Guid(Constants.DEFAULTGUID);
                byte[] rowVersion = null;

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null &&
                    addDeliveryPointDTO.DeliveryPointDTO != null)
                {
                    string postCode =
                        postalAddressBusinessService.CheckForDuplicateNybRecords(addDeliveryPointDTO.PostalAddressDTO);
                    if (addDeliveryPointDTO.PostalAddressDTO.ID == Guid.Empty &&
                        postalAddressBusinessService.CheckForDuplicateAddressWithDeliveryPoints(addDeliveryPointDTO
                            .PostalAddressDTO))
                    {
                        message = Constants.DUPLICATEDELIVERYPOINT;
                    }
                    else if (addDeliveryPointDTO.PostalAddressDTO.ID == Guid.Empty && !string.IsNullOrEmpty(postCode))
                    {
                        message = Constants.DUPLICATENYBRECORDS + postCode;
                    }
                    else
                    {
                        CreateDeliveryPointModelDTO createDeliveryPointModelDTO =
                            postalAddressBusinessService.CreateAddressAndDeliveryPoint(addDeliveryPointDTO);
                        rowVersion = deliveryPointsRepository.GetDeliveryPointRowVersion(createDeliveryPointModelDTO.ID);
                        returnGuid = createDeliveryPointModelDTO.ID;
                        blockSequenceBusinessService.CreateBlockSequenceForDeliveryPoint(addDeliveryPointDTO.DeliveryPointDTO.DeliveryRoute_Guid, returnGuid);
                        returnXCoordinate = createDeliveryPointModelDTO.XCoordinate;
                        returnYCoordinate = createDeliveryPointModelDTO.YCoordinate;

                        if (createDeliveryPointModelDTO.IsAddressLocationAvailable)
                        {
                            var referenceDataCategoryList =
                                referenceDataBusinessService.GetReferenceDataCategoriesByCategoryNames(new List<string>
                                {
                                ReferenceDataCategoryNames.OperationalObjectType
                                });

                            var deliveryOperationObjectTypeId = referenceDataCategoryList
                                .Where(x => x.CategoryName == ReferenceDataCategoryNames.OperationalObjectType)
                                .SelectMany(x => x.ReferenceDatas)
                                .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID;

                            var isAccessLinkCreated =
                                accessLinkBusinessService.CreateAccessLink(
                                    createDeliveryPointModelDTO.ID,
                                    deliveryOperationObjectTypeId);

                            message = isAccessLinkCreated
                                ? Constants.DELIVERYPOINTCREATED
                                : Constants.DELIVERYPOINTCREATEDWITHOUTACCESSLINK;
                        }
                        else
                        {
                            message = Constants.DELIVERYPOINTCREATEDWITHOUTLOCATION;
                        }
                    }
                }

                loggingHelper.LogInfo(methodName + Constants.COLON + Constants.MethodExecutionCompleted, LoggerTraceConstants.Category, LoggerTraceConstants.CreateDeliveryPointPriority, LoggerTraceConstants.CreateDeliveryPoinBusinessMethodExitEventId, LoggerTraceConstants.Title);

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
            using (loggingHelper.FmoTraceManager.StartTrace("Business.UpdateDeliveryPointLocation"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;

                loggingHelper.LogInfo(methodName + Constants.COLON + Constants.MethodExecutionCompleted, LoggerTraceConstants.Category, LoggerTraceConstants.UpdateDeliveryPointPriority, LoggerTraceConstants.UpdateDeliveryPoinBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                Guid returnGuid = Guid.Empty;
                string sbLocationXY = string.Format(
                                                    Constants.USRGEOMETRYPOINT,
                                                    Convert.ToString(deliveryPointModelDTO.XCoordinate),
                                                    Convert.ToString(deliveryPointModelDTO.YCoordinate));

                DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), Constants.BNGCOORDINATESYSTEM);

                Guid locationProviderId = referenceDataBusinessService.GetReferenceDataId(Constants.NETWORKLINKDATAPROVIDER, Constants.INTERNAL);

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

                await deliveryPointsRepository.UpdateDeliveryPointLocationOnUDPRN(deliveryPointDTO).ContinueWith(t =>
                {
                    if (t.IsFaulted && t.Exception != null)
                    {
                        throw t.Exception;
                    }

                    if (t.Result != Guid.Empty)
                    {
                        returnGuid = t.Result;
                        var referenceDataCategoryList =
                                 referenceDataBusinessService.GetReferenceDataCategoriesByCategoryNames(new List<string>
                                 {
                             ReferenceDataCategoryNames.OperationalObjectType
                                 });

                        var deliveryOperationObjectTypeId = referenceDataCategoryList
                            .Where(x => x.CategoryName == ReferenceDataCategoryNames.OperationalObjectType)
                            .SelectMany(x => x.ReferenceDatas)
                            .Single(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).ID;

                        accessLinkBusinessService.CreateAccessLink(deliveryPointModelDTO.ID, deliveryOperationObjectTypeId);
                    }
                });
                loggingHelper.LogInfo(methodName + Constants.COLON + Constants.MethodExecutionCompleted, LoggerTraceConstants.Category, LoggerTraceConstants.UpdateDeliveryPointPriority, LoggerTraceConstants.UpdateDeliveryPoinBusinessMethodExitEventId, LoggerTraceConstants.Title);

                return new UpdateDeliveryPointModelDTO { XCoordinate = deliveryPointModelDTO.XCoordinate, YCoordinate = deliveryPointModelDTO.YCoordinate };
            }
        }

        /// <summary>
        /// This Method fetches Route and DPUse for a single DeliveryPoint
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as GUID</param>
        /// <returns>KeyValuePair for Route and DPUse </returns>
        public List<KeyValuePair<string, string>> GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            using (loggingHelper.FmoTraceManager.StartTrace(LoggerTraceConstants.BusinessLayer + methodName))
            {
                loggingHelper.LogInfo(methodName + Constants.COLON + Constants.MethodExecutionStarted, LoggerTraceConstants.Category, LoggerTraceConstants.GetRouteForDeliveryPointPriority, LoggerTraceConstants.GetRouteForDeliveryPointBusinessMethodEntryEventId, LoggerTraceConstants.Title);

                List<KeyValuePair<string, string>> dpDetails = new List<KeyValuePair<string, string>>();
                string routeName = deliveryPointsRepository.GetRouteForDeliveryPoint(deliveryPointId);
                string dpUse = GetDPUse(deliveryPointId);
                if (routeName != null)
                {
                    dpDetails.Add(new KeyValuePair<string, string>(Constants.RouteName, routeName));
                }

                dpDetails.Add(new KeyValuePair<string, string>(Constants.DpUse, dpUse));
                loggingHelper.LogInfo(methodName + Constants.COLON + Constants.MethodExecutionCompleted, LoggerTraceConstants.Category, LoggerTraceConstants.GetRouteForDeliveryPointPriority, LoggerTraceConstants.GetRouteForDeliveryPointBusinessMethodExitEventId, LoggerTraceConstants.Title);
                return dpDetails;
            }
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
            using (loggingHelper.FmoTraceManager.StartTrace(LoggerTraceConstants.BusinessLayer + methodName))
            {
                loggingHelper.LogInfo(methodName + Constants.COLON + Constants.MethodExecutionStarted, LoggerTraceConstants.Category, LoggerTraceConstants.GetDPUsePriority, LoggerTraceConstants.GetDPUseBusinessMethodEntryEventId, LoggerTraceConstants.Title);
                List<string> categoryNames = new List<string>
                {
                    ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                };

                var referenceDataCategoryList =
                    referenceDataBusinessService.GetReferenceDataCategoriesByCategoryNames(categoryNames);
                string dpUsetype = deliveryPointsRepository.GetDPUse(referenceDataCategoryList, deliveryPointId);
                loggingHelper.LogInfo(methodName + Constants.COLON + Constants.MethodExecutionCompleted, LoggerTraceConstants.Category, LoggerTraceConstants.GetDPUsePriority, LoggerTraceConstants.GetDPUseBusinessMethodExitEventId, LoggerTraceConstants.Title);
                return dpUsetype;
            }
        }

        #endregion Private Methods
    }
}