using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MappingConfiguration;
using Entity = Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    public class ReferenceDataCategoryRepository : RepositoryBase<Entity.ReferenceDataCategory, FMODBContext>, IReferenceDataCategoryRepository
    {
        public ReferenceDataCategoryRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        ///  Retreive GUID for specified category
        /// </summary>
        /// <param name="strCategoryname">categoryname</param>
        /// <param name="strRefDataName">Reference data Name</param>
        /// <returns>GUID</returns>
        public Guid GetReferenceDataId(string strCategoryname, string strRefDataName)
        {
            Guid statusId = Guid.Empty;
            var result = DataContext.ReferenceDataCategories.Include(m => m.ReferenceDatas).Where(n => n.CategoryName.Trim().Equals(strCategoryname, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            if (result != null)
            {
                if (result.ReferenceDatas != null && result.ReferenceDatas.Count > 0)
                {
                    var referenceData = result.ReferenceDatas.Where(n => n.DataDescription.Trim().Equals(strRefDataName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (referenceData != null)
                    {
                        statusId = referenceData.ID;
                    }
                }
            }

            return statusId;
        }

        public List<ReferenceDataDTO> RouteLogStatus()
        {
            List<ReferenceDataDTO> lstReferenceDt = null;
            var query = DataContext.ReferenceDataCategories.Include(m => m.ReferenceDatas).Where(n => n.CategoryName.Equals("Delivery Point Operational Status", StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            if (query != null && query.ReferenceDatas != null && query.ReferenceDatas.Count > 0)
            {
                lstReferenceDt = GenericMapper.MapList<Entity.ReferenceData, ReferenceDataDTO>(query.ReferenceDatas.ToList());
            }

            return lstReferenceDt;
        }
    }
}