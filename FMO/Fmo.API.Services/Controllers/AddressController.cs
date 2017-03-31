﻿using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private IPostalAddressBusinessService businessService = default(IPostalAddressBusinessService);

        public AddressController(IPostalAddressBusinessService _businessService)
        {
            businessService = _businessService;
        }

        [HttpPost("SaveAddressdetails")]
        public bool SaveAddressdetails([FromBody]List<PostalAddressDTO> lstAddressDetails)
        {
            bool saveFlag = false;
            try
            {
                businessService.SavePostalAddress(lstAddressDetails);
            }
            catch (Exception)
            {
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