using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Fmo.DTO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Fmo.API.Services.Controllers
{
    [Route("api/[controller]")]
    public class ReferenceDataController : Controller
    {
        private IReferenceDataBusinessService referenceDataBusinessService = default(IReferenceDataBusinessService);

        public ReferenceDataController(IReferenceDataBusinessService referenceDataBusinessService)
        {
            this.referenceDataBusinessService = referenceDataBusinessService;
        }


        /// <summary>
        /// Gets all reference data.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllReferenceData")]
        public IActionResult GetAllReferenceData()
        {
            List<ReferenceDataCategoryDTO> referenceDataCategorys = referenceDataBusinessService.GetAllReferenceCategoryList();
            return Ok(referenceDataCategorys);
        }
    }
}
