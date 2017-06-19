namespace RM.Common.ReferenceData.WebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
    using RM.CommonLibrary.ExceptionMiddleware;
    using RM.CommonLibrary.HelperMiddleware;

    [Route("api/ReferenceDataManager")]
    public class ReferenceDataController : RMBaseController
    {
        #region Member Variables

        private IReferenceDataBusinessService referenceDataBusinessService = default(IReferenceDataBusinessService);

        #endregion Member Variables

        #region Constructor

        public ReferenceDataController(IReferenceDataBusinessService referenceDataBusinessService)
        {
            this.referenceDataBusinessService = referenceDataBusinessService;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// retrieval of name-value pairs as a discrete type of reference data addressed via the URI
        /// path /nameValuePairs
        /// </summary>
        /// <param name="appGroupName">appGroupName is recorded as the category name</param>
        /// <param name="appItemName">appItemName is recorded as the item name</param>
        /// <returns></returns>
        [HttpGet("nameValuePair")]
        public IActionResult GetNameValueReferenceData(string appGroupName, string appItemName)
        {
            if (string.IsNullOrEmpty(appGroupName) || string.IsNullOrEmpty(appItemName))
            {
                return BadRequest();
            }

            var nameValuePairsObject = referenceDataBusinessService.GetReferenceDataByNameValuePairs(appGroupName, appItemName);

            Tuple<string, NameValuePair> nameValueSingleResource = new Tuple<string, NameValuePair>(Constants.NameValuePair, nameValuePairsObject);

            if (nameValuePairsObject == null)
            {
                throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
            }

            return Ok(nameValueSingleResource);
        }

        /// <summary>
        /// retrieval of name-value pairs as a discrete type of reference data addressed via the URI
        /// path /nameValuePairs
        /// </summary>
        /// <param name="appGroupName">appGroupName is recorded as the category name</param>
        /// <param name="appItemName">appItemName is recorded as the item name</param>
        /// <returns></returns>
        [HttpGet("nameValuePairs")]
        public IActionResult GetNameValueReferenceData(string appGroupName)
        {
            if (string.IsNullOrEmpty(appGroupName))
            {
                return BadRequest();
            }

            var nameValuePairsObject = referenceDataBusinessService.GetReferenceDataByNameValuePairs(appGroupName);
            Tuple<string, List<NameValuePair>> nameValueList = new Tuple<string, List<NameValuePair>>(Constants.NameValuePairs, nameValuePairsObject);

            if (nameValuePairsObject == null)
            {
                throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
            }

            return Ok(nameValueList);
        }

        /// <summary>
        /// retrieval of simple lists as a discrete type of reference data addressed via the URI path /simpleLists
        /// </summary>
        /// <param name="listName">listname maps to ReferenceDataCategory.CategoryName</param>
        /// <returns></returns>
        [HttpGet("simpleLists")]
        public IActionResult GetSimpleListsReferenceData(string listName)
        {
            if (string.IsNullOrEmpty(listName))
            {
                return BadRequest();
            }

            var simpleListObject = referenceDataBusinessService.GetSimpleListsReferenceData(listName);

            Tuple<string, SimpleListDTO> nameValueSingleResource = new Tuple<string, SimpleListDTO>(Constants.SimpleList, simpleListObject);

            if (simpleListObject == null)
            {
                throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
            }

            return Ok(nameValueSingleResource);
        }

        /// <summary>
        /// retrieval of simple lists as a discrete type of reference data addressed via the URI path /simpleLists
        /// </summary>
        /// <param name="listName">listname maps to ReferenceDataCategory.CategoryName</param>
        /// <returns></returns>
        [HttpPost("simpleLists")]
        public IActionResult GetSimpleListsReferenceData([FromBody]List<string> listNames)
        {
            if (listNames == null)
            {
                return BadRequest();
            }

            var simpleDtoList = referenceDataBusinessService.GetSimpleListsReferenceData(listNames);

            if (simpleDtoList.Count == 0)
            {
                throw new BusinessLogicException(ErrorConstants.Err_MisMatchConfigFile, HttpStatusCode.ExpectationFailed);
            }

            return Ok(simpleDtoList);
        }

        #endregion Methods
    }
}