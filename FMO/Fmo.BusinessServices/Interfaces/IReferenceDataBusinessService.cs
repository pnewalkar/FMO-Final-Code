using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IReferenceDataBusinessService
    {
        /// <summary>
        /// Fetch the Delivery Route status.
        /// </summary>
        /// <returns>List</returns>
        List<ReferenceDataDTO> FetchRouteLogStatus();

        /// <summary>
        /// Fetch the Delivery Route Selection Type.
        /// </summary>
        /// <returns>List</returns>
        List<ReferenceDataDTO> FetchRouteLogSelectionType();

        /// <summary>
        /// Gets all reference category list along with associated reference data.
        /// </summary>
        /// <returns>List<ReferenceDataCategoryDTO></returns>
        List<ReferenceDataCategoryDTO> GetAllReferenceCategoryList();
    }
}
