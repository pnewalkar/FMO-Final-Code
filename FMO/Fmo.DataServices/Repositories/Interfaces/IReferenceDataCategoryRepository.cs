using System;
using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    /// <summary>
    /// IReferenceDataCategoryRepository interface to abstract away the ReferenceDataCategoryRepository implementation
    /// </summary>
    public interface IReferenceDataCategoryRepository
    {
        /// <summary>
        /// Get the reference data guid for a given category name and reference data name
        /// </summary>
        /// <param name="strCategoryname">Category Name</param>
        /// <param name="strRefDataName">Reference Data Name</param>
        /// <returns>Guid</returns>
        Guid GetReferenceDataId(string strCategoryname, string strRefDataName);

        List<ReferenceDataDTO> RouteLogStatus();

        List<ReferenceDataDTO> RouteLogSelectionType();
    }
}