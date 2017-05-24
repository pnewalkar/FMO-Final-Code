﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Interface;
using Fmo.DTO;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// PAF controller to handle PAF API request from windows service
    /// </summary>
    [Route("api/[controller]")]
    public class PAFController : FmoBaseController
    {
        private IPostalAddressBusinessService postalAddressBusinessService = default(IPostalAddressBusinessService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public PAFController(IPostalAddressBusinessService postalAddressBusinessService, ILoggingHelper loggingHelper)
        {
            this.postalAddressBusinessService = postalAddressBusinessService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Api to save PAF details in DB.
        /// </summary>
        /// <param name="postalAddress">List of posatl address DTO</param>
        /// <returns></returns>
        [HttpPost("SavePAFDetails")]
        public IActionResult SavePAFDetails([FromBody] List<PostalAddressDTO> postalAddress)
        {
            bool IsPAFSaved = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (postalAddress != null && postalAddress.Count > 0)
                IsPAFSaved = postalAddressBusinessService.SavePAFDetails(postalAddress);

            return Ok(IsPAFSaved);
        }
    }
}