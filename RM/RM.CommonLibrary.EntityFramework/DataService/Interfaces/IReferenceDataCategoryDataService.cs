using RM.CommonLibrary.EntityFramework.DTO;
using System;
using System.Collections.Generic;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    /// <summary>
    /// IReferenceDataCategoryDataService interface to abstract away the ReferenceDataCategoryDataService implementation
    /// </summary>
    public interface IReferenceDataCategoryDataService
    {
        /// <summary>
        /// Get the reference data guid for a given category name and reference data name
        /// </summary>
        /// <param name="strCategoryname">Category Name</param>
        /// <param name="strRefDataName">Reference Data Name</param>
        /// <returns>Guid</returns>
        Guid GetReferenceDataId(string strCategoryname, string strRefDataName);

        /// <summary>
        /// Routes the log status.
        /// </summary>
        /// <returns>List<ReferenceDataDTO></returns>
        List<ReferenceDataDTO> RouteLogStatus();

        /// <summary>
        /// Routes the type of the log selection.
        /// </summary>
        /// <returns>List<ReferenceDataDTO></returns>
        List<ReferenceDataDTO> RouteLogSelectionType();

        /// <summary>
        /// Gets all reference category list along with associated reference data.
        /// </summary>
        /// <returns>List<ReferenceDataCategoryDTO></returns>
        List<ReferenceDataCategoryDTO> GetAllReferenceCategoryList();

        /// <summary>
        /// Gets the name of the reference data categories by category.
        /// </summary>
        /// <param name="categoryNames">The category names.</param>
        /// <returns></returns>
        List<ReferenceDataCategoryDTO> GetReferenceDataCategoriesByCategoryNames(List<string> categoryNames);

        /// <summary>
        ///  Retreive GUIDs for specified categories
        /// </summary>
        /// <param name="strCategoryname">categoryname</param>
        /// <param name="lstRefDataName">Reference data Names</param>
        /// <returns>List<Guid></returns>
        List<Guid> GetReferenceDataIds(string strCategoryname, List<string> lstRefDataName);
    }
}