using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class ReferenceDataBusinessService : IReferenceDataBusinessService
    {
        private IReferenceDataCategoryRepository referenceDataCategoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDataBusinessService"/> class and other classes.
        /// </summary>
        /// <param name="referenceDataCategoryRepository">IReferenceDataCategoryRepository reference</param>   
        public ReferenceDataBusinessService(IReferenceDataCategoryRepository referenceDataCategoryRepository)
        {
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
        }

        /// <summary>
        /// Fetch the Route Log Status.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        /// <summary>
        /// Fetch the Route Log Selection Type.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> FetchRouteLogSelectionType()
        {
            return referenceDataCategoryRepository.RouteLogSelectionType();
        }
    }
}
