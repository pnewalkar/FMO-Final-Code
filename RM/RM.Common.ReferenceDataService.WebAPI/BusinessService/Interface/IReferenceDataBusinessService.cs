namespace RM.Common.ReferenceData.WebAPI.BusinessService.Interface
{
    using System;
    using System.Collections.Generic;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;

    #region ReferenceData Interface

    /// <summary>
    /// This interface contains declaration of methods for fetching reference data.
    /// </summary>
    public interface IReferenceDataBusinessService
    {
        #region Reference Data Manager methods

        /// <summary>
        /// retrieval of name-value pairs as a discrete type of reference data.
        /// </summary>
        /// <param name="appGroupName">appGroupName is recorded as the category name</param>
        /// <param name="appItemName">appItemName is recorded as the item name</param>
        /// <returns>DTO of <see cref="ReferenceDataCategoryDTO"></returns>
        NameValuePair GetReferenceDataByNameValuePairs(string appGroupName, string appItemName);

        /// <summary>
        /// Retrieval of name-value pairs as a discrete type of reference data using Guid id.
        /// </summary>
        /// <param name="id">Guid id</param>
        /// <returns>NameValuePair</returns>
        NameValuePair GetReferenceDataByNameValuePairs(Guid id);

        /// <summary>
        /// retrieval of name-value pairs as a discrete type of reference data.
        /// </summary>
        /// <param name="appGroupName">appGroupName is recorded as the category name</param>
        /// <returns>DTO of <see cref="ReferenceDataCategoryDTO"></returns>
        List<NameValuePair> GetReferenceDataByNameValuePairs(string appGroupName);

        /// <summary>
        /// retrieval of simple lists as a discrete type of reference data addressed via the URI path /simpleLists
        /// </summary>
        /// <param name="listName">listname maps to ReferenceDataCategory.CategoryName</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        SimpleListDTO GetSimpleListsReferenceData(string listName);

        /// <summary>
        /// Retrieval of simple lists as a discrete type of reference data using Guid id
        /// </summary>
        /// <param name="id">Guid id</param>
        /// <returns>SimpleListDTO</returns>
        SimpleListDTO GetSimpleListsReferenceData(Guid id);

        /// <summary>
        /// retrieval of simple lists as a discrete type of reference data addressed via the URI path /simpleLists
        /// </summary>
        /// <param name="listNames">List<string listName> where listName maps to ReferenceDataCategory.CategoryName</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        List<SimpleListDTO> GetSimpleListsReferenceData(List<string> listNames);

        #endregion Reference Data Manager methods
    }

    #endregion ReferenceData Interface
}