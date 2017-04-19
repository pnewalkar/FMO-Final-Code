using System;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.MappingConfiguration;
using Entity = Fmo.Entities;

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