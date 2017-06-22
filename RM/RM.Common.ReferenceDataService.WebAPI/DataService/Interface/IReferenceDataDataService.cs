namespace RM.Common.ReferenceData.WebAPI.DataService.Interface
{
    using System.Collections.Generic;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;

    /// <summary>
    /// IReferenceDataDataService interface to abstract away the ReferenceDataDataService implementation
    /// </summary>
    public interface IReferenceDataDataService
    {
        #region Reference Data Manager methods

        /// <summary>
        /// Gets all reference data.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <param name="dbItemName">dbItemName is recorded as the item name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        NameValuePair GetNameValueReferenceData(string dbGroupName, string dbItemName);

        /// <summary>
        /// Gets all reference data.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        List<NameValuePair> GetNameValueReferenceData(string dbGroupName);

        /// <summary>
        /// Gets all reference data.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <param name="dbItemName">dbItemName is recorded as the item name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        SimpleListDTO GetSimpleListReferenceData(string dbGroupName);

        #endregion Reference Data Manager methods
    }
}