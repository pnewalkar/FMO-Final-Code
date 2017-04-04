using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IReferenceDataCategoryRepository
    {
        int GetReferenceDataId(string strCategoryname, string strRefDataName);

        List<DTO.ReferenceDataDTO> RouteLogStatus();
    }
}