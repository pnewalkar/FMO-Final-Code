namespace RM.Common.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Interface;
    using RM.Common.ReferenceData.WebAPI.Entities;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
    using RM.CommonLibrary.HelperMiddleware;

    public class ReferenceDataDataService : DataServiceBase<ReferenceDataCategory, ReferenceDataDBContext>, IReferenceDataDataService
    {
        #region Constructor

        public ReferenceDataDataService(IDatabaseFactory<ReferenceDataDBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Gets all reference data for category type nameValuePair.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <param name="dbItemName">dbItemName is recorded as the item name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public NameValuePair GetNameValueReferenceData(string dbGroupName, string dbItemName)
        {
            RM.Common.ReferenceData.WebAPI.Entities.ReferenceData referenceData = null;

            var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas)
                .Where(n => n.CategoryName.Equals(dbGroupName, StringComparison.OrdinalIgnoreCase)
                            && n.CategoryType.Equals(Constants.ReferenceDataCategoryTypeForNameValuePair)).SingleOrDefault();

            if (referenceDataCategories?.ReferenceDatas != null && referenceDataCategories.ReferenceDatas.Count > 0)
            {
                referenceData = referenceDataCategories.ReferenceDatas
                        .Where(n => n.ReferenceDataName.Trim().Equals(dbItemName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            }

            if (referenceData != null)
            {
                return new NameValuePair
                {
                    Id = referenceData.ID,
                    Group = referenceDataCategories.CategoryName,
                    Name = referenceData.ReferenceDataName,
                    Value = referenceData.ReferenceDataValue,
                    DisplayText = referenceData.DisplayText,
                    Description = referenceData.DataDescription,
                    maintainable = referenceDataCategories.Maintainable
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all reference data for category type NameValuePair.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public List<NameValuePair> GetNameValueReferenceData(string dbGroupName)
        {
            List<NameValuePair> nameValuePairList = null;

            var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas)
                .Where(n => n.CategoryName.Equals(dbGroupName, StringComparison.OrdinalIgnoreCase)
                            && n.CategoryType.Equals(Constants.ReferenceDataCategoryTypeForNameValuePair)).SingleOrDefault();
            if (referenceDataCategories?.ReferenceDatas != null && referenceDataCategories.ReferenceDatas.Count > 0)
            {
                nameValuePairList = new List<NameValuePair>();
                referenceDataCategories.ReferenceDatas.ToList().ForEach(refData => nameValuePairList.Add(
                new NameValuePair
                {
                    Id = refData.ID,
                    Group = referenceDataCategories.CategoryName,
                    Name = refData.ReferenceDataName,
                    Value = refData.ReferenceDataValue,
                    DisplayText = refData.DisplayText,
                    Description = refData.DataDescription,
                    maintainable = referenceDataCategories.Maintainable
                }));
            }

            return nameValuePairList;
        }

        /// <summary>
        /// Gets all reference data for category type SimpleList.
        /// </summary>
        /// <param name="dbGroupName">dbGroupName is recorded as the category name</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public SimpleListDTO GetSimpleListReferenceData(string dbGroupName)
        {
            List<ListItems> listItems = new List<ListItems>();

            var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas)
                .Where(n => n.CategoryName.Equals(dbGroupName, StringComparison.OrdinalIgnoreCase)
                            && n.CategoryType.Equals(Constants.ReferenceDataCategoryTypeForSimpleList)).SingleOrDefault();

            if (referenceDataCategories?.ReferenceDatas != null && referenceDataCategories.ReferenceDatas.Count > 0)
            {
                referenceDataCategories.ReferenceDatas.ToList().ForEach(refData => listItems.Add(new ListItems
                {
                    Id = refData.ID,
                    Name = refData.ReferenceDataName,
                    Value = refData.ReferenceDataValue,
                    DisplayText = refData.DisplayText,
                    Description = refData.DataDescription
                }));
            }

            if (referenceDataCategories != null)
            {
                return new SimpleListDTO
                {
                    Id = referenceDataCategories.ID,
                    ListName = referenceDataCategories.CategoryName,
                    Maintainable = referenceDataCategories.Maintainable,
                    ListItems = listItems
                };
            }
            else
            {
                return null;
            }
        }

        #endregion Methods
    }
}