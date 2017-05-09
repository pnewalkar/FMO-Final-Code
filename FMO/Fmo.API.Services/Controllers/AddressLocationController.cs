using Microsoft.AspNetCore.Mvc;
using Fmo.DTO;
using Fmo.BusinessServices.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    /// <summary>
    /// This class contains methods used to fetch Address Location data.
    /// </summary>
    [Route("api/[controller]")]
    public class AddressLocationController : FmoBaseController
    {
        private IAddressLocationBusinessService addressLocationBussinessService = default(IAddressLocationBusinessService);

        public AddressLocationController(IAddressLocationBusinessService addressLocationBussinessService)
        {
            this.addressLocationBussinessService = addressLocationBussinessService;
        }

        [HttpGet("GetAddressLocationByUDPRN")]
        public object GetAddressLocationByUDPRN(int uDPRN)
        {
            return this.addressLocationBussinessService.GetAddressLocationByUDPRN(uDPRN);
        }
    }
}
