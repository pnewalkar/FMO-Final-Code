using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;
using System.Linq;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Integration.Interface;

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

        #region property declaration

        private IUnitLocationDataService unitLocationDataService;
        private IPostCodeSectorDataService postcodeSectorDataService = default(IPostCodeSectorDataService);
        private IPostCodeDataService postCodeDataService = default(IPostCodeDataService);
        private IScenarioDataService scenarioDataService = default(IScenarioDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IUnitManagerIntegrationService unitManagerIntegrationService = default(IUnitManagerIntegrationService);

        #endregion property declaration
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationBusinessService"/> class.
        /// </summary>
        /// <param name="unitLocationDataService"> The unit location respository</param>
        /// <param name="postcodeSectorDataService">post code sector data service</param>
        /// <param name="postCodeDataService">post code data service</param>
        /// <param name="scenarioDataService">scenario data service</param>
        public UnitLocationBusinessService(IUnitLocationDataService unitLocationDataService, IPostCodeSectorDataService postcodeSectorDataService, IPostCodeDataService postCodeDataService, IScenarioDataService scenarioDataService, ILoggingHelper loggingHelper, IUnitManagerIntegrationService unitManagerIntegrationService)
        {
            this.unitLocationDataService = unitLocationDataService;
            this.postcodeSectorDataService = postcodeSectorDataService;
            this.postCodeDataService = postCodeDataService;
            this.scenarioDataService = scenarioDataService;
            this.loggingHelper = loggingHelper;
            this.unitManagerIntegrationService = unitManagerIntegrationService;
        }

        /// <summary>
        /// Get all Delivery units for an user.
        /// </summary>
        /// <param name="userId">The unit unique identifier.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO" />.
        /// </returns>
        public List<UnitLocationDTO> GetDeliveryUnitsForUser(Guid userId)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetDeliveryUnitsForUser);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetDeliveryUnitsForUser"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                //reference data value for PostcodeSector with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.PostcodeArea.GetDescription()).Result;

                var unitLocationDataDtoList = unitLocationDataService.GetDeliveryUnitsForUser(userId, Guid.NewGuid());

                var unitLocationDtoList = unitLocationDataDtoList.Select(x => new UnitLocationDTO
                {
                    ID = x.LocationId,
                    Area = x.Area,
                    UnitBoundryPolygon = x.Shape,
                    UnitName = x.Name
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
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUdprn(int udprn)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostCodeSectorByUdprn);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeSectorByUdprn"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                var postcodeTypCategory = unitManagerIntegrationService.GetReferenceDataSimpleLists(PostCodeType).Result;
                var dictionarySector = postcodeTypCategory.ReferenceDatas.Where(x => x.ReferenceDataValue == PostCodeTypeCategory.PostcodeSector.GetDescription()).SingleOrDefault().ID;
                var dictionaryDistrict = postcodeTypCategory.ReferenceDatas.Where(x => x.ReferenceDataValue == PostCodeTypeCategory.PostcodeSector.GetDescription()).SingleOrDefault().ID;

                var postCodeSector = await postcodeSectorDataService.GetPostCodeSectorByUdprn(udprn, dictionarySector, dictionaryDistrict);
                PostCodeSectorDTO postCodeSectorDto = GenericMapper.Map<PostCodeSectorDataDTO, PostCodeSectorDTO>(postCodeSector);
               
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
        public async Task<List<PostCodeDTO>> GetPostCodeUnitForBasicSearch(string searchText, Guid userUnit)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostCodeUnitForBasicSearch);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeUnitForBasicSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                //reference data value for Postcode with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.Postcode.GetDescription()).Result;

                var postcodeUnits = await postCodeDataService.GetPostCodeUnitForBasicSearch(searchText, userUnit, postcodeTypeGUID).ConfigureAwait(false);
                List<PostCodeDTO> postCodeList = postcodeUnits.Select(x => new PostCodeDTO
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
        public async Task<int> GetPostCodeUnitCount(string searchText, Guid userUnit)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostCodeUnitCount);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeUnitCount"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                //reference data value for Postcode with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.Postcode.GetDescription()).Result;
                var postCodeUnitCount = await postCodeDataService.GetPostCodeUnitCount(searchText, userUnit, postcodeTypeGUID).ConfigureAwait(false);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postCodeUnitCount;
            }
        }

        /// <summary>
        /// Fetch the post code for advanced Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">userUnit Guid Id</param>
        /// <returns>list of PostCodeDTO</returns>
        public async Task<List<PostCodeDTO>> GetPostCodeUnitForAdvanceSearch(string searchText, Guid userUnit)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostCodeUnitForAdvanceSearch);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeUnitForAdvanceSearch"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                //reference data value for Postcode with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.Postcode.GetDescription()).Result;
                var postcodeUnits = await postCodeDataService.GetPostCodeUnitForAdvanceSearch(searchText, userUnit, postcodeTypeGUID).ConfigureAwait(false);

                List<PostCodeDTO> postCodeList = postcodeUnits.Select(x => new PostCodeDTO
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
        public async Task<Guid> GetPostCodeID(string postCode)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetPostCodeID);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeID"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                var getPostCodeID = await postCodeDataService.GetPostCodeID(postCode);

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return getPostCodeID;
            }
        }

        /// <summary>
        /// Get the list of route scenarios by the operationstateID and locationID.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="locationID">The location id.</param>
        /// <returns>List</returns>
        public List<ScenarioDTO> GetRouteScenarios(Guid operationStateID, Guid locationID)
        {
            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetRouteScenarios);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetRouteScenarios"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                var scenarioDataList = scenarioDataService.GetRouteScenarios(operationStateID, locationID);
                List<ScenarioDTO> scenariolist = GenericMapper.MapList<ScenarioDataDTO, ScenarioDTO>(scenarioDataList);

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return scenariolist;
            }
        }

        /// <summary>
        /// Gets postcode details by postcode guids
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <returns>List</returns>
        public async Task<List<PostCodeDTO>> GetPostCodeDetails(List<Guid> postcodeGuids)
        {
            //TODO: 1. Add to entry in reference data xml in ref data service for - PostcodeSector
            //      2. Call ReferenceDataService for getting PostcodeSector - 9813B8BE-83FB-48D0-A8A2-703B2524002B
            //      3. Replace Guid.NewGuid() with actual value

            string methodName = typeof(UnitLocationBusinessService) + "." + nameof(GetDeliveryUnitsForUser);
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeUnitCount"))
            {
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId);

                //reference data value for Postcode with Category - Postcode Type
                Guid postcodeTypeGUID = unitManagerIntegrationService.GetReferenceDataGuId(PostCodeType, PostCodeTypeCategory.PostcodeSector.GetDescription()).Result;

                var postcodeDataDto = await unitLocationDataService.GetPostCodes(postcodeGuids, postcodeTypeGUID);
                var postCodeDto = GenericMapper.MapList<PostCodeDataDTO, PostCodeDTO>(postcodeDataDto.ToList());

                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId);
                return postCodeDto;
            }
        }
    }
}