﻿using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;
using Fmo.Common.Interface;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
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

        [HttpPost("SaveAddressdetails")]
        public bool SaveAddressdetails([FromBody]List<PostalAddressDTO> lstAddressDetails)
        {
            bool saveFlag = false;
            try
            {
                saveFlag = businessService.SavePostalAddress(lstAddressDetails);
            }
            catch (Exception ex)
            {
                saveFlag = false;
                loggingHelper.LogError(ex);
            }
            return saveFlag;
        }

        [HttpGet("getSample")]
        public string GetSample()
        {
            return "virendra";
        }
    }
}