using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RM.DataManagement.PostalAddress.WebAPI.Controllers
{
    /// <summary>
    /// Base controller class for FMO Web API
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public abstract class RMBaseController : Controller
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
                var userId = User.Claims.Where(c => c.Type == ClaimTypes.PrimarySid)
                               .Select(c => c.Value).FirstOrDefault();

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