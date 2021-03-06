﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using AutoMapper;
using Microsoft.SqlServer.Types;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.PostalAddress.WebAPI.DTO.Model;
using RM.Data.PostalAddress.WebAPI.Utils;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.DataDTO;
using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.DataManagement.PostalAddress.WebAPI.DTO.Model;
using RM.DataManagement.PostalAddress.WebAPI.IntegrationService.Interface;

namespace RM.DataManagement.PostalAddress.WebAPI.BusinessService.Implementation
{
    /// <summary>
    /// Business service to handle CRUD operations on Postal Address entites
    /// </summary>
    public class PostalAddressBusinessService : IPostalAddressBusinessService
    {
        #region Property Declarations

        private IPostalAddressDataService addressDataService = default(IPostalAddressDataService);

        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IPostalAddressIntegrationService postalAddressIntegrationService = default(IPostalAddressIntegrationService);

        #endregion Property Declarations

        #region Constructor

        public PostalAddressBusinessService(
            IPostalAddressDataService addressDataService,
            ILoggingHelper loggingHelper,
            IConfigurationHelper configurationHelper,
            IHttpHandler httpHandler,
            IPostalAddressIntegrationService postalAddressIntegrationService)
        {
            this.addressDataService = addressDataService;
            this.loggingHelper = loggingHelper;
            this.configurationHelper = configurationHelper;
            this.httpHandler = httpHandler;
            this.postalAddressIntegrationService = postalAddressIntegrationService;
        }

        #endregion Constructor

        #region public methods

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDTO> GetPostalAddress(int? uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddress"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPostalAddress);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                PostalAddressDataDTO postalAddressDBDTO = await addressDataService.GetPostalAddress(uDPRN);

                var postalAddress = ConvertToDTO(postalAddressDBDTO);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postalAddress;
            }
        }

