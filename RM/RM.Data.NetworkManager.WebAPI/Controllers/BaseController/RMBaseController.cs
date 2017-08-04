using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RM.DataManagement.NetworkManager.WebAPI.Controllers
{
    /// <summary>
    /// Base controller class for FMO Web API
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public abstract class RMBaseController : Controller
    {
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
        /// Gets Current User Unit Type. e.g: Delivery Hub or Collection Hub, etc.
        /// </summary>
        public string CurrentUserUnitType
        {
            get
            {
                var userUnitType = User.Claims.Where(c => c.Type == ClaimTypes.Upn)
                               .Select(c => c.Value).SingleOrDefault();

                return userUnitType;
            }
        }
    }
}