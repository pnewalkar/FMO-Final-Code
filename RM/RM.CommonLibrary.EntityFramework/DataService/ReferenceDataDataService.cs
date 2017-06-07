using System;
using System.Collections.Generic;
using System.Linq;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    public class ReferenceDataDataService : DataServiceBase<ReferenceData, RMDBContext>, IReferenceDataDataService
    {
        public ReferenceDataDataService(IDatabaseFactory<RMDBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public ReferenceDataDTO GetReferenceDataId(string strDataDesc, string strDisplayText)
        {
            ReferenceData referenceData = DataContext.ReferenceDatas.Where(refData => refData.DataDescription.Equals(strDataDesc) && refData.ReferenceDataValue.Equals(strDisplayText)).FirstOrDefault();
            return GenericMapper.Map<ReferenceData, ReferenceDataDTO>(referenceData);
        }

        /// <summary>
        /// Gets the name of the reference data by category.
        /// </summary>
        /// <param name="categoryName">The string categoryname.</param>
        /// <returns>List ReferenceDataDTO</returns>
        public List<ReferenceDataDTO> GetReferenceDataByCategoryName(string categoryName)
        {
            var referenceData = DataContext.ReferenceDatas.AsNoTracking().Where(n => n.ReferenceDataCategory.CategoryName.Trim().Equals(categoryName, StringComparison.OrdinalIgnoreCase)).ToList();
            return GenericMapper.MapList<ReferenceData, ReferenceDataDTO>(referenceData);
        }
    }
}