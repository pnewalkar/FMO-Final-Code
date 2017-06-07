using RM.CommonLibrary.EntityFramework.DTO;
using System.Collections.Generic;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    public interface IReferenceDataDataService
    {
        ReferenceDataDTO GetReferenceDataId(string strDataDesc, string strDisplayText);

        List<ReferenceDataDTO> GetReferenceDataByCategoryName(string categoryName);
    }
}