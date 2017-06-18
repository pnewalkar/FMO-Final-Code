using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.DataManagement.PostalAddress.WebAPI.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.Controllers;
using System.Diagnostics;
using RM.CommonLibrary.Utilities.HelperMiddleware;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// Address controller to handle NYB/PAF API request from windows service
    /// </summary>
    [Route("api/postaladdressmanager")]
    public class PostalAddressController : RMBaseController
    {
        #region Member Variables

        private IPostalAddressBusinessService businessService = default(IPostalAddressBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion Member Variables

        #region Constructors

        public PostalAddressController(IPostalAddressBusinessService _businessService, ILoggingHelper _loggingHelper)
        {
            businessService = _businessService;
            loggingHelper = _loggingHelper;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Api to save NYB details in DB.
        /// </summary>
        /// <param name="strFileName">File name</param>
        /// <param name="lstAddressDetails">List of posatl address DTO</param>
        /// <returns></returns>
        // [HttpPost("SaveAddressdetails/{strFileName}")]
        [HttpPost("nybaddresses/{strFileName}")]
        public async Task<IActionResult> SaveAddressdetails(string strFileName, [FromBody] List<PostalAddressDBDTO> lstAddressDetails)
        {
            try
            {
                bool isSaved = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                {
                    isSaved = await businessService.SavePostalAddressForNYB(lstAddressDetails, strFileName);
                    return Ok(isSaved);
                }

                return Ok(isSaved);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, System.Diagnostics.TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Api to save PAF details in DB.
        /// </summary>
        /// <param name="postalAddress">List of posatl address DTO</param>
        /// <returns></returns>
        [HttpPost("pafaddresses")]
        public async Task<IActionResult> SavePAFDetails([FromBody] List<PostalAddressDTO> postalAddress)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("Controller.SavePAFDetails"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressControllerMethodEntryEventId, LoggerTraceConstants.Title);
                    bool IsPAFSaved = false;
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (postalAddress != null && postalAddress.Count > 0)
                    {
                        IsPAFSaved = await this.businessService.SavePAFDetails(postalAddress);
                        return Ok(IsPAFSaved);
                    }
                    loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressControllerMethodExitEventId, LoggerTraceConstants.Title);
                    return Ok(IsPAFSaved);
                }
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, System.Diagnostics.TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Api searches pstcode and thorough in postal address entity on basis of searhtext
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <returns></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]

        // [HttpGet("SearchAddressdetails")]
        [HttpGet("postaladdress/search/{searchText}")]
        public async Task<IActionResult> SearchAddressdetails(string searchText)
        {
            try
            {
                List<string> postalAddressList = await businessService.GetPostalAddressSearchDetails(searchText, CurrentUserUnit);
                return Ok(postalAddressList);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, System.Diagnostics.TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Filters postal address on basis of postcode
        /// </summary>
        /// <param name="postCode">postcode</param>
        /// <returns></returns>
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [HttpGet("postaladdress/filter")]
        public async Task<IActionResult> GetAddressByPostCode(string selectedItem)
        {
            try
            {
                PostalAddressDBDTO postalAddressDto = await businessService.GetPostalAddressDetails(selectedItem, CurrentUserUnit);
                return Ok(postalAddressDto);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, System.Diagnostics.TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        ///  Filters postal address on basis of postal address id.
        /// </summary>
        /// <param name="addressGuid"></param>
        /// <returns>PostalAddressDTO</returns>
        // [HttpGet("GetPostalAddressByGuid")]
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        [HttpGet("postaladdress/filter/addressguid:{addressguid}")]
        public IActionResult GetPostalAddressByGuid(Guid addressGuid)
        {
            PostalAddressDBDTO postalAddressDto = businessService.GetPostalAddressDetails(addressGuid);
            return Ok(postalAddressDto);
        }

        /// <summary>
        /// Get the existing postal address details based on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostalAddress DTO</returns>
        // [HttpGet("GetPostalAddress")]
        [HttpGet("postaladdress/filter/udprn:{udprn}")]
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        public async Task<IActionResult> GetPostalAddress(int? uDPRN)
        {
            try
            {
                PostalAddressDBDTO postalAddressDTO = await businessService.GetPostalAddress(uDPRN);
                return Ok(postalAddressDTO);
            }
            catch (AggregateException ae)
            {
                foreach (var exception in ae.InnerExceptions)
                {
                    loggingHelper.Log(exception, System.Diagnostics.TraceEventType.Error);
                }

                var realExceptions = ae.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">PostalAddressDTO as input</param>
        /// <returns>string</returns>
        // [HttpPost("CheckForDuplicateNybRecords")]
        [HttpPost("postaladdress/nybduplicate/")]
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        public async Task<IActionResult> CheckForDuplicateNybRecords([FromBody] PostalAddressDBDTO objPostalAddress)
        {
            try
            {
                string postCode = businessService.CheckForDuplicateNybRecords(objPostalAddress);
                return Ok(postCode);
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
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        // [HttpPost("CheckForDuplicateAddressWithDeliveryPoints")]
        [HttpPost("postaladdress/duplicatedeliverypoint/")]
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        public async Task<IActionResult> CheckForDuplicateAddressWithDeliveryPoints([FromBody] PostalAddressDBDTO objPostalAddress)
        {
            try
            {
                bool isDuplicate = businessService.CheckForDuplicateAddressWithDeliveryPoints(objPostalAddress);
                return Ok(isDuplicate);
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
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        [HttpPost("postaladdress/savedeliverypointaddress/")]
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        public async Task<IActionResult> CreateAddressAndDeliveryPoint([FromBody] AddDeliveryPointDTO addDeliveryPointDTO)
        {
            try
            {
                var deliveryPointAddressDetails = businessService.CreateAddressAndDeliveryPoint(addDeliveryPointDTO);
                return Ok(deliveryPointAddressDetails);
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


        [HttpPost("postaladdress/postaladdresses/")]
        public async Task<IActionResult> GetPostalAddresses([FromBody] List<Guid> addressGuids)
        {
            try
            {
                var addressDetails = await businessService.GetPostalAddresses(addressGuids);
                return Ok(addressDetails);
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

        #endregion Public Methods
    }
}