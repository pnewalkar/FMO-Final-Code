using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// Unit location controller.
    /// </summary>
    [Route("api/[controller]")]
    public class UnitLocationController : FmoBaseController
    {
        private readonly IUnitLocationBusinessService unitLocationBusinessService = default(IUnitLocationBusinessService);

        public UnitLocationController(IUnitLocationBusinessService unitLocationBusinessService)
        {
            this.unitLocationBusinessService = unitLocationBusinessService;
        }

        /// <summary>
        /// Fetches Delivery Unit
        /// </summary>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("DeliveryUnit")]
        public UnitLocationDTO DeliveryUnit()
        {
            return unitLocationBusinessService.FetchDeliveryUnit(CurrentUserUnit);
        }

        /// <summary>
        /// Fetches Delivery Unit
        /// </summary>
        /// <returns>List</returns>
        [Authorize]
        [HttpGet("DeliveryUnitsForUser")]
        public List<UnitLocationDTO> DeliveryUnitsForUser()
        {
            return unitLocationBusinessService.FetchDeliveryUnitsForUser(UserId);
        }
    }
}