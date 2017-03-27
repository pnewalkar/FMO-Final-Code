using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IReferenceDataCategoryRepository
    {
        List<ReferenceDataCategoryDTO> GetReferenceDataCategoryDetails(string strCategoryname);
    }
}