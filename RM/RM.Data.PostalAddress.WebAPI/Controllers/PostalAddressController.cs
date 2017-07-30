using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.PostalAddress.WebAPI.BusinessService.Interface;
using RM.DataManagement.PostalAddress.WebAPI.Controllers;
using RM.DataManagement.PostalAddress.WebAPI.DTO;

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

        private int priority = LoggerTraceConstants.PostalAddressAPIPriority;
        private int entryEventId = LoggerTraceConstants.PostalAddressControllerMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.PostalAddressControllerMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public PostalAddressController(IPostalAddressBusinessService businessService, ILoggingHelper loggingHelper)
        {
            this.businessService = businessService;
            this.loggingHelper = loggingHelper;
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
        public async Task<IActionResult> SaveAddressdetails(string strFileName, [FromBody] List<PostalAddressDTO> lstAddressDetails)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("Controller.SaveAddressdetails"))
                {
                    string methodName = typeof(PostalAddressController) + "." + nameof(SaveAddressdetails);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

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

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(isSaved);
                }
            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    loggingHelper.Log(exception, System.Diagnostics.TraceEventType.Error);
                }

                var realExceptions = ex.Flatten().InnerException;
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
                    string methodName = typeof(PostalAddressController) + "." + nameof(SavePAFDetails);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    bool isPAFSaved = false;
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    if (postalAddress != null && postalAddress.Count > 0)
                    {
                        isPAFSaved = await this.businessService.ProcessPAFDetails(postalAddress);
                        return Ok(isPAFSaved);
                    }

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(isPAFSaved);
                }
            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    loggingHelper.Log(exception, System.Diagnostics.TraceEventType.Error);
                }

                var realExceptions = ex.Flatten().InnerException;
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
            using (loggingHelper.RMTraceManager.StartTrace("Controller.GetPostalAddressByGuid"))
            {
                string methodName = typeof(PostalAddressController) + "." + nameof(GetPostalAddressByGuid);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                PostalAddressDTO postalAddressDto = businessService.GetPostalAddressDetails(addressGuid);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Ok(postalAddressDto);
            }
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
                using (loggingHelper.RMTraceManager.StartTrace("Controller.GetPostalAddress"))
                {
                    string methodName = typeof(PostalAddressController) + "." + nameof(GetPostalAddress);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    PostalAddressDTO postalAddressDTO = await businessService.GetPostalAddress(uDPRN);

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return Ok(postalAddressDTO);
                }
            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    loggingHelper.Log(exception, System.Diagnostics.TraceEventType.Error);
                }

                var realExceptions = ex.Flatten().InnerException;
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
        public async Task<IActionResult> CheckForDuplicateNybRecords([FromBody] PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Controller.CheckForDuplicateNybRecords"))
            {
                string methodName = typeof(PostalAddressController) + "." + nameof(CheckForDuplicateNybRecords);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string postCode = await businessService.CheckForDuplicateNybRecords(objPostalAddress);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Ok(postCode);
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
        public async Task<IActionResult> CheckForDuplicateAddressWithDeliveryPoints([FromBody] PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Controller.CheckForDuplicateAddressWithDeliveryPoints"))
            {
                string methodName = typeof(PostalAddressController) + "." + nameof(CheckForDuplicateAddressWithDeliveryPoints);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                bool isDuplicate = await businessService.CheckForDuplicateAddressWithDeliveryPoints(objPostalAddress);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return Ok(isDuplicate);
            }
        }

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        [HttpPost("postaladdress/savedeliverypointaddress/")]
        [Authorize(Roles = UserAccessFunctionsConstants.MaintainDeliveryPoints)]
        public IActionResult CreateAddressForDeliveryPoint([FromBody] AddDeliveryPointDTO addDeliveryPointDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("Controller.CreateAddressForDeliveryPoint"))
                {
                    string methodName = typeof(PostalAddressController) + "." + nameof(CreateAddressForDeliveryPoint);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    var deliveryPointAddressDetails = businessService.CreateAddressForDeliveryPoint(addDeliveryPointDTO);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(deliveryPointAddressDetails);
                }
            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ex.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        ///  Get Postal Addresses on adress guid's  as search criteria
        /// </summary>
        /// <param name="addressGuids"></param>
        /// <returns></returns>
        [HttpPost("postaladdress/postaladdresses/")]
        public async Task<IActionResult> GetPostalAddresses([FromBody] List<Guid> addressGuids)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("Controller.GetPostalAddresses"))
                {
                    string methodName = typeof(PostalAddressController) + "." + nameof(GetPostalAddresses);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                    var addressDetails = await businessService.GetPostalAddresses(addressGuids);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return Ok(addressDetails);
                }
            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ex.Flatten().InnerException;
                throw realExceptions;
            }
        }

        /// <summary>
        /// Get Postal Address on UDPRN value
        /// </summary>
        /// <param name="udprn">udprn value of PostalAddress</param>
        /// <returns></returns>
        [HttpGet("postaladdress/pafaddress/{udprn}")]
        public async Task<PostalAddressDTO> GetPAFAddress(int udprn)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("Controller.GetPAFAddress"))
                {
                    string methodName = typeof(PostalAddressController) + "." + nameof(GetPAFAddress);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    var postalAddress = await businessService.GetPAFAddress(udprn);
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return postalAddress;
                }
            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    loggingHelper.Log(exception, TraceEventType.Error);
                }

                var realExceptions = ex.Flatten().InnerException;
                throw realExceptions;
            }
        }

        #endregion Public Methods
    }
}