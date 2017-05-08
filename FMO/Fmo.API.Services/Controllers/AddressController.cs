using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Interface;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// Address controller to handle NYB/PAF API request from windows service
    /// </summary>
    [Route("api/[controller]")]
    public class AddressController : FmoBaseController
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

        /// <summary>
        /// Api searches pstcode and thorough in postal address entity on basis of searhtext
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <returns></returns>
        [HttpGet("SearchAddressdetails")]
        public async Task<List<string>> SearchAddressdetails(string searchText)
        {
            return await businessService.GetPostalAddressSearchDetails(searchText, CurrentUserUnit);
        }

        /// <summary>
        /// Filters postal address on basis of postcode
        /// </summary>
        /// <param name="postCode">postcode</param>
        /// <returns></returns>
        [HttpGet("GetAddressByPostCode")]
        public async Task<PostalAddressDTO> GetAddressByPostCode(string postCode)
        {
            return await businessService.GetPostalAddressDetails(postCode, CurrentUserUnit);
        }

        /// <summary>
        ///  Filters postal address on basis of postal address id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetPostalAddressByGuid")]
        public PostalAddressDTO GetPostalAddressByGuid(Guid addressGuid)
        {
            return businessService.GetPostalAddressDetails(addressGuid);
        }
    }
}