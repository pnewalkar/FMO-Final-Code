using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.Utilities.HelperMiddleware;
using RM.DataManagement.DeliveryPoint.WebAPI.BusinessService;

namespace RM.DataManagement.DeliveryPoint.WebAPI.Controllers
{
    [Route("api/DeliveryPointManager")]
    public class DeliveryPointController : RMBaseController
    {
        #region Member Variables

        private IDeliveryPointBusinessService businessService = default(IDeliveryPointBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

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
        [Authorize(Roles = UserAccessFunctionsConstants.ViewDeliveryPoints)]
        [Route("deliverypoints")]
        [HttpGet]
        public JsonResult GetDeliveryPoints(string bbox)
        {
            return Json(businessService.GetDeliveryPoints(bbox, CurrentUserUnit));
        }

        /// <summary>
        /// Get coordinates of the delivery point by Guid
        /// </summary>
        /// <param name="Guid">The Guid </param>
        /// <returns>The coordinates of the delivery point</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewDeliveryPoints)]
        [Route("deliverypoint/Guid/{id}")]
        [HttpGet]
        public IActionResult GetDeliveryPointByGuId(Guid id, [FromQuery]string fields)
        {
            var geoJsonfeature = businessService.GetDeliveryPointByGuid(id);
            return Ok(geoJsonfeature);
        }

        /// <summary>
        /// Get mapped address location of delivery point by UDPRN
        /// </summary>
        /// <param name="udprn">The UDPRN number</param>
        /// <returns>The coordinates of the delivery point</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.ViewDeliveryPoints)]
        [Route("deliverypoint/Details/{udprn}")]
        [HttpGet]
        public JsonResult GetDetailDeliveryPointByUDPRN(int udprn, [FromQuery]string fields)
        {
            return Json(businessService.GetDetailDeliveryPointByUDPRN(udprn));
        }

        /// <summary>
        /// Create delivery point for PAF and NYB records.
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto</param>
        /// <returns>createDeliveryPointModelDTO</returns>
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [Route("deliverypoint/newdeliverypoint")]

        // [Route("CreateDeliveryPoint")]
        [HttpPost]
        public async Task<IActionResult> CreateDeliveryPoint([FromBody]AddDeliveryPointDTO deliveryPointDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.AddDeliveryPoint"))
                {
                    CreateDeliveryPointModelDTO createDeliveryPointModelDTO = null;
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    createDeliveryPointModelDTO = await businessService.CreateDeliveryPoint(deliveryPointDto);
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodExitEventId, LoggerTraceConstants.Title);

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
        [Route("deliverypoint")]
        [HttpPut]
        public async Task<IActionResult> UpdateDeliveryPoint([FromBody] DeliveryPointModelDTO deliveryPointModelDto)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdateDeliveryPoint"))
                {
                    UpdateDeliveryPointModelDTO updateDeliveryPointModelDTO = null;
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    updateDeliveryPointModelDTO = await businessService.UpdateDeliveryPointLocation(deliveryPointModelDto);
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodExitEventId, LoggerTraceConstants.Title);
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
        [Route("deliverypoint/routes/{deliveryPointId}")]
        [HttpGet]
        public List<KeyValuePair<string, string>> GetRouteForDeliveryPoint(Guid deliveryPointId)
        {
            return businessService.GetRouteForDeliveryPoint(deliveryPointId);
        }

        /// <summary>
        /// This method is used to fetch delivery points for Basic search.
        /// </summary>
        /// <param name="searchText">searchText as string</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>Task List of Delivery Point Dto</returns>
        [Route("deliverypoints/basic/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> FetchDeliveryPointsForBasicSearch(string searchText)
        {
            List<DeliveryPointDTO> deliveryPointDTo = null;
            try
            {
                deliveryPointDTo = await businessService.FetchDeliveryPointsForBasicSearch(searchText, CurrentUserUnit);
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

        /// <summary>
        /// Get the count of delivery point
        /// </summary>
        /// <param name="searchText">The text to be searched</param>
        /// <returns>The total count of delivery point</returns>
        [Route("deliverypoints/count/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> GetDeliveryPointsCount(string searchText)
        {
            try
            {
                int deliveryPointCount = await businessService.GetDeliveryPointsCount(searchText, CurrentUserUnit);
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

        /// <summary>
        /// Fetches the delivery points for advanced search.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        [Route("deliverypoints/advance/{searchText}")]
        [HttpGet]
        public async Task<IActionResult> FetchDeliveryPointsForAdvancedSearch(string searchText)
        {
            List<DeliveryPointDTO> deliveryPointDTo = null;

            try
            {
                deliveryPointDTo = await businessService.FetchDeliveryPointsForAdvanceSearch(searchText, CurrentUserUnit);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
            }

            return Ok(deliveryPointDTo);
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
            try
            {
                bool deliveryPointExists = await businessService.DeliveryPointExists(udprn);
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

        /// <summary>
        /// This method updates delivery point location using UDPRN
        /// </summary>
        /// <param name="deliveryPointDto">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        [HttpPost("deliverypoint/batch/location")]
        public async Task<IActionResult> UpdateDeliveryPointLocationOnUDPRN([FromBody]string deliveryPointDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdateDeliveryPointLocationOnUDPRN"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    int deliveryPointGuid = await businessService.UpdateDeliveryPointLocationOnUDPRN(JsonConvert.DeserializeObject<DeliveryPointDTO>(deliveryPointDTO));

                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodExitEventId, LoggerTraceConstants.Title);
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
        [HttpPost("deliverypoint/location")]
        public async Task<IActionResult> UpdateDeliveryPointLocationOnID([FromBody]string deliveryPointDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdateDeliveryPointLocationOnID"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    Guid deliveryPointGuid = await businessService.UpdateDeliveryPointLocationOnID(JsonConvert.DeserializeObject<DeliveryPointDTO>(deliveryPointDTO));
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodExitEventId, LoggerTraceConstants.Title);
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
                // using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointByUDPRN"))
                // {
                DeliveryPointDTO deliveryPointDTO = await businessService.GetDeliveryPointByUDPRNforBatch(udprn);
                return Ok(deliveryPointDTO);

                // }
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
                // using (loggingHelper.RMTraceManager.StartTrace("WebService.GetDeliveryPointByPostalAddress"))
                // {
                var deliveryPoint = businessService.GetDeliveryPointByPostalAddress(addressId);
                if (deliveryPoint == null)
                {
                    return NotFound();
                }

                return Ok(deliveryPoint);

                // }
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
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [Route("deliverypoint/batch")]
        [HttpPost]
        public async Task<IActionResult> InsertDeliveryPoint([FromBody] string objDeliveryPointJson)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("WebService.InsertDeliveryPoint"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodEntryEventId, LoggerTraceConstants.Title);

                    bool success = await businessService.InsertDeliveryPoint(JsonConvert.DeserializeObject<DeliveryPointDTO>(objDeliveryPointJson));
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodExitEventId, LoggerTraceConstants.Title);
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

        /// <summary>
        /// This method updates delivery point access link status
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPointDto as DTO</param>
        /// <returns>updated delivery point</returns>
        [Route("deliverypoint/accesslinkstatus")]
        [HttpPut]
        public IActionResult UpdateDeliveryPointAccessLinkCreationStatus([FromBody]string deliveryPointDTOJson)
        {
            using (loggingHelper.RMTraceManager.StartTrace("WebService.UpdateDeliveryPointAccessLinkCreationStatus"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodEntryEventId, LoggerTraceConstants.Title);

                bool success = businessService.UpdateDeliveryPointAccessLinkCreationStatus(JsonConvert.DeserializeObject<DeliveryPointDTO>(deliveryPointDTOJson));
                loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.DeliveryPointAPIPriority, LoggerTraceConstants.DeliveryPointControllerMethodExitEventId, LoggerTraceConstants.Title);
                return Ok(success);
            }
        }

        /// <summary> This method is used to get the delivery points crossing the operational object.
        /// </summary> <param name="boundingBoxCoordinates">bbox coordinates</param> <param
        /// name="accessLink">access link coordinate array</param> <returns>List<DeliveryPointDTO></returns>
        [HttpPost("deliverypoint/crosses/operationalobject/{boundingBoxCoordinates}")]
        public IActionResult GetDeliveryPointsCrossingOperationalObject(string boundingBoxCoordinates, [FromBody]string accessLink)
        {
            var deliveryPoints = businessService.GetDeliveryPointsCrossingOperationalObject(boundingBoxCoordinates, JsonConvert.DeserializeObject<DbGeometry>(accessLink, new DbGeometryConverter()));
            if (deliveryPoints == null)
            {
                return NotFound();
            }

            return Ok(deliveryPoints);
        }

        /// <summary>
        /// This method is used to fetch Delivery Point by unique identifier.
        /// </summary>
        /// <param name="deliveryPointGuid">Delivery point unique identifier.</param>
        /// <returns>DeliveryPointDTO</returns>
        [HttpGet("deliverypoint/guid:{deliveryPointGuid}")]
        public IActionResult GetDeliveryPoint(Guid deliveryPointGuid, [FromQuery]string fields)
        {
            var deliveryPoint = businessService.GetDeliveryPoint(deliveryPointGuid);
            if (deliveryPoint == null)
            {
                return NotFound();
            }

            CreateSummaryObject<DeliveryPointDTO> createSummary = new CreateSummaryObject<DeliveryPointDTO>();

            if (!string.IsNullOrEmpty(fields))
            {
                deliveryPoint = (DeliveryPointDTO)createSummary.SummariseProperties(deliveryPoint, fields);
            }

            return Ok(deliveryPoint);
        }

        #endregion Methods
    }
}