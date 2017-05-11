using System;
using System.Collections.Generic;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MappingConfiguration;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    public class ReferenceDataRepository : RepositoryBase<ReferenceData, FMODBContext>, IReferenceDataRepository
    {
        public ReferenceDataRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public ReferenceDataDTO GetReferenceDataId(string strDataDesc, string strDisplayText)
        {
            ReferenceData referenceData = DataContext.ReferenceDatas.Where(refData => refData.DataDescription.Equals(strDataDesc) && refData.DisplayText.Equals(strDisplayText)).FirstOrDefault();
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