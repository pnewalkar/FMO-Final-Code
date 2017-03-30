using System;
using System.Data.Entity;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Entity = Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    public class ReferenceDataCategoryRepository : RepositoryBase<Entity.ReferenceDataCategory, FMODBContext>, IReferenceDataCategoryRepository
    {
        public ReferenceDataCategoryRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public int GetReferenceDataId(string strCategoryname, string strRefDataName)
        {
            int statusId = 0;
            try
            {
                var result = DataContext.ReferenceDataCategories.Include(m => m.ReferenceDatas).Where(n => n.CategoryName == strCategoryname).SingleOrDefault();
                if (result != null)
                {
                    if (result.ReferenceDatas != null && result.ReferenceDatas.Count > 0)
                    {
                        statusId = result.ReferenceDatas.Where(n => n.ReferenceDataName == strRefDataName).SingleOrDefault().ReferenceData_Id;
                    }
                }
            }
            catch (Exception)
            {
                // TO DO implement logging
                throw;
            }

            return statusId;
        }
    }
}