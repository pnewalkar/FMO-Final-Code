using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.SqlServer.Types;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.UnitManager.WebAPI.DataDTO;
using RM.Data.UnitManager.WebAPI.DTO;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Integration.Interface;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.DTO;

namespace RM.DataManagement.UnitManager.WebAPI.BusinessService.Implementation
{
    /// <summary>
    /// Business service for unit related operations
    /// </summary>
    /// <seealso cref="Fmo.BusinessServices.Interfaces.IUnitLocationBusinessService" />
    public class UnitLocationBusinessService : IUnitLocationBusinessService
    {
        private const int BNGCOORDINATESYSTEM = 27700;
        private const string PostCodeType = "Postcode Type";
        private const string PostalAddressType = "PostalAddressType";
        private const int SearchResultCount = 5;

        #region property declaration

        private IUnitLocationDataService unitLocationDataService;
        private IPostcodeSectorDataService postcodeSectorDataService = default(IPostcodeSectorDataService);
        private IPostcodeDataService postCodeDataService = default(IPostcodeDataService);
        private IScenarioDataService scenarioDataService = default(IScenarioDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IUnitManagerIntegrationService unitManagerIntegrationService = default(IUnitManagerIntegrationService);
        private IPostalAddressDataService postalAddressDataService = default(IPostalAddressDataService);

        #endregion property declaration

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationBusinessService"/> class.
        /// </summary>
        /// <param name="unitLocationDataService"> The unit location respository</param>
        /// <param name="postcodeSectorDataService">post code sector data service</param>
        /// <param name="postCodeDataService">post code data service</param>
        /// <param name="scenarioDataService">scenario data service</param>
        public UnitLocationBusinessService(IUnitLocationDataService unitLocationDataService, IPostcodeSectorDataService postcodeSectorDataService, IPostcodeDataService postCodeDataService, IScenarioDataService scenarioDataService, IPostalAddressDataService postalAddressDataService, ILoggingHelper loggingHelper, IUnitManagerIntegrationService unitManagerIntegrationService)
        {
            // Store injected dependencies
            this.unitLocationDataService = unitLocationDataService;
            this.postcodeSectorDataService = postcodeSectorDataService;
            this.postCodeDataService = postCodeDataService;
            this.scenarioDataService = scenarioDataService;
            this.loggingHelper = loggingHelper;
            this.unitManagerIntegrationService = unitManagerIntegrationService;
            this.postalAddressDataService = postalAddressDataService;
        }

        /// <summary>
        /// Get all units for a user.
        /// </summary>
        /// <param name="userId">user Id.</param>
        /// <param name="currentUserUnitType">current user unit type.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO" />.
        /// </returns>
        public async Task<IEnumerable<UnitLocationDTO>> GetUnitsByUser(Guid userId, string currentUserUnitType)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetUnitsByUser);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetUnitsByUser"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                // reference data value for PostcodeSector with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.PostcodeArea.GetDescription()).Result;

                IEnumerable<UnitLocationDataDTO> unitLocationDataDtoList = null;
                if (!currentUserUnitType.Equals(UserUnit.National.GetDescription()))
                {
                    unitLocationDataDtoList = await unitLocationDataService.GetUnitsByUser(userId, postcodeTypeGUID);
                }
                else
                {
                    unitLocationDataDtoList = await unitLocationDataService.GetUnitsByUserForNational(userId, postcodeTypeGUID);
                }

                var unitLocationDtoList = unitLocationDataDtoList.Select(x => new UnitLocationDTO
                {
                    ID = x.LocationId,
                    Area = x.Area,
                    UnitBoundryPolygon = x.Shape,
                    UnitName = currentUserUnitType.Equals(UserUnit.National.ToString()) ? currentUserUnitType : x.Name
                }).ToList();

                foreach (var unitLocationDto in unitLocationDtoList)
                {
                    // take the unit boundry plus 1 mile envelope
                    var unitBoundary = SqlGeometry.STPolyFromWKB(new SqlBytes(unitLocationDto.UnitBoundryPolygon.Envelope.Buffer(1609.34).Envelope.AsBinary()), BNGCOORDINATESYSTEM).MakeValid();

                    unitLocationDto.BoundingBoxCenter = new List<double> { unitBoundary.STCentroid().STPointN(1).STX.Value, unitBoundary.STCentroid().STPointN(1).STY.Value };

                    unitLocationDto.BoundingBox = new List<double> { unitBoundary.STPointN(1).STX.Value, unitBoundary.STPointN(1).STY.Value, unitBoundary.STPointN(3).STX.Value, unitBoundary.STPointN(3).STY.Value };
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return unitLocationDtoList;
            }
        }

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        public async Task<PostcodeSectorDTO> GetPostcodeSectorByUdprn(int udprn)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostcodeSectorByUdprn);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeSectorByUdprn"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                var postcodeTypCategory = unitManagerIntegrationService.GetReferenceDataSimpleLists(PostCodeType).Result;
                var sectorGuid = postcodeTypCategory.ReferenceDatas.Where(x => x.ReferenceDataValue == PostCodeTypeCategory.PostcodeSector.GetDescription()).SingleOrDefault().ID;
                var districtGuid = postcodeTypCategory.ReferenceDatas.Where(x => x.ReferenceDataValue == PostCodeTypeCategory.PostcodeDistrict.GetDescription()).SingleOrDefault().ID;

                var postCodeSector = await postcodeSectorDataService.GetPostcodeSectorByUdprn(udprn, sectorGuid, districtGuid);
                PostcodeSectorDTO postCodeSectorDto = GenericMapper.Map<PostcodeSectorDataDTO, PostcodeSectorDTO>(postCodeSector);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postCodeSectorDto;
            }
        }

        /// <summary>
        /// Fetch the post code for Basic Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<IEnumerable<PostcodeDTO>> GetPostcodeUnitForBasicSearch(string searchText, Guid userUnit)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostcodeUnitForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostcodeUnitForBasicSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                // reference data value for Postcode with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.Postcode.GetDescription()).Result;
                SearchInputDataDto searchInputs = new SearchInputDataDto
                {
                    SearchText = searchText,
                    UserUnitLocationId = userUnit,
                    PostcodeTypeGUID = postcodeTypeGUID,
                    SearchResultCount = SearchResultCount
                };

                var postcodeUnits = await postCodeDataService.GetPostcodeUnitForBasicSearch(searchInputs).ConfigureAwait(false);
                List<PostcodeDTO> postCodeList = postcodeUnits.Select(x => new PostcodeDTO
                {
                    PostcodeUnit = x.PostcodeUnit
                }).ToList();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postCodeList;
            }
        }

        /// <summary>
        /// Get the count of post code
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <param name="userUnit">Guid userUnit</param>
        /// <returns>The total count of post code</returns>
        public async Task<int> GetPostcodeUnitCount(string searchText, Guid userUnit)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostcodeUnitCount);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostcodeUnitCount"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                // reference data value for Postcode with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.Postcode.GetDescription()).Result;
                SearchInputDataDto searchInputs = new SearchInputDataDto
                {
                    SearchText = searchText,
                    UserUnitLocationId = userUnit,
                    PostcodeTypeGUID = postcodeTypeGUID,
                    SearchResultCount = SearchResultCount
                };

                var postCodeUnitCount = await postCodeDataService.GetPostcodeUnitCount(searchInputs).ConfigureAwait(false);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postCodeUnitCount;
            }
        }

        /// <summary>
        /// Fetch the post code for advanced Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">userUnit Guid Id</param>
        /// <returns>list of PostcodeDTO</returns>
        public async Task<IEnumerable<PostcodeDTO>> GetPostcodeUnitForAdvanceSearch(string searchText, Guid userUnit)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostcodeUnitForAdvanceSearch);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostcodeUnitForAdvanceSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                // reference data value for Postcode with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.Postcode.GetDescription()).Result;
                SearchInputDataDto searchInputs = new SearchInputDataDto
                {
                    SearchText = searchText,
                    UserUnitLocationId = userUnit,
                    PostcodeTypeGUID = postcodeTypeGUID,
                    SearchResultCount = SearchResultCount
                };

                var postcodeUnits = await postCodeDataService.GetPostcodeUnitForAdvanceSearch(searchInputs).ConfigureAwait(false);

                List<PostcodeDTO> postCodeList = postcodeUnits.Select(x => new PostcodeDTO
                {
                    PostcodeUnit = x.PostcodeUnit
                }).ToList();

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postCodeList;
            }
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        public async Task<Guid> GetPostcodeID(string postCode)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostcodeID);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostcodeID"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                var postCodeData = await postCodeDataService.GetPostcodeID(postCode);
                var postCodeID = postCodeData != null ? postCodeData.ID : Guid.Empty;
                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postCodeID;
            }
        }

        /// <summary>
        /// Get the list of route scenarios by the operationstateID and locationID.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="locationID">The location id.</param>
        /// <returns>List</returns>
        public async Task<IEnumerable<ScenarioDTO>> GetRouteScenarios(Guid operationStateID, Guid locationID)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetRouteScenarios);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteScenarios"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                var scenarioDataList = await scenarioDataService.GetScenariosByOperationStateAndDeliveryUnit(operationStateID, locationID);
                List<ScenarioDTO> scenariolist = GenericMapper.MapList<ScenarioDataDTO, ScenarioDTO>(scenarioDataList.ToList());

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return scenariolist;
            }
        }

        /// <summary>
        /// Gets postcode details by postcode guids
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <returns>List</returns>
        public async Task<IEnumerable<PostcodeDTO>> GetPostcodes(List<Guid> postcodeGuids)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostcodes);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostcodes"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                // reference data value for Postcode with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.PostcodeSector.GetDescription()).Result;

                var postcodeDataDto = await unitLocationDataService.GetPostcodes(postcodeGuids, postcodeTypeGUID);
                var postCodeDto = GenericMapper.MapList<PostcodeDataDTO, PostcodeDTO>(postcodeDataDto.ToList());

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postCodeDto;
            }
        }

        /// <summary>
        /// Filter PostalAddress based on the post code
        /// </summary>
        /// <param name="selectedItem">selectedItem</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of postcodes</returns>
        public async Task<PostalAddressDTO> GetPostalAddressDetails(string selectedItem, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressDetails"))
            {
                string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostalAddressDetails);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                List<BindingDTO> nybDetails = new List<BindingDTO>();
                List<BindingDTO> routeDetails = new List<BindingDTO>();
                PostalAddressDTO postalAddressDto = null;
                var postalAddressDetails = await postalAddressDataService.GetPostalAddressDetails(selectedItem, unitGuid);
                var routeData = unitManagerIntegrationService.GetRouteData(selectedItem.Split(',')[1].Trim(), "ID,RouteName").Result;
                routeData.ForEach(route =>
                {
                    routeDetails.Add(new BindingDTO { Value = route.ID, DisplayText = route.RouteName });
                });

                Guid nybAddressTypeId = unitManagerIntegrationService.GetReferenceDataGuId(PostalAddressType, FileType.Nyb.ToString()).Result;

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<PostalAddressDataDTO, PostalAddressDTO>();
                });

                var postalAddressDetailsDTO = Mapper.Map<List<PostalAddressDataDTO>, List<PostalAddressDTO>>(postalAddressDetails);

                if (postalAddressDetailsDTO != null && postalAddressDetailsDTO.Count > 0)
                {
                    postalAddressDto = postalAddressDetailsDTO[0];
                    foreach (var postalAddress in postalAddressDetailsDTO)
                    {
                        if (postalAddress.AddressType_GUID == nybAddressTypeId)
                        {
                            string address = string.Join(",", Convert.ToString(postalAddress.BuildingNumber) ?? string.Empty, postalAddress.BuildingName, postalAddress.SubBuildingName);
                            string formattedAddress = Regex.Replace(address, ",+", ",").Trim(',');
                            nybDetails.Add(new BindingDTO { Value = postalAddress.ID, DisplayText = formattedAddress });
                        }
                    }

                    nybDetails.OrderBy(n => n.DisplayText);
                    nybDetails.Add(new BindingDTO { Value = Guid.Empty, DisplayText = "Not Shown" });
                    postalAddressDto.NybAddressDetails = nybDetails;
                    postalAddressDto.RouteDetails = routeDetails;
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postalAddressDto;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitGuid"></param>
        /// <returns></returns>
        public async Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostalAddressSearchDetails"))
            {
                string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostalAddressSearchDetails);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                List<string> listNames = new List<string> { FileType.Paf.ToString().ToUpper(), FileType.Nyb.ToString().ToUpper() };

                var referenceDataCategoryList = unitManagerIntegrationService.GetReferenceDataSimpleLists(ReferenceDataCategoryNames.PostalAddressType).Result;
                List<Guid> addresstypeIDs = referenceDataCategoryList.ReferenceDatas
                .Where(a => listNames.Contains(a.ReferenceDataValue))
                .Select(a => a.ID).ToList();

                var postalAddress = await postalAddressDataService.GetPostalAddressSearchDetails(searchText, unitGuid, addresstypeIDs);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postalAddress;
            }
        }

        /// <summary>
        /// Gets approx location based on the postal code.
        /// </summary>
        /// <param name="postcode">Postal code</param>
        /// <param name="unitId">Unique identifier for unit.</param>
        /// <returns>The approx location for the given postal code.</returns>
        public async Task<DbGeometry> GetApproxLocation(string postcode, Guid unitId)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetApproxLocation);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetApproxLocation"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                var approxLocation = await postCodeDataService.GetApproxLocation(postcode, unitId);

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return approxLocation;
            }
        }
    }
}