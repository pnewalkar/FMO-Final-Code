using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPoint.WebAPI.DTO;
using RM.Data.DeliveryPoint.WebAPI.DTO.Model;
using RM.DataManagement.DeliveryPoint.WebAPI.BusinessService;
using Microsoft.AspNetCore.Authorization;

namespace RM.DataManagement.DeliveryPoint.WebAPI.Controllers
{
    [Route("api/DeliveryPointManager")]
    public class DeliveryPointController : RMBaseController
    {
        #region Member Variables

        private IDeliveryPointBusinessService businessService = default(IDeliveryPointBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        private int priority = LoggerTraceConstants.DeliveryPointAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointControllerMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public DeliveryPointController(IDeliveryPointBusinessService businessService, ILoggingHelper loggingHelper)
        {
            this.businessService = businessService;
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///This method is used to Get Delivery Point Object.
        /// </summary>
        /// <param name="bbox">bbox as string</param>
        /// <returns>Json Result of Delivery Points</returns>
     //   [Authorize]
        [Route("deliverypoints")]
        [HttpGet]
        public JsonResult GetDeliveryPoints(string bbox)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPoints"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPoints);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                object deliveryPoints = businessService.GetDeliveryPoints(bbox, CurrentUserUnit);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Json(deliveryPoints);
            }
        }

        /// <summary>
        /// Get coordinates of the delivery point by Guid
        /// </summary>
        /// <param name="Guid">The Guid </param>
        /// <returns>The coordinates of the delivery point</returns>
     //   [Authorize]
        [Route("deliverypoint/Guid/{id}")]
        [HttpGet]
        public IActionResult GetDeliveryPointByGuId(Guid id, [FromQuery]string fields)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointByGuId"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPointByGuId);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var geoJsonfeature = businessService.GetDeliveryPointByGuid(id);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Ok(geoJsonfeature);
            }
        }

        /// <summary>
        /// Get mapped address location of delivery point by UDPRN
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        [Authorize]
        [Route("deliverypoint/Details/{udprn}")]
        [HttpGet]
        public JsonResult GetDetailDeliveryPointByUDPRN(int udprn, [FromQuery]string fields)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDetailDeliveryPointByUDPRN"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetDetailDeliveryPointByUDPRN);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getDetailDeliveryPointByUDPRN = Json(businessService.GetDetailDeliveryPointByUDPRN(udprn));
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return getDetailDeliveryPointByUDPRN;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto</param>
        /// <returns>createDeliveryPointModelDTO</returns>
     //   [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [Route("deliverypoint/newdeliverypoint")]
        [HttpPost]
        public async Task<IActionResult> CreateDeliveryPoint([FromBody]AddDeliveryPointDTO deliveryPointDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.AddDeliveryPoint"))
                {
                    CreateDeliveryPointModelDTO createDeliveryPointModelDTO = null;
                    string methodName = typeof(DeliveryPointController) + "." + nameof(CreateDeliveryPoint);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    createDeliveryPointModelDTO = await businessService.CreateDeliveryPoint(deliveryPointDto);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return Ok(createDeliveryPointModelDTO);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Update delivery point
        /// </summary>
        /// <param name="deliveryPointModelDto">deliveryPointDTO</param>
        /// <returns>updateDeliveryPointModelDTO</returns>
     //   [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [Route("deliverypoint")]
        [HttpPut]
        public async Task<IActionResult> UpdateDeliveryPoint([FromBody] DeliveryPointModelDTO deliveryPointModelDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdateDeliveryPoint"))
                {
                    UpdateDeliveryPointModelDTO updateDeliveryPointModelDTO = null;
                    string methodName = typeof(DeliveryPointController) + "." + nameof(UpdateDeliveryPoint);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    updateDeliveryPointModelDTO = await businessService.UpdateDeliveryPointLocation(deliveryPointModelDto);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(updateDeliveryPointModelDTO);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// this method fetches route details for selected delivery point
        /// </summary>
        /// <param name="deliveryPointId">Guid</param>
        /// <returns>List of Key Value Pair for route details</returns>
        [Authorize]
        [Route("deliverypoint/routes/{deliveryPointId}")]
        [HttpGet]
        public List<KeyValuePair<string, string>> GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetRouteForDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetRouteForDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var getRouteForDeliveryPoint = businessService.GetRouteForDeliveryPoint(deliveryPointId);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return getRouteForDeliveryPoint;
            }
        }

        /// <summary>
        /// This method is used to fetch delivery points for Basic search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
       // [Authorize]
        [Route("deliverypoints/basic/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> GetDeliveryPointsForBasicSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointsForBasicSearch"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPointsForBasicSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<DeliveryPointDTO> deliveryPointDTo = null;
                try
                {
                    deliveryPointDTo = await businessService.GetDeliveryPointsForBasicSearch(searchText, CurrentUserUnit);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(deliveryPointDTo);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <returns>The total count of delivery point</returns>
      //  [Authorize]
        [Route("deliverypoints/count/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> GetDeliveryPointsCount(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointsCount"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPointsCount);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    int deliveryPointCount = await businessService.GetDeliveryPointsCount(searchText, CurrentUserUnit);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(deliveryPointCount);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// Fetches the delivery points for advanced search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
       // [Authorize]
        [Route("deliverypoints/advance/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> GetDeliveryPointsForAdvancedSearch(string searchText)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointsForAdvancedSearch"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPointsForAdvancedSearch);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<DeliveryPointDTO> deliveryPointDTo = null;

                try
                {
                    deliveryPointDTo = await businessService.GetDeliveryPointsForAdvanceSearch(searchText, CurrentUserUnit);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Ok(deliveryPointDTo);
            }
        }

        /// <summary>
        /// Check if the delivery point exists for a given UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>boolean value</returns>
        [Route("deliverypoint/existence/{udprn}")]
        [HttpGet]
        public async Task<IActionResult> DeliveryPointExists(int udprn)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.DeliveryPointExists"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(DeliveryPointExists);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    bool deliveryPointExists = await businessService.DeliveryPointExists(udprn);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return Ok(deliveryPointExists);
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
            }
        }

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        [Authorize]
        [HttpPost("deliverypoint/batch/location")]
        public async Task<IActionResult> UpdateDeliveryPointLocationOnUDPRN([FromBody]string deliveryPointDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdateDeliveryPointLocationOnUDPRN"))
                {
                    string methodName = typeof(DeliveryPointController) + "." + nameof(UpdateDeliveryPointLocationOnUDPRN);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    int deliveryPointGuid = await businessService.UpdateDeliveryPointLocationOnUDPRN(JsonConvert.DeserializeObject<DeliveryPointDTO>(deliveryPointDTO));

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(deliveryPointGuid);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        [Authorize]
        [HttpPost("deliverypoint/location")]
        public async Task<IActionResult> UpdateDeliveryPointLocationOnID([FromBody]string deliveryPointDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdateDeliveryPointLocationOnID"))
                {
                    string methodName = typeof(DeliveryPointController) + "." + nameof(UpdateDeliveryPointLocationOnID);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    Guid deliveryPointGuid = await businessService.UpdateDeliveryPointLocationOnID(JsonConvert.DeserializeObject<DeliveryPointDTO>(deliveryPointDTO));
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return Ok(deliveryPointGuid);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by udprn.
        /// </summary>
        /// <param name="udprn">udprn as int</param>
        /// <returns>DeliveryPointDTO</returns>
        [Route("deliverypoint/batch/{udprn}")]
        [HttpGet]
        public async Task<IActionResult> GetDeliveryPointByUDPRNforBatch(int udprn)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointByUDPRNforBatch"))
                {
                    string methodName = typeof(DeliveryPointController) + "." + nameof(InsertDeliveryPoint);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    DeliveryPointDTO deliveryPointDTO = await businessService.GetDeliveryPointByUDPRNforBatch(udprn);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return Ok(deliveryPointDTO);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// This method is used to fetch delivery points by the Postal Address Guid
        /// </summary>
        /// <param name="addressId">Postal Address Guid to find corresponding delivery point</param>
        /// <returns>DeliveryPointDTO object</returns>
        [Route("deliverypoint/addressId:{addressId}")]
        [HttpGet]
        public IActionResult GetDeliveryPointByPostalAddress(Guid addressId)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointByPostalAddress"))
                {
                    string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPointByPostalAddress);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    var deliveryPoint = businessService.GetDeliveryPointByPostalAddress(addressId);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return Ok(deliveryPoint);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        [Route("deliverypoint/location/addressId:{addressId}")]
        [HttpGet]
        public async Task<IActionResult> GetDeliveryPointByPostalAddressWithLocation(Guid addressId)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointByPostalAddressWithLocation"))
                {
                    string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPointByPostalAddressWithLocation);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    var deliveryPoint = await businessService.GetDeliveryPointByPostalAddressWithLocation(addressId);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(deliveryPoint);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// This method is used to insert delivery point for case of PAF Batch job.
        /// </summary>
        /// <param name="objDeliveryPoint">Delivery point dto as object</param>
        /// <returns>bool</returns>
        [Authorize]
        [Route("deliverypoint/batch")]
        [HttpPost]
        public async Task<IActionResult> InsertDeliveryPoint([FromBody] string objDeliveryPointJson)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.InsertDeliveryPoint"))
                {
                    string methodName = typeof(DeliveryPointController) + "." + nameof(InsertDeliveryPoint);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    bool success = await businessService.InsertDeliveryPoint(JsonConvert.DeserializeObject<DeliveryPointDTO>(objDeliveryPointJson)) != Guid.Empty;
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(success);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary> This method is used to get the delivery points crossing the operational object.
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        [HttpPost("deliverypoint/crosses/operationalobject/{boundingBoxCoordinates}")]
        public IActionResult GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, [FromBody]string accessLink)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointsCrossingOperationalObject"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPointsCrossingOperationalObject);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var deliveryPoints = businessService.GetDeliveryPointsCrossingOperationalObject(boundingBoxCoordinates, JsonConvert.DeserializeObject<DbGeometry>(accessLink, new DbGeometryConverter()));
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodExitEventId, LoggerTraceConstants.Title);

                return Ok(deliveryPoints);
            }
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>DeliveryPointDTO</returns>
        [HttpGet("deliverypoint/guid:{deliveryPointGuid}")]
        public IActionResult GetDeliveryPoint(Guid deliveryPointGuid, [FromQuery]string fields)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(GetDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var deliveryPoint = businessService.GetDeliveryPoint(deliveryPointGuid);
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodExitEventId, LoggerTraceConstants.Title);

                if (deliveryPoint == null)
                {
                    return NotFound();
                }

                CreateSummaryObject<DeliveryPointDTO> createSummary = new CreateSummaryObject<DeliveryPointDTO>();

                if (!string.IsNullOrEmpty(fields))
                {
                    deliveryPoint = (DeliveryPointDTO)createSummary.SummariseProperties(deliveryPoint, fields);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Ok(deliveryPoint);
            }
        }

        [HttpPut("deliverypoint/batch/addressGuid:{addressGuid}")]
        public Task<bool> UpdatePAFIndicator(Guid addressGuid, [FromBody] Guid pafIndicator)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdatePAFIndicator"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(UpdatePAFIndicator);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var isDeliveryPointUpdated = businessService.UpdatePAFIndicator(addressGuid, pafIndicator);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return isDeliveryPointUpdated;
            }
        }

        [HttpDelete("deliverypoint/batch/delete/id:{id}")]
        public Task<bool> DeleteDeliveryPoint(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.DeleteDeliveryPoint"))
            {
                string methodName = typeof(DeliveryPointController) + "." + nameof(DeleteDeliveryPoint);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var isDeliveryPointUpdated = businessService.DeleteDeliveryPoint(id);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return isDeliveryPointUpdated;
            }
        }

        #endregion Methods
    }
}