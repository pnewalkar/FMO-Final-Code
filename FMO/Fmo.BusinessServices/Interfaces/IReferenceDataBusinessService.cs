using System;
using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
    /// <summary>
    /// This interface contains declaration of methods for fetching reference data.
    /// </summary>
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

        /// <summary>
        /// Get Reference datav Guid by Categoryname and RefDataName
        /// </summary>
        /// <param name="strCategoryname"></param>
        /// <param name="strRefDataName"></param>
        /// <returns>Guid</returns>
        Guid GetReferenceDataId(string strCategoryname, string strRefDataName);

        /// <summary>
        /// Gets the name of the reference data categories by category.
        /// </summary>
        /// <param name="categoryNames">The category names.</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        List<ReferenceDataCategoryDTO> GetReferenceDataCategoriesByCategoryNames(List<string> categoryNames);
    }
}