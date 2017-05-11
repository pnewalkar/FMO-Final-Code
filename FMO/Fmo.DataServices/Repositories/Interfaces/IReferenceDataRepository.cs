using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IReferenceDataRepository
    {
        ReferenceDataDTO GetReferenceDataId(string strDataDesc, string strDisplayText);

        List<ReferenceDataDTO> GetReferenceDataByCategoryName(string categoryName);
    }
}