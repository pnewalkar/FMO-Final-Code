using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;

namespace RM.DataManagement.UnitManager.WebAPI.BusinessService.Implementation
{
    /// <summary>
    /// This class contains methods for fetching Delivery unit data.
    /// </summary>
    /// <seealso cref="Fmo.BusinessServices.Interfaces.IUnitLocationBusinessService" />
    public class UnitLocationBusinessService // : IUnitLocationBusinessService
    {
        private const int BNGCOORDINATESYSTEM = 27700;

        private IUnitLocationDataService unitLocationRespository;
        private IPostCodeSectorDataService postcodeSectorDataService = default(IPostCodeSectorDataService);
        private IPostCodeDataService postCodeDataService = default(IPostCodeDataService);
        private IScenarioDataService scenarioDataService = default(IScenarioDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitLocationBusinessService"/> class.
        /// </summary>
        /// <param name="unitLocationRespository"> The unit location respository</param>
        /// <param name="postcodeSectorDataService">post code sector data service</param>
        /// <param name="postCodeDataService">post code data service</param>
        /// <param name="scenarioDataService">scenario data service</param>
        public UnitLocationBusinessService(IUnitLocationDataService unitLocationRespository, IPostCodeSectorDataService postcodeSectorDataService, IPostCodeDataService postCodeDataService, IScenarioDataService scenarioDataService, ILoggingHelper loggingHelper)
        {
            this.unitLocationRespository = unitLocationRespository;
            this.postcodeSectorDataService = postcodeSectorDataService;
            this.postCodeDataService = postCodeDataService;
            this.scenarioDataService = scenarioDataService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Fetch the Delivery unit.
        /// </summary>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The <see cref="UnitLocationDTO" />.
        /// </returns>
        public UnitLocationDTO FetchDeliveryUnit(Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryUnit"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var unitLocationDTO = unitLocationRespository.FetchDeliveryUnit(unitGuid);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return unitLocationDTO;
            }
        }

        /// <summary>
        /// Fetch the Delivery units for user.
        /// </summary>
        /// <param name="userId">The unit unique identifier.</param>
        /// <returns>
        /// List of <see cref="UnitLocationDTO" />.
        /// </returns>
        public List<UnitLocationDTO> FetchDeliveryUnitsForUser(Guid userId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryUnitsForUser"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var unitLocationDTOList = unitLocationRespository.FetchDeliveryUnitsForUser(userId);

                foreach (var unitLocationDTO in unitLocationDTOList)
                {
                    // take the unit boundry plus 1 mile envelope
                    var unitBoundary = SqlGeometry.STPolyFromWKB(new SqlBytes(unitLocationDTO.UnitBoundryPolygon.Envelope.Buffer(1609.34).Envelope.AsBinary()), BNGCOORDINATESYSTEM).MakeValid();

                    unitLocationDTO.BoundingBoxCenter = new List<double> { unitBoundary.STCentroid().STPointN(1).STX.Value, unitBoundary.STCentroid().STPointN(1).STY.Value };

                    unitLocationDTO.BoundingBox = new List<double> { unitBoundary.STPointN(1).STX.Value, unitBoundary.STPointN(1).STY.Value, unitBoundary.STPointN(3).STX.Value, unitBoundary.STPointN(3).STY.Value };
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return unitLocationDTOList;
            }
        }

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeSectorByUDPRN"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);
                var getPostCodeSectorByUDPRN = await postcodeSectorDataService.GetPostCodeSectorByUDPRN(uDPRN);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return getPostCodeSectorByUDPRN;
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
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchPostCodeUnitForBasicSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var fetchPostCodeUnitForBasicSearch = await postCodeDataService.FetchPostCodeUnitForBasicSearch(searchText, userUnit).ConfigureAwait(false);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return fetchPostCodeUnitForBasicSearch;
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
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeUnitCount"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var getPostCodeUnitCount = await postCodeDataService.GetPostCodeUnitCount(searchText, userUnit).ConfigureAwait(false);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return getPostCodeUnitCount;
            }
        }

        /// <summary>
        /// Fetch the post code for advanced Search.
        /// </summary>
        /// <param name="searchText">Text to search</param>
        /// <param name="userUnit">Guid</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText, Guid userUnit)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchPostCodeUnitForAdvanceSearch"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var fetchPostCodeUnitForAdvanceSearch = await postCodeDataService.FetchPostCodeUnitForAdvanceSearch(searchText, userUnit).ConfigureAwait(false);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return fetchPostCodeUnitForAdvanceSearch;
            }
        }

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        public async Task<Guid> GetPostCodeID(string postCode)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GetPostCodeID"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var getPostCodeID = await postCodeDataService.GetPostCodeID(postCode);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return getPostCodeID;
            }
        }

        /// <summary>
        /// Fetch the Delivery Scenario.
        /// </summary>
        /// <param name="operationStateID">The operationstate id.</param>
        /// <param name="deliveryScenarioID">The delivery scenario id.</param>
        /// <returns>List</returns>
        public List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.FetchDeliveryScenario"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var fetchScenario = scenarioDataService.FetchScenario(operationStateID, deliveryScenarioID);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.UnitManagerAPIPriority, LoggerTraceConstants.UnitManagerBusinessServiceMethodExitEventId, LoggerTraceConstants.Title);
                return fetchScenario;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <returns></returns>
        public async Task<List<PostCodeDTO>> GetPostCodeDetails(List<Guid> postcodeGuids)
        {

            //TODO: 1. Add to entry in reference data xml in ref data service for - PostcodeSector
            //      2. Call ReferenceDataService for getting PostcodeSector - 9813B8BE-83FB-48D0-A8A2-703B2524002B
            //      3. Replace Guid.NewGuid() with actual value
            return await unitLocationRespository.GetPostCodes(postcodeGuids, Guid.NewGuid());
        }
    }
}