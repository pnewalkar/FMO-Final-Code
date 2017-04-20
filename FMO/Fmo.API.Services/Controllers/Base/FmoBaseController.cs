﻿using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// Base controller class for FMO Web API
    /// </summary>
    [Route("api/[controller]")]
    public abstract class FmoBaseController : Controller
    {
        /// <summary>
        /// Gets Current UserName
        /// </summary>
        public string UserName
        {
            get
            {
                var userName = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                               .Select(c => c.Value).SingleOrDefault();
                return userName;
            }
        }

        /// <summary>
        /// Gets Current UserId
        /// </summary>
        public Guid UserId
        {
            get
            {
                var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                               .Select(c => c.Value).SingleOrDefault();

                Guid userGuid;
                bool isGuid = Guid.TryParse(userId, out userGuid);
                return userGuid;
            }
        }

        /// <summary>
        /// Gets Current User Unit
        /// </summary>
        public Guid CurrentUserUnit
        {
            get
            {
                var userUnit = User.Claims.Where(c => c.Type == ClaimTypes.UserData)
                               .Select(c => c.Value).SingleOrDefault();
                Guid unitGuid;
                bool isGuid = Guid.TryParse(userUnit, out unitGuid);

                return unitGuid;
            }
        }
    }
}