using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;

namespace RM.DataManagement.UnitManager.WebAPI.Controllers
{
    /// <summary>
    /// Unit location controller.
    /// </summary>
    [Route("api/UnitManager")]
    public class UnitLocationController : RMBaseController
    {
        private readonly IUnitLocationBusinessService unitLocationBusinessService = default(IUnitLocationBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public UnitLocationController(IUnitLocationBusinessService unitLocationBusinessService, ILoggingHelper loggingHelper)
        {
            this.unitLocationBusinessService = unitLocationBusinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Fetches Delivery Unit
        /// </summary>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("Unit")]
        public List<UnitLocationDTO> GetUnitLocations()
        {
            return unitLocationBusinessService.FetchDeliveryUnitsForUser(UserId);
        }

        /// <summary>
        /// Fetches Postcode sector
        /// </summary>
        /// <returns>List</returns>
        [Authorize]

        // [HttpGet("GetPostCodeSectorByUDPRN")
        [HttpGet("postcodesector/udprn: {udprn}")]
        public async Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN)
        {
            try
            {
                return await unitLocationBusinessService.GetPostCodeSectorByUDPRN(uDPRN);
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
        /// Fetch the post code unit for basic search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        // [HttpGet("FetchPostCodeUnitForBasicSearch")]
        [HttpGet("postcodes/basic/{searchText}")]
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText)
        {
            try
            {
                return await unitLocationBusinessService.FetchPostCodeUnitForBasicSearch(searchText, CurrentUserUnit);
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
        /// Get the post code unit count
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        // [HttpGet("GetPostCodeUnitCount")]
        [HttpGet("postcodes/count/{searchText}")]
        public async Task<int> GetPostCodeUnitCount(string searchText)
        {
            try
            {
                return await unitLocationBusinessService.GetPostCodeUnitCount(searchText, CurrentUserUnit);
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
        /// Fetch the post code unit for advance search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        // [HttpGet("FetchPostCodeUnitForAdvanceSearch")]
        [HttpGet("postcodes/advance/{searchText}")]
        public async Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText)
        {
            try
            {
                return await unitLocationBusinessService.FetchPostCodeUnitForAdvanceSearch(searchText, CurrentUserUnit);
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
        /// Fetch the post code unit for advance search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("postcode/guid/{searchText}")]
        public async Task<Guid> GetPostCodeID(string searchText)
        {
            try
            {
                return await unitLocationBusinessService.GetPostCodeID(searchText);
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
        /// Fetches Delivery Scenario
        /// </summary>
        /// <param name="operationStateID">operation State ID</param>
        /// <param name="deliveryUnitID">delivery Unit ID</param>
        /// <param name="fields">The fields to be returned</param>
        /// <returns></returns>
        // [HttpGet("Scenario")]
        [HttpGet("scenario/{operationStateID}/{deliveryUnitID}/{fields}")]
        public IActionResult FetchDeliveryScenario(Guid operationStateID, Guid deliveryUnitID, string fields)
        {
            List<object> deliveryScenerioList = null;
            List<ScenarioDTO> deliveryScenerio = unitLocationBusinessService.FetchDeliveryScenario(operationStateID, deliveryUnitID);
            CreateSummaryObject<ScenarioDTO> createSummary = new CreateSummaryObject<ScenarioDTO>();

            if (!string.IsNullOrEmpty(fields))
            {
                deliveryScenerioList = createSummary.SummarisePropertiesForList(deliveryScenerio, fields);
            }

            return Ok(deliveryScenerioList);
        }
    }
}