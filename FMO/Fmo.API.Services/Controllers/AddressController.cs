using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;
using Fmo.Common.Interface;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// Address controller to handle NYB/PAF API request from windows service
    /// </summary>
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private IPostalAddressBusinessService businessService = default(IPostalAddressBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public AddressController(IPostalAddressBusinessService _businessService, ILoggingHelper _loggingHelper)
        {
            businessService = _businessService;
            loggingHelper = _loggingHelper;
        }

        /// <summary>
        /// Api to save NYB details in DB.
        /// </summary>
        /// <param name="strFileName">File name</param>
        /// <param name="lstAddressDetails">List of posatl address DTO</param>
        /// <returns></returns>
        [HttpPost("SaveAddressdetails/{strFileName}")]
        public bool SaveAddressdetails(string strFileName, [FromBody]List<PostalAddressDTO> lstAddressDetails)
        {
            if (lstAddressDetails != null && lstAddressDetails.Count > 0)
                return businessService.SavePostalAddress(lstAddressDetails, strFileName);
            else
                return false;
        }

    }
}