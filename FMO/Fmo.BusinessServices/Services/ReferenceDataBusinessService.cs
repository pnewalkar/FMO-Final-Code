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

        public List<ReferenceDataDTO> FetchRouteLogSelectionType()
        {
            throw new NotImplementedException();
        }

        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            throw new NotImplementedException();
        }
    }
}