        /// <summary>
        /// Save list of NYB details into database.
        /// </summary>
        /// <param name="lstPostalAddress">List Of address DTO</param>
        /// <param name="strFileName">CSV filename</param>
        /// <returns>returns true or false</returns>
        public async Task<bool> SavePostalAddressForNYB(List<PostalAddressDTO> lstPostalAddress, string strFileName)
        {
            bool isPostalAddressInserted = false;
            string postalAddressList = new JavaScriptSerializer().Serialize(lstPostalAddress);

            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SavePostalAddressForNYB"))
                {
                    string methodName = typeof(PostalAddressBusinessService) + "." + nameof(SavePostalAddressForNYB);
                    loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);
                    List<string> categoryNamesSimpleLists = new List<string>
                    {
                        PostalAddressConstants.PostalAddressType,
                        PostalAddressConstants.PostalAddressStatus
                    };

                    Guid addressTypeId = GetReferenceData(categoryNamesSimpleLists, PostalAddressConstants.PostalAddressType, FileType.Nyb.ToString());
                    Guid addressStatusId = GetReferenceData(categoryNamesSimpleLists, PostalAddressConstants.PostalAddressStatus, PostCodeStatus.Live.GetDescription());

                    Mapper.Initialize(cfg =>
                    {
                        cfg.CreateMap<PostalAddressDTO, PostalAddressDataDTO>();
                    });
                    Mapper.Configuration.CreateMapper();
                    List<PostalAddressDataDTO> lstPostalAddressDBDTO = Mapper.Map<List<PostalAddressDTO>, List<PostalAddressDataDTO>>(lstPostalAddress);

                    if (lstPostalAddressDBDTO != null && lstPostalAddressDBDTO.Count > 0)
                    {
                        List<int> lstUDPRNS = lstPostalAddressDBDTO.Select(n => (n.UDPRN != null ? n.UDPRN.Value : 0)).ToList();
                        if (!lstUDPRNS.All(a => a == 0))
                        {
                            foreach (var postalAddress in lstPostalAddressDBDTO)
                            {
                                postalAddress.PostalAddressStatus.Add(GetPostalAddressStatus(postalAddress.ID, addressStatusId));
                                postalAddress.AddressType_GUID = addressTypeId;
                                await addressDataService.SaveAddress(postalAddress, strFileName, addressStatusId);
                            }

                            isPostalAddressInserted = await addressDataService.DeleteNYBPostalAddress(lstUDPRNS, addressTypeId);
                        }
                    }

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                }
            }
            catch (Exception ex)
            {
                this.loggingHelper.Log(ex.ToString(), TraceEventType.Error, ex);
                this.loggingHelper.Log(postalAddressList, TraceEventType.Information);
                throw;
            }

            return isPostalAddressInserted;
        }

        /// <summary>
        /// Business rules for PAF details
        /// </summary>
        /// <param name="lstPostalAddress">list of PostalAddress DTO</param>
        /// <returns>returns true or false</returns>
        public async Task<bool> ProcessPAFDetails(List<PostalAddressDTO> lstPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.ProcessPAFDetails"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(ProcessPAFDetails);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                bool isPostalAddressProcessed = false;
                string postalAddressList = new JavaScriptSerializer() { MaxJsonLength = 50000000 }.Serialize(lstPostalAddress);
                try
                {
                    List<string> categoryNames = new List<string> { PostalAddressConstants.PostalAddressType, PostalAddressConstants.PostalAddressStatus };

                    var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;

                    Guid addressTypeUSR = referenceDataCategoryList
                  .Where(x => x.CategoryName == PostalAddressConstants.PostalAddressType)
                  .SelectMany(x => x.ReferenceDatas)
                  .Where(x => x.ReferenceDataValue.Equals(FileType.Usr.ToString(), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID)
                  .SingleOrDefault();

                    Guid addressTypePAF = referenceDataCategoryList
                  .Where(x => x.CategoryName == PostalAddressConstants.PostalAddressType)
                  .SelectMany(x => x.ReferenceDatas)
                  .Where(x => x.ReferenceDataValue.Equals(FileType.Paf.ToString(), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID)
                  .SingleOrDefault();

                    Guid addressTypeNYB = referenceDataCategoryList
                  .Where(x => x.CategoryName == PostalAddressConstants.PostalAddressType)
                  .SelectMany(x => x.ReferenceDatas)
                  .Where(x => x.ReferenceDataValue.Equals(FileType.Nyb.ToString(), StringComparison.OrdinalIgnoreCase)).Select(x => x.ID)
                  .SingleOrDefault();

                    Guid pendingDelete = referenceDataCategoryList
                  .Where(x => x.CategoryName == PostalAddressConstants.PostalAddressStatus)
                  .SelectMany(x => x.ReferenceDatas)
                  .Where(x => x.ReferenceDataValue == PostalAddressConstants.PendingDeleteInFMO).Select(x => x.ID)
                  .SingleOrDefault();

                    foreach (var item in lstPostalAddress)
                    {
                        loggingHelper.Log("record no " + lstPostalAddress.IndexOf(item) + " , Udprn :" + item.UDPRN, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                        if (!string.IsNullOrEmpty(item.AmendmentType))
                        {
                            AmmendmentType ammendmentType = (AmmendmentType)Enum.Parse(typeof(AmmendmentType), item.AmendmentType.ToUpper());

                            switch (ammendmentType)
                            {
                                case AmmendmentType.C:
                                    await ModifyPAFRecords(item, addressTypePAF, item.FileName);
                                    break;

                                case AmmendmentType.I:
                                    await SavePAFRecords(item, addressTypeUSR, addressTypeNYB, addressTypePAF, item.FileName);
                                    break;

                                case AmmendmentType.D:
                                    await ModifyAddressOnUdprn(item.UDPRN.Value, addressTypePAF, pendingDelete);
                                    break;
                            }
                        }
                    }

                    isPostalAddressProcessed = true;

                    loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex.ToString(), TraceEventType.Error, ex);
                    this.loggingHelper.Log(postalAddressList, TraceEventType.Information);
                    throw;
                }

                return isPostalAddressProcessed;
            }
        }

        /// <summary>
        /// Method implementation to save delivery point and Task for notification for PAF create events
        /// </summary>
        /// <param name="objPostalAddress">pass PostalAddreesDTO</param>
        public async Task SaveDeliveryPointProcess(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SaveDeliveryPointProcess"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(SaveDeliveryPointProcess);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                // Call postalAddressIntegrationService to get reference data
                List<string> categoryNamesSimpleLists = new List<string>
                    {
                       ReferenceDataCategoryNames.TASKNOTIFICATION,
                       ReferenceDataCategoryNames.NETWORKLINKDATAPROVIDER,
                       ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                        ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                        ReferenceDataCategoryNames.NetworkNodeType
                    };

                Guid tasktypeId = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.TASKNOTIFICATION, PostalAddressConstants.TASKACTION);
                Guid locationProviderId = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NETWORKLINKDATAPROVIDER, PostalAddressConstants.EXTERNAL);
                Guid deliveryPointUseIndicator = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointUseIndicator, PostalAddressConstants.DeliveryPointUseIndicatorPAF, true);
                Guid operationalStatusGUIDLive = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, PostalAddressConstants.OperationalStatusGUIDLive, true);
                Guid operationalStatusGUIDLivePending = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, PostalAddressConstants.OperationalStatusGUIDLivePending, true);
                Guid networkNodeTypeRMGServiceNode = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NetworkNodeType, PostalAddressConstants.NetworkNodeTypeRMGServiceNode, true);

                // Search Address Location for Postal Address If found, Add delivery point as per Address
                // Location details Else, Add delivery point w/o Address location details and also add
                // task in notification
                var objAddressLocation = postalAddressIntegrationService.GetAddressLocationByUDPRN(objPostalAddress.UDPRN ?? default(int)).Result;

                string postCodeDistrict = objPostalAddress.Postcode.Substring(0, objPostalAddress.Postcode.Length - 4);

                if (objAddressLocation == null)
                {
                    // deriving an approximate Location for the Delivery Point
                    DbGeometry approxLocation = await postalAddressIntegrationService.GetApproxLocation(objPostalAddress.Postcode);

                    var newDeliveryPoint = new DeliveryPointDTO
                    {
                        ID = Guid.NewGuid(),
                        Address_GUID = objPostalAddress.ID,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,
                        LocationXY = approxLocation,
                        OperationalStatus_GUID = operationalStatusGUIDLivePending,
                        NetworkNodeType_GUID = networkNodeTypeRMGServiceNode
                    };
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);

                    // Create task
                    var objTask = new NotificationDTO
                    {
                        ID = Guid.NewGuid(),
                        NotificationType_GUID = tasktypeId,
                        NotificationPriority_GUID = null,
                        NotificationSource = PostalAddressConstants.TASKSOURCE,
                        Notification_Heading = PostalAddressConstants.TASKPAFACTION,
                        Notification_Message = AddressFields(objPostalAddress),
                        PostcodeDistrict = postCodeDistrict,
                        NotificationDueDate = DateTime.UtcNow.AddHours(PostalAddressConstants.NOTIFICATIONDUE),
                        NotificationActionLink = string.Format(PostalAddressConstants.PAFNOTIFICATIONLINK, objPostalAddress.UDPRN)
                    };

                    // Call Notification service
                    await postalAddressIntegrationService.AddNewNotification(objTask);
                }
                else
                {
                    var newDeliveryPoint = new DeliveryPointDTO
                    {
                        ID = Guid.NewGuid(),
                        Address_GUID = objPostalAddress.ID,
                        LocationXY = objAddressLocation.LocationXY,
                        Latitude = objAddressLocation.Lattitude,
                        Longitude = objAddressLocation.Longitude,
                        LocationProvider_GUID = locationProviderId,
                        DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator,

                        OperationalStatus_GUID = operationalStatusGUIDLive,
                        NetworkNodeType_GUID = networkNodeTypeRMGServiceNode
                    };
                    loggingHelper.Log(PostalAddressConstants.OperationalStatusGUIDLive.ToString(), TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.SaveDeliveryPointProcessPriority, LoggerTraceConstants.SaveDeliveryPointProcessBusinessMethodExitEventId, LoggerTraceConstants.Title);
                    await postalAddressIntegrationService.InsertDeliveryPoint(newDeliveryPoint);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Get Postal Address based on postal address id.
        /// </summary>
        /// <param name="postalAddressId">PostalAddress Unique Identifier</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDTO GetPostalAddressDetails(Guid postalAddressId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddressDetails"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPostalAddressDetails);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var postalAddressDBDTO = addressDataService.GetPostalAddressDetails(postalAddressId);

                var postalAddress = ConvertToDTO(postalAddressDBDTO);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.GetPostalAddressDetailsByIdPriority, LoggerTraceConstants.GetPostalAddressDetailsByIdBusinessMethodExitEventId);
                return postalAddress;
            }
        }

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO as input</param>
        /// <returns>string</returns>
        public async Task<string> CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CheckForDuplicateNybRecords"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CheckForDuplicateNybRecords);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var referenceDataCategoryList = await postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressConstants.PostalAddressType);
                Guid addressTypeNYB = GetReferenceData(PostalAddressConstants.PostalAddressType, FileType.Nyb.ToString());

                string postCode = string.Empty;
                var objDataPostalAddress = ConvertToDataDTO(objPostalAddress);
                postCode = addressDataService.CheckForDuplicateNybRecords(objDataPostalAddress, addressTypeNYB);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postCode;
            }
        }

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        public async Task<bool> CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CheckForDuplicateAddressWithDeliveryPoints"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CheckForDuplicateAddressWithDeliveryPoints);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                bool isDuplicate = await addressDataService.CheckForDuplicateAddressWithDeliveryPoints(ConvertToDataDTO(objPostalAddress));
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return isDuplicate;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public CreateDeliveryPointModelDTO CreateAddressForDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CreateAddressForDeliveryPoint"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CreateAddressForDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                bool isAddressLocationAvailable = false;
                double? addLocationXCoOrdinate = 0;
                double? addLocationYCoOrdinate = 0;
                List<string> listNames = new List<string> { ReferenceDataCategoryNames.PostalAddressType, ReferenceDataCategoryNames.PostalAddressStatus, ReferenceDataCategoryNames.PostalAddressAliasType };

                Guid usrAddressTypeId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressType, FileType.Usr.ToString().ToUpper(), true);
                Guid liveAddressStatusId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressStatus, PostalAddressConstants.LiveAddressStatus, true);
                Guid deliveryPointAliasId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressAliasType, PostalAddressConstants.DeliveryPointAlias, false);

                if (addDeliveryPointDTO != null && addDeliveryPointDTO.PostalAddressDTO != null)
                {
                    if (addDeliveryPointDTO.PostalAddressDTO.ID == Guid.Empty)
                    {
                        addDeliveryPointDTO.PostalAddressDTO.ID = Guid.NewGuid();
                    }

                    addDeliveryPointDTO.PostalAddressDTO.AddressType_GUID = usrAddressTypeId;

                    addDeliveryPointDTO.PostalAddressDTO.AddressStatus_GUID = liveAddressStatusId;

                    addDeliveryPointDTO.PostalAddressDTO.PostalAddressAlias = addDeliveryPointDTO.PostalAddressAliasDTOs;

                    if (addDeliveryPointDTO.PostalAddressDTO.PostalAddressAlias != null)
                    {
                        if (addDeliveryPointDTO.PostalAddressDTO.PostalAddressAlias.Count == 1)
                        {
                            addDeliveryPointDTO.PostalAddressDTO.PostalAddressAlias.First().PreferenceOrderIndex = 1;
                        }

                        addDeliveryPointDTO.PostalAddressDTO.PostalAddressAlias.ForEach(p => p.AliasTypeGUID = deliveryPointAliasId);
                    }
                }

                var postalAddressId = addressDataService.CreateAddressForDeliveryPoint(ConvertToDataDTO(addDeliveryPointDTO.PostalAddressDTO));

                // check if third partylocation exists
                var addressLocation = postalAddressIntegrationService.GetAddressLocationByUDPRN(addDeliveryPointDTO.PostalAddressDTO.UDPRN ?? default(int)).Result;

                if (addressLocation == null)
                {
                    isAddressLocationAvailable = false;
                }
                else
                {
                    isAddressLocationAvailable = true;
                    addLocationXCoOrdinate = Convert.ToDouble(addressLocation.Lattitude);
                    addLocationYCoOrdinate = Convert.ToDouble(addressLocation.Longitude);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);

                return new CreateDeliveryPointModelDTO { ID = postalAddressId, IsAddressLocationAvailable = isAddressLocationAvailable, XCoordinate = addLocationXCoOrdinate, YCoordinate = addLocationYCoOrdinate };
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public async Task<List<CreateDeliveryPointModelDTO>> CreateAddressForDeliveryPointForRange(List<PostalAddressDTO> postalAddressDTOs)
        {
            if (postalAddressDTOs == null)
            {
                throw new ArgumentNullException(nameof(postalAddressDTOs));
            }

            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CreateAddressForDeliveryPointForRange"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CreateAddressForDeliveryPointForRange);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                List<CreateDeliveryPointModelDTO> createDeliveryPointModelDTOs = null;
                List<string> listNames = new List<string> { ReferenceDataCategoryNames.PostalAddressType, ReferenceDataCategoryNames.PostalAddressStatus };

                Guid usrAddressTypeId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressType, FileType.Usr.ToString().ToUpper(), true);
                Guid liveAddressStatusId = GetReferenceData(listNames, ReferenceDataCategoryNames.PostalAddressStatus, PostalAddressConstants.LiveAddressStatus, true);
                Mapper.Initialize(cfg => cfg.CreateMap<PostalAddressDTO, PostalAddressDataDTO>());
                List<PostalAddressDataDTO> postalAddressDataDTOs = Mapper.Map<List<PostalAddressDTO>, List<PostalAddressDataDTO>>(postalAddressDTOs);

                List<int> udprns = null;

                if (postalAddressDataDTOs != null && postalAddressDataDTOs.Count > 0)
                {
                    udprns = new List<int>();

                    postalAddressDataDTOs.ForEach(pa =>
                    {
                        pa.AddressType_GUID = usrAddressTypeId;
                        pa.PostalAddressStatus.Add(GetPostalAddressStatus(pa.ID, liveAddressStatusId));

                        if (pa.UDPRN != null)
                            udprns.Add((int)pa.UDPRN);
                    });

                    createDeliveryPointModelDTOs = new List<CreateDeliveryPointModelDTO>();

                    var addressLocations = (udprns != null && udprns.Count > 0) ? await postalAddressIntegrationService.GetAddressLocationsByUDPRN(udprns) : null;

                    postalAddressDataDTOs.ForEach(pa =>
                    {
                        AddressLocationDTO addressLocationDTO = (addressLocations != null && addressLocations.Count > 0) ? addressLocations.Where(al => al.UDPRN == pa.UDPRN).FirstOrDefault() : null;
                        SqlGeometry deliveryPointSqlGeometry = null;

                        if (addressLocationDTO != null)
                            deliveryPointSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(addressLocationDTO.LocationXY.AsBinary()), PostalAddressConstants.BNGCOORDINATESYSTEM);

                        var postalAddressId = addressDataService.CreateAddressForDeliveryPoint(pa);

                        createDeliveryPointModelDTOs.Add(new CreateDeliveryPointModelDTO
                        {
                            ID = postalAddressId,
                            IsAddressLocationAvailable = (addressLocations != null && addressLocations.Count > 0) ? addressLocations.Where(al => al.UDPRN == pa.UDPRN).Any() : false,
                            XCoordinate = deliveryPointSqlGeometry != null ? deliveryPointSqlGeometry.STX.Value : 0,
                            YCoordinate = deliveryPointSqlGeometry != null ? deliveryPointSqlGeometry.STY.Value : 0
                        });
                    });
                }

                return createDeliveryPointModelDTOs;
            }
        }

        /// <summary>
        /// Get Postal Address for the List of Guids
        /// </summary>
        /// <param name="addressGuids">List of Address Guids</param>
        /// <returns></returns>
        public async Task<List<PostalAddressDTO>> GetPostalAddresses(List<Guid> addressGuids)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddresses"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPostalAddresses);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var addressDetails = await addressDataService.GetPostalAddresses(addressGuids);

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<PostalAddressDataDTO, PostalAddressDTO>();
                });
                Mapper.Configuration.CreateMapper();

                var listAddressDetails = Mapper.Map<List<PostalAddressDataDTO>, List<PostalAddressDTO>>(addressDetails);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return listAddressDetails;
            }
        }

        /// <summary>
        /// Get PAF Addresses on UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN number</param>
        /// <returns>Postal Address DTO</returns>
        public async Task<PostalAddressDTO> GetPAFAddress(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddresses"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPAFAddress);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                Guid addressTypePAF = GetReferenceData(PostalAddressConstants.PostalAddressType, FileType.Paf.ToString());

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return await addressDataService.GetPAFAddress(udprn, addressTypePAF);
            }
        }

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO as input</param>
        /// <returns>string</returns>
        public async Task<DuplicateDeliveryPointDTO> CheckForDuplicateNybRecordsForRange(List<PostalAddressDTO> postalAddresses)
        {
            if (postalAddresses == null)
            {
                throw new ArgumentNullException(nameof(postalAddresses));
            }

            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CheckForDuplicateNybRecords"))
            {
                DuplicateDeliveryPointDTO duplicateDeliveryPointDTO = new DuplicateDeliveryPointDTO();
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CheckForDuplicateNybRecords);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressConstants.PostalAddressType).Result;
                Guid addressTypeNYB = GetReferenceData(PostalAddressConstants.PostalAddressType, FileType.Nyb.ToString());

                string postCode = string.Empty;

                List<PostalAddressDataDTO> postalAddressDataDTOs = ConvertToDataDTO(postalAddresses);

                var duplicatePostalAddresses = await addressDataService.CheckForDuplicateNybRecordsForRange(postalAddressDataDTOs, addressTypeNYB);

                duplicateDeliveryPointDTO.PostalAddressDTO = ConvertToDTO(duplicatePostalAddresses.Item2);
                duplicateDeliveryPointDTO.IsDuplicate = duplicatePostalAddresses.Item1;

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return duplicateDeliveryPointDTO;
            }
        }

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        public async Task<DuplicateDeliveryPointDTO> CheckForDuplicateAddressWithDeliveryPointsForRange(List<PostalAddressDTO> postalAddressDTOs)
        {
            if (postalAddressDTOs == null)
            {
                throw new ArgumentNullException(nameof(postalAddressDTOs));
            }

            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.CheckForDuplicateNybRecords"))
            {
                DuplicateDeliveryPointDTO duplicateDeliveryPointDTO = new DuplicateDeliveryPointDTO();
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(CheckForDuplicateNybRecords);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                List<PostalAddressDataDTO> postalAddressDataDTOs = ConvertToDataDTO(postalAddressDTOs);

                var duplicatePostalAddresses = await addressDataService.CheckForDuplicateAddressWithDeliveryPointsForRange(postalAddressDataDTOs);

                duplicateDeliveryPointDTO.PostalAddressDTO = ConvertToDTO(duplicatePostalAddresses.Item2);
                duplicateDeliveryPointDTO.IsDuplicate = duplicatePostalAddresses.Item1;

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return duplicateDeliveryPointDTO;
            }
        }


        /// <summary>
        /// Delete Postal Addresses as part of Housekeeping
        /// </summary>
        /// <returns>Void</returns>
        public async Task DeletePostalAddressesForHouseKeeping()
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.DeletePostalAddressesForHouseKeeping"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(DeletePostalAddressesForHouseKeeping);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                List<string> categoryNames = new List<string> { PostalAddressConstants.PostalAddressType, PostalAddressConstants.PostalAddressStatus };

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryNames).Result;

                Guid pendingDelete = referenceDataCategoryList
                  .Where(x => x.CategoryName == PostalAddressConstants.PostalAddressStatus)
                  .SelectMany(x => x.ReferenceDatas)
                  .Where(x => x.ReferenceDataValue == PostalAddressConstants.PendingDeleteInFMO).Select(x => x.ID)
                  .SingleOrDefault();

                List<PostalAddressDataDTO> postalAddresses = await addressDataService.GetAllPendingDeletePostalAddresses(pendingDelete);

                if (postalAddresses != null && postalAddresses.Count > 0)
                {
#if DEBUG
                    loggingHelper.Log("Count of Postal Addresses: " + postalAddresses.Count, TraceEventType.Information);
                    await addressDataService.DeletePostalAddressForHousekeeping(postalAddresses);
#else
                    await addressDataService.DeletePostalAddressForHousekeeping(postalAddresses);
#endif
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// Business rule implementation for PAF create events
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO to process</param>
        /// <param name="addressTypeUSR">addressType Guid for USR</param>
        /// <param name="addressTypeNYB">addressType Guid for NYB</param>
        /// <param name="addressTypePAF">addressType Guid for PAF</param>
        /// <param name="strFileName">FileName on PAF events to track against DB</param>
        private async Task SavePAFRecords(PostalAddressDTO objPostalAddressBatch, Guid addressTypeUSR, Guid addressTypeNYB, Guid addressTypePAF, string strFileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.SavePAFRecords"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(SavePAFRecords);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                Guid deliveryPointUseIndicatorPAF = Guid.Empty;
                Guid postCodeGuid = Guid.Empty;

                // Construct New PostalAddressDTO
                PostalAddressDataDTO objPostalAddress = new PostalAddressDataDTO
                {
                    Postcode = objPostalAddressBatch.Postcode,
                    PostTown = objPostalAddressBatch.PostTown,
                    DependentLocality = objPostalAddressBatch.DependentLocality,
                    DoubleDependentLocality = objPostalAddressBatch.DoubleDependentLocality,
                    Thoroughfare = objPostalAddressBatch.Thoroughfare,
                    DependentThoroughfare = objPostalAddressBatch.DependentThoroughfare,
                    BuildingNumber = objPostalAddressBatch.BuildingNumber,
                    BuildingName = objPostalAddressBatch.BuildingName,
                    SubBuildingName = objPostalAddressBatch.SubBuildingName,
                    POBoxNumber = objPostalAddressBatch.POBoxNumber,
                    DepartmentName = objPostalAddressBatch.DepartmentName,
                    OrganisationName = objPostalAddressBatch.OrganisationName,
                    UDPRN = objPostalAddressBatch.UDPRN,
                    PostcodeType = objPostalAddressBatch.PostcodeType,
                    SmallUserOrganisationIndicator = objPostalAddressBatch.SmallUserOrganisationIndicator,
                    DeliveryPointSuffix = objPostalAddressBatch.DeliveryPointSuffix,
                    RowCreateDateTime = DateTime.UtcNow
                };

                // Address type will be PAF only in case of Inserting and updating records
                objPostalAddress.AddressType_GUID = addressTypePAF;
                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(PostalAddressConstants.PostalAddressStatus).Result;
                var addressStatus_GUID = GetReferenceData(PostalAddressConstants.PostalAddressStatus, PostCodeStatus.Live.ToString());

                // match if PostalAddress exists on UDPRN match
                PostalAddressDataDTO objPostalAddressMatchedUDPRN = await addressDataService.GetPostalAddress(objPostalAddress.UDPRN);

                // match if PostalAddress exists on Address match
                var objPostalAddressMatchedAddress = await addressDataService.GetPostalAddress(objPostalAddress);

                // PAF process Logic
                if (objPostalAddressMatchedUDPRN != null)
                {
                    if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypeNYB)
                    {
                        objPostalAddress.ID = objPostalAddressMatchedUDPRN.ID;
                        objPostalAddress.PostalAddressStatus = objPostalAddressMatchedUDPRN.PostalAddressStatus;

                        if (await addressDataService.UpdateAddress(objPostalAddress, strFileName))
                        {
                            // calling delivery point web api
                            var objDeliveryPoint = await postalAddressIntegrationService.GetDeliveryPointByPostalAddress(objPostalAddress.ID);
                            if (objDeliveryPoint == null)
                            {
                                await SaveDeliveryPointProcess(objPostalAddress);
                            }
                        }
                    }
                    else
                    {
                        loggingHelper.Log(string.Format(PostalAddressConstants.PAFERRORLOGMESSAGE, PostalAddressConstants.PAFErrorMessageForAddressTypeNYBNotFound, objPostalAddress.UDPRN, objPostalAddressBatch.AmendmentType, strFileName, FileType.Paf, DateTime.UtcNow), TraceEventType.Error);
                    }
                }
                else if (objPostalAddressMatchedAddress != null)
                {
                    if (objPostalAddressMatchedAddress.AddressType_GUID == addressTypeUSR)
                    {
                        objPostalAddress.ID = objPostalAddressMatchedAddress.ID;
                        objPostalAddress.PostalAddressStatus = objPostalAddressMatchedAddress.PostalAddressStatus;

                        // Update Postal address
                        if (await addressDataService.UpdateAddress(objPostalAddress, strFileName))
                        {
                            // Update delivery point for USR records
                            await UpdateDeliveryPointProcess(objPostalAddress);
                        }
                    }
                    else
                    {
                        loggingHelper.Log(string.Format(PostalAddressConstants.PAFERRORLOGMESSAGE, PostalAddressConstants.PAFErrorMessageForAddressTypeUSRNotFound, objPostalAddress.UDPRN, objPostalAddressBatch.AmendmentType, strFileName,
                                            FileType.Paf, DateTime.UtcNow), TraceEventType.Error);
                    }
                }
                else
                {
                    objPostalAddress.ID = Guid.NewGuid();
                    objPostalAddress.PostalAddressStatus.Add(GetPostalAddressStatus(objPostalAddress.ID, addressStatus_GUID));
                    await addressDataService.InsertAddress(objPostalAddress, strFileName);
                    await SaveDeliveryPointProcess(objPostalAddress);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Updating Delivery Point to the database in case of USR records
        /// </summary>
        /// <param name="objPostalAddress">Postal Address DTO</param>
        /// <returns>nothing</returns>
        private async Task UpdateDeliveryPointProcess(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.UpdateDeliveryPointProcess"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(UpdateDeliveryPointProcess);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                List<string> categoryNamesSimpleLists = new List<string>
                    {
                       ReferenceDataCategoryNames.TASKNOTIFICATION,
                       ReferenceDataCategoryNames.NETWORKLINKDATAPROVIDER,
                       ReferenceDataCategoryNames.DeliveryPointUseIndicator,
                        ReferenceDataCategoryNames.DeliveryPointOperationalStatus,
                        ReferenceDataCategoryNames.NetworkNodeType
                    };

                string postCodeDistrict = objPostalAddress.Postcode.Substring(0, objPostalAddress.Postcode.Length - 4);
                Guid tasktypeId = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.TASKNOTIFICATION, PostalAddressConstants.TASKACTION);
                Guid locationProviderId = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NETWORKLINKDATAPROVIDER, PostalAddressConstants.EXTERNAL);
                Guid deliveryPointUseIndicator = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointUseIndicator, PostalAddressConstants.DeliveryPointUseIndicatorPAF, true);
                Guid operationalStatusGUIDLive = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, ReferenceDataCategoryNames.NetworkNodeType, true);
                Guid operationalStatusGUIDLivePending = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.DeliveryPointOperationalStatus, PostalAddressConstants.OperationalStatusGUIDLivePending, true);
                Guid networkNodeTypeRMGServiceNode = GetReferenceData(categoryNamesSimpleLists, ReferenceDataCategoryNames.NetworkNodeType, PostalAddressConstants.NetworkNodeTypeRMGServiceNode, true);

                var objDeliveryPoint = await postalAddressIntegrationService.GetDeliveryPointByPostalAddress(objPostalAddress.ID);

                if (objDeliveryPoint != null && objDeliveryPoint.OperationalStatus_GUID == operationalStatusGUIDLivePending)
                {
                    AddressLocationDTO addressLocationDTO = await postalAddressIntegrationService.GetAddressLocationByUDPRN((int)objPostalAddress.UDPRN);

                    if (addressLocationDTO != null)
                    {
                        // DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO
                        // {
                        // ID = Guid.NewGuid(),
                        objDeliveryPoint.Address_GUID = objPostalAddress.ID;
                        objDeliveryPoint.LocationXY = addressLocationDTO.LocationXY;
                        objDeliveryPoint.LocationProvider_GUID = locationProviderId;
                        objDeliveryPoint.DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator;
                        objDeliveryPoint.OperationalStatus_GUID = operationalStatusGUIDLive;

                        objDeliveryPoint.NetworkNodeType_GUID = networkNodeTypeRMGServiceNode;

                        // };
                        if (await postalAddressIntegrationService.CheckIfNotificationExists((int)objPostalAddress.UDPRN, PostalAddressConstants.TASKPAFACTION))
                        {
                            await postalAddressIntegrationService.UpdateDeliveryPoint(objDeliveryPoint);

                            // update the notification if it exists.
                            await postalAddressIntegrationService.UpdateNotificationByUDPRN((int)objPostalAddress.UDPRN, PostalAddressConstants.TASKPAFACTION, PostalAddressConstants.NOTIFICATIONCLOSED);
                        }
                    }
                    else
                    {
                        if (await postalAddressIntegrationService.CheckIfNotificationExists((int)objPostalAddress.UDPRN, PostalAddressConstants.TASKPAFACTION))
                        {
                            string message = AddressFields(objPostalAddress);
                            await postalAddressIntegrationService.UpdateNotificationMessageByUDPRN((int)objPostalAddress.UDPRN, PostalAddressConstants.TASKPAFACTION, message);
                        }
                    }
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">PAF create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressFields(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.AddressFields"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(AddressFields);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                string pafMessage = PostalAddressConstants.PAFTaskBodyPreText +
                        objPostalAddress.OrganisationName + PostalAddressConstants.Comma +
                        objPostalAddress.DepartmentName + PostalAddressConstants.Comma +
                        objPostalAddress.BuildingName + PostalAddressConstants.Comma +
                        objPostalAddress.BuildingNumber + PostalAddressConstants.Comma +
                        objPostalAddress.SubBuildingName + PostalAddressConstants.Comma +
                        objPostalAddress.Thoroughfare + PostalAddressConstants.Comma +
                        objPostalAddress.DependentThoroughfare + PostalAddressConstants.Comma +
                        objPostalAddress.DependentLocality + PostalAddressConstants.Comma +
                        objPostalAddress.DoubleDependentLocality + PostalAddressConstants.Comma +
                        objPostalAddress.PostTown + PostalAddressConstants.Comma +
                        objPostalAddress.Postcode;

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return pafMessage;
            }
        }

        /// <summary>
        /// Construct Postal Address Status for resp Postal Address
        /// </summary>
        /// <param name="objPostalAddress">PAF create event PostalAddressDTO</param>
        /// <returns>returns postal address DTO</returns>
        private PostalAddressStatusDataDTO GetPostalAddressStatus(Guid postalAddressGUID, Guid operationalStatusGUID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetPostalAddressStatus"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetPostalAddressStatus);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                PostalAddressStatusDataDTO postalAddressStatusDataDTO = new PostalAddressStatusDataDTO
                {
                    ID = Guid.NewGuid(),
                    PostalAddressGUID = postalAddressGUID,
                    OperationalStatusGUID = operationalStatusGUID,
                    StartDateTime = DateTime.UtcNow,
                    RowCreateDateTime = DateTime.UtcNow
                };

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postalAddressStatusDataDTO;
            }
        }

        /// <summary>
        /// Get the Reference data for the categorynames based on the referencedata value
        /// </summary>
        /// <param name="categoryNamesSimpleLists">list of category names</param>
        /// <param name="categoryName">category name</param>
        /// <param name="referenceDataValue">reference data value</param>
        /// <param name="isWithSpace"> whether </param>
        /// <returns></returns>
        private Guid GetReferenceData(List<string> categoryNamesSimpleLists, string categoryName, string referenceDataValue, bool isWithSpace = false)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetReferenceData);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryNamesSimpleLists).Result;
                Guid referenceDataGuid = Guid.Empty;
                if (isWithSpace)
                {
                    referenceDataGuid = referenceDataCategoryList
                                       .Where(list => list.CategoryName.Replace(" ", string.Empty) == categoryName)
                                       .SelectMany(list => list.ReferenceDatas)
                                       .Where(item => item.ReferenceDataValue.Equals(referenceDataValue, StringComparison.OrdinalIgnoreCase))
                                       .Select(s => s.ID).SingleOrDefault();
                }
                else
                {
                    referenceDataGuid = referenceDataCategoryList
                                    .Where(list => list.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                                    .SelectMany(list => list.ReferenceDatas)
                                    .Where(item => item.ReferenceDataValue.Equals(referenceDataValue, StringComparison.OrdinalIgnoreCase))
                                    .Select(s => s.ID).SingleOrDefault();
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return referenceDataGuid;
            }
        }

        private Guid GetReferenceData(string categoryName, string referenceDataValue)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(GetReferenceData);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var referenceDataCategoryList = postalAddressIntegrationService.GetReferenceDataSimpleLists(categoryName).Result;
                Guid referenceDataGuid = referenceDataCategoryList.ReferenceDatas
                    .Where(a => a.ReferenceDataValue.Equals(referenceDataValue, StringComparison.OrdinalIgnoreCase))
                    .Select(a => a.ID).FirstOrDefault();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return referenceDataGuid;
            }
        }

        /// <summary>
        /// Method to convert DTO to DataDTO of PostalAddress
        /// </summary>
        /// <param name="postalAddressDTO">PostalAddress DTO Object</param>
        /// <returns>returns converted PostalAddressDataDTO object</returns>
        private PostalAddressDataDTO ConvertToDataDTO(PostalAddressDTO postalAddressDTO)
        {
            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO();
            if (postalAddressDTO != null)
            {
                postalAddressDataDTO.ID = postalAddressDTO.ID;

                // postalAddressDataDTO.AddressStatus_GUID = postalAddressDTO.AddressStatus_GUID;
                postalAddressDataDTO.AddressType_GUID = postalAddressDTO.AddressType_GUID;
                postalAddressDataDTO.AMUApproved = postalAddressDTO.AMUApproved;
                postalAddressDataDTO.BuildingName = postalAddressDTO.BuildingName;
                postalAddressDataDTO.BuildingNumber = postalAddressDTO.BuildingNumber;
                postalAddressDataDTO.DeliveryPointSuffix = postalAddressDTO.DeliveryPointSuffix;
                postalAddressDataDTO.DepartmentName = postalAddressDTO.DepartmentName;
                postalAddressDataDTO.DependentLocality = postalAddressDTO.DependentLocality;
                postalAddressDataDTO.DependentThoroughfare = postalAddressDTO.DependentThoroughfare;
                postalAddressDataDTO.DoubleDependentLocality = postalAddressDTO.DoubleDependentLocality;
                postalAddressDataDTO.OrganisationName = postalAddressDTO.OrganisationName;
                postalAddressDataDTO.POBoxNumber = postalAddressDTO.POBoxNumber;
                postalAddressDataDTO.Postcode = postalAddressDTO.Postcode;

                // postalAddressDataDTO.pos = postalAddressDTO.PostCodeGUID;
                postalAddressDataDTO.PostcodeType = postalAddressDTO.PostcodeType;
                postalAddressDataDTO.PostTown = postalAddressDTO.PostTown;
                postalAddressDataDTO.SmallUserOrganisationIndicator = postalAddressDTO.SmallUserOrganisationIndicator;
                postalAddressDataDTO.SubBuildingName = postalAddressDTO.SubBuildingName;
                postalAddressDataDTO.Thoroughfare = postalAddressDTO.Thoroughfare;
                postalAddressDataDTO.UDPRN = postalAddressDTO.UDPRN;

                PostalAddressStatusDataDTO postalAddressStatusDataDTO = new PostalAddressStatusDataDTO();
                postalAddressStatusDataDTO.ID = Guid.NewGuid();
                postalAddressStatusDataDTO.PostalAddressGUID = postalAddressDTO.ID;
                postalAddressStatusDataDTO.OperationalStatusGUID = postalAddressDTO.AddressStatus_GUID.HasValue ? postalAddressDTO.AddressStatus_GUID.Value : Guid.Empty;
                postalAddressDataDTO.PostalAddressStatus.Add(postalAddressStatusDataDTO);

                if (postalAddressDTO.PostalAddressAlias != null)
                {
                    List<PostalAddressAliasDataDTO> postalAddressAliases = new List<PostalAddressAliasDataDTO>();
                    foreach (var postalAddressAliasDTO in postalAddressDTO.PostalAddressAlias)
                    {
                        PostalAddressAliasDataDTO postalAddressAlias = new PostalAddressAliasDataDTO
                        {
                            ID = Guid.NewGuid(),
                            PostalAddressID = postalAddressDTO.ID,
                            AliasTypeGUID = postalAddressAliasDTO.AliasTypeGUID,
                            AliasName = postalAddressAliasDTO.AliasName,
                            PreferenceOrderIndex = postalAddressAliasDTO.PreferenceOrderIndex
                        };
                        postalAddressAliases.Add(postalAddressAlias);
                    }

                    postalAddressDataDTO.PostalAddressAlias = postalAddressAliases;
                }
            }

            return postalAddressDataDTO;
        }

        /// <summary>
        /// Method to convert List od DTO to List of DataDTO of PostalAddress
        /// </summary>
        /// <param name="postalAddressDTOList">List of PostalAddress DTO Object</param>
        /// <returns>returns converted List of PostalAddressDataDTO object</returns>
        private List<PostalAddressDataDTO> ConvertToDataDTO(List<PostalAddressDTO> postalAddressDTOList)
        {
            List<PostalAddressDataDTO> postalAddressDataDTO = new List<PostalAddressDataDTO>();

            foreach (var postalAddressDTO in postalAddressDTOList)
            {
                postalAddressDataDTO.Add(ConvertToDataDTO(postalAddressDTO));
            }

            return postalAddressDataDTO;
        }

        /// <summary>
        /// Method to convert DataDTO to DTO of PostalAddress
        /// </summary>
        /// <param name="postalAddressDataDTO">postalAddressDataDTO Object</param>
        /// <returns>returns converted PostalAddressDTO object</returns>
        private PostalAddressDTO ConvertToDTO(PostalAddressDataDTO postalAddressDataDTO)
        {
            PostalAddressDTO postalAddressDTO = new PostalAddressDTO();

            if (postalAddressDataDTO != null)
            {
                postalAddressDTO.ID = postalAddressDataDTO.ID;

                // postalAddressDataDTO.AddressStatus_GUID = postalAddressDTO.AddressStatus_GUID;
                postalAddressDTO.AddressType_GUID = postalAddressDataDTO.AddressType_GUID;
                postalAddressDTO.AMUApproved = postalAddressDataDTO.AMUApproved;
                postalAddressDTO.BuildingName = postalAddressDataDTO.BuildingName;
                postalAddressDTO.BuildingNumber = postalAddressDataDTO.BuildingNumber;
                postalAddressDTO.DeliveryPointSuffix = postalAddressDataDTO.DeliveryPointSuffix;
                postalAddressDTO.DepartmentName = postalAddressDataDTO.DepartmentName;
                postalAddressDTO.DependentLocality = postalAddressDataDTO.DependentLocality;
                postalAddressDTO.DependentThoroughfare = postalAddressDataDTO.DependentThoroughfare;
                postalAddressDTO.DoubleDependentLocality = postalAddressDataDTO.DoubleDependentLocality;
                postalAddressDTO.OrganisationName = postalAddressDataDTO.OrganisationName;
                postalAddressDTO.POBoxNumber = postalAddressDataDTO.POBoxNumber;
                postalAddressDTO.Postcode = postalAddressDataDTO.Postcode;

                // postalAddressDataDTO.pos = postalAddressDTO.PostCodeGUID;
                postalAddressDTO.PostcodeType = postalAddressDataDTO.PostcodeType;
                postalAddressDTO.PostTown = postalAddressDataDTO.PostTown;
                postalAddressDTO.SmallUserOrganisationIndicator = postalAddressDataDTO.SmallUserOrganisationIndicator;
                postalAddressDTO.SubBuildingName = postalAddressDataDTO.SubBuildingName;
                postalAddressDTO.Thoroughfare = postalAddressDataDTO.Thoroughfare;
                postalAddressDTO.UDPRN = postalAddressDataDTO.UDPRN;

                if (postalAddressDataDTO.PostalAddressStatus != null && postalAddressDataDTO.PostalAddressStatus.Count > 0)
                {
                    postalAddressDTO.AddressStatus_GUID = postalAddressDataDTO.PostalAddressStatus.First().OperationalStatusGUID;
                }
            }

            return postalAddressDTO;
        }

        /// <summary>
        /// Method to convert List of DataDTO to List of DTO for PostalAddress
        /// </summary>
        /// <param name="postalAddressDataDTOList">List of postalAddressDataDTO Object</param>
        /// <returns>returns converted List of PostalAddressDTO object</returns>
        private List<PostalAddressDTO> ConvertToDTO(List<PostalAddressDataDTO> postalAddressDataDTOList)
        {
            List<PostalAddressDTO> postalAddressDTO = new List<PostalAddressDTO>();

            foreach (var postalAddressDataDTO in postalAddressDataDTOList)
            {
                postalAddressDTO.Add(ConvertToDTO(postalAddressDataDTO));
            }

            return postalAddressDTO;
        }

        /// <summary>
        /// Business rule implementation for PAF modify events
        /// </summary>
        /// <param name="postalAddressDetails">Postal Address Details</param>
        private async Task ModifyPAFRecords(PostalAddressDTO postalAddressDetails, Guid addressTypePAF, string strFileName)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.ModifyPAFRecords"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(ModifyPAFRecords);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                Guid deliveryPointUseIndicatorPAF = Guid.Empty;
                Guid postCodeGuid = Guid.Empty;

                // Construct New PostalAddressDTO
                PostalAddressDataDTO objPostalAddress = new PostalAddressDataDTO
                {
                    Postcode = postalAddressDetails.Postcode,
                    PostTown = postalAddressDetails.PostTown,
                    DependentLocality = postalAddressDetails.DependentLocality,
                    DoubleDependentLocality = postalAddressDetails.DoubleDependentLocality,
                    Thoroughfare = postalAddressDetails.Thoroughfare,
                    DependentThoroughfare = postalAddressDetails.DependentThoroughfare,
                    BuildingNumber = postalAddressDetails.BuildingNumber,
                    BuildingName = postalAddressDetails.BuildingName,
                    SubBuildingName = postalAddressDetails.SubBuildingName,
                    POBoxNumber = postalAddressDetails.POBoxNumber,
                    DepartmentName = postalAddressDetails.DepartmentName,
                    OrganisationName = postalAddressDetails.OrganisationName,
                    UDPRN = postalAddressDetails.UDPRN,
                    PostcodeType = postalAddressDetails.PostcodeType,
                    SmallUserOrganisationIndicator = postalAddressDetails.SmallUserOrganisationIndicator,
                    DeliveryPointSuffix = postalAddressDetails.DeliveryPointSuffix,
                    RowCreateDateTime = DateTime.UtcNow
                };

                // Matching UDPRN
                PostalAddressDataDTO objPostalAddressMatchedUDPRN = await addressDataService.GetPostalAddress(objPostalAddress.UDPRN);

                // If UDPRN matches..
                if (objPostalAddressMatchedUDPRN != null)
                {
                    // And Address Type is <PAF>
                    if (objPostalAddressMatchedUDPRN.AddressType_GUID == addressTypePAF)
                    {
                        objPostalAddress.AddressType_GUID = addressTypePAF;
                        objPostalAddress.ID = objPostalAddressMatchedUDPRN.ID;

                        bool recordUpdated = await addressDataService.UpdateAddress(objPostalAddress, strFileName);

                        // If Postal address updated then update DeliveryPointUseIndicatorGUID for DeliveryPoint
                        if (recordUpdated)
                        {
                            // Update DPUse in delivery point for matching UDPRN
                            var isDPUseUpdated = postalAddressIntegrationService.UpdateDPUse(postalAddressDetails).Result;
                            if (!isDPUseUpdated)
                            {
                                loggingHelper.Log(string.Format(PostalAddressConstants.PAFERRORLOGMESSAGE, PostalAddressConstants.NoMatchingDP, objPostalAddress.UDPRN, PostalAddressConstants.UPDATE, null, FileType.Paf, DateTime.UtcNow), TraceEventType.Error);
                            }
                        }
                    }

                    // If Address Type is not <PAF>, log error
                    else
                    {
                        loggingHelper.Log(string.Format(PostalAddressConstants.PAFERRORLOGMESSAGE, PostalAddressConstants.WrongAddressType, objPostalAddress.UDPRN, PostalAddressConstants.UPDATE, null, FileType.Paf, DateTime.UtcNow), TraceEventType.Error);
                    }
                }

                // If UDPRN does not match, log error
                else
                {
                    loggingHelper.Log(string.Format(PostalAddressConstants.PAFERRORLOGMESSAGE, PostalAddressConstants.NoMatchToAddressOnUDPRN, objPostalAddress.UDPRN, PostalAddressConstants.UPDATE, null, FileType.Paf, DateTime.UtcNow), TraceEventType.Error);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Delete postal Address details
        /// </summary>
        /// <param name="addressId">Postal Address Id</param>
        /// <returns>boolean</returns>
        private async Task<bool> DeletePostalAddress(Guid addressId, Guid deliveryPointId)
        {
            bool isPostalAddressDeleted = false;
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.DeletePostalAddress"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(DeletePostalAddress);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                //Check referece tables before running housekeeping activities

                // PostalAddress physical delete once all refereences are deleted
                isPostalAddressDeleted = await addressDataService.DeletePostalAddress(addressId);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }

            return isPostalAddressDeleted;
        }

        /// <summary>
        /// Match postal Address on the basis of udprn.
        /// </summary>
        /// <param name="udprn">Postal address UDPRN</param>
        /// <param name="pafAddressType">Address type as PAF</param>
        /// <param name="postalAddressStatus">Address status</param>
        private async Task ModifyAddressOnUdprn(int udprn, Guid pafAddressType, Guid postalAddressStatus)
        {
            bool isPostalAddressMarkedDeleted = false;

            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.ModifyAddressOnUdprn"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(ModifyAddressOnUdprn);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                var postalAddress = addressDataService.GetPostalAddress(udprn).Result;

                if (postalAddress == null)
                {
                    loggingHelper.Log(string.Format(PostalAddressConstants.NoMatchToAddressOnUDPRN, udprn), TraceEventType.Information);
                }
                else if (postalAddress != null && postalAddress.AddressType_GUID != pafAddressType)
                {
                    loggingHelper.Log(string.Format(PostalAddressConstants.WrongAddressType, udprn), TraceEventType.Information);
                }
                else
                {
                    // Soft delete
                    if (await addressDataService.UpdatePostalAddressStatus(postalAddress.ID, postalAddressStatus))
                    {
                        if (postalAddress.DeliveryPoints != null && postalAddress.DeliveryPoints.Count == 1)
                        {
                            // Delete delivery point for respective postal address
                            isPostalAddressMarkedDeleted = postalAddressIntegrationService.DeleteDeliveryPoint(postalAddress.DeliveryPoints.SingleOrDefault().ID).Result;
                        }
                        else
                        {
                            // delivery point not found for respective postal address
                            loggingHelper.Log(string.Format(PostalAddressConstants.PAFErrorMessageForDPNotFoundDelete, postalAddress.ID), TraceEventType.Information);
                        }
                    }
                    else
                    {
                        loggingHelper.Log(string.Format(PostalAddressConstants.PAFERRORLOGMESSAGE, PostalAddressConstants.PAFErrorMessageForPostalAddressStatusNotUpdated, udprn, PostalAddressConstants.DELETE, null, FileType.Paf, DateTime.UtcNow), TraceEventType.Error);
                    }
                }
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
            }
        }

        /// <summary>
        /// Method to convert List DTO to List DataDTO of PostalAddress
        /// </summary>
        /// <param name="objPostalAddress"></param>
        /// <returns></returns>
        private List<PostalAddressDataDTO> ConvertCollectionDTOToCollectionDataDTO(List<PostalAddressDTO> postalAddresses)
        {
            using (loggingHelper.RMTraceManager.StartTrace("BusinessService.GetReferenceData"))
            {
                string methodName = typeof(PostalAddressBusinessService) + "." + nameof(ConvertCollectionDTOToCollectionDataDTO);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodEntryEventId);

                List<PostalAddressDataDTO> postalAddressDTOs = new List<PostalAddressDataDTO>();

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<PostalAddressDTO, PostalAddressDataDTO>();
                });
                Mapper.Configuration.CreateMapper();

                postalAddressDTOs = Mapper.Map<List<PostalAddressDTO>, List<PostalAddressDataDTO>>(postalAddresses);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressBusinessServiceMethodExitEventId);
                return postalAddressDTOs;
            }
        }

        #endregion private methods
    }
}