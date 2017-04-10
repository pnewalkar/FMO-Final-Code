using Fmo.DataServices.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using Entity = Fmo.Entities;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.DBContext;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class ReferenceDataRepository : RepositoryBase<Entity.ReferenceData, FMODBContext>, IReferenceDataRepository
    {
        public ReferenceDataRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public ReferenceDataDTO GetReferenceDataId(string strDataDesc, string strDisplayText)
        {
            try
            {
                Entity.ReferenceData referenceData = DataContext.ReferenceDatas.Where(refData => refData.DataDescription.Equals(strDataDesc) && refData.DisplayText.Equals(strDisplayText)).FirstOrDefault();
                ReferenceDataDTO referenceDataDTO = new ReferenceDataDTO();
                GenericMapper.Map(referenceData, referenceDataDTO);
                return referenceDataDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
