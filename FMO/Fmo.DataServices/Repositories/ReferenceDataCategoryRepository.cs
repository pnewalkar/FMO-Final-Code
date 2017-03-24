using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Entity = Fmo.Entities;
using Fmo.MappingConfiguration;
using Dto = Fmo.DTO;
using System;

namespace Fmo.DataServices.Repositories
{
    public class ReferenceDataCategoryRepository : RepositoryBase<Entity.ReferenceDataCategory, FMODBContext>, IReferenceDataCategoryRepository
    {
        public ReferenceDataCategoryRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<Dto.ReferenceDataCategory> GetReferenceDataCategoryDetails(string strCategoryname)
        {
            try
            {
                var result = DataContext.ReferenceDataCategories.Include(m => m.ReferenceDatas).Where(n => n.CategoryName == strCategoryname).ToList();
                return GenericMapper.MapList<Entity.ReferenceDataCategory, Dto.ReferenceDataCategory>(result);
            }
            catch (Exception ex)
            {
                //TO DO implement logging
                throw;
            }
        }
    }
}
