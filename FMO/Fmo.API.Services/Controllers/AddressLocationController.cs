using Fmo.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods used to fetch Address Location data.
    /// </summary>
    [Route("api/[controller]")]
    public class AddressLocationController : FmoBaseController
    {
        #region Member Variables

        private IAddressLocationBusinessService addressLocationBussinessService = default(IAddressLocationBusinessService);

        #endregion Member Variables

        #region Constructors

        public AddressLocationController(IAddressLocationBusinessService addressLocationBussinessService)
        {
            this.addressLocationBussinessService = addressLocationBussinessService;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the address location by udprn.
        /// </summary>
        /// <param name="uDPRN">The u DPRN.</param>
        /// <returns>addressLocation</returns>
        [HttpGet("GetAddressLocationByUDPRN")]
        public IActionResult GetAddressLocationByUDPRN(int uDPRN)
        {
            var addressLocation = this.addressLocationBussinessService.GetAddressLocationByUDPRN(uDPRN);
            return Ok(addressLocation);
        }

        #endregion Methods
    }
}