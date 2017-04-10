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

        public Guid GetReferenceDataId(string strCategoryname, string strRefDataName)
        {
            Guid statusId = Guid.Empty;
            var result = DataContext.ReferenceDataCategories.Include(m => m.ReferenceDatas).Where(n => string.Equals(n.CategoryName, strCategoryname, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            if (result != null)
            {
                if (result.ReferenceDatas != null && result.ReferenceDatas.Count > 0)
                {
                    var referenceData = result.ReferenceDatas.Where(n => n.ReferenceDataName == strRefDataName).SingleOrDefault();
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
            var query = DataContext.ReferenceDatas.Join(DataContext.ReferenceDataCategories, r => r.ReferenceDataCategory_GUID, p => p.ID, (r, p) => new { r.ReferenceDataCategory_GUID, r.DataDescription, r.DisplayText })
                .Select(a => new Entity.ReferenceData { DataDescription = a.DataDescription, DisplayText = a.DisplayText, ReferenceDataCategory_GUID = a.ReferenceDataCategory_GUID });
            return GenericMapper.MapList<Entity.ReferenceData, DTO.ReferenceDataDTO>(query.ToList());
        }
    }
}