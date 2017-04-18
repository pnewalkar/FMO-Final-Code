using System;
using System.Collections.Generic;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IReferenceDataCategoryRepository
    {
        Guid GetReferenceDataId(string strCategoryname, string strRefDataName);

        List<ReferenceDataDTO> RouteLogStatus();

        List<ReferenceDataDTO> RouteLogSelectionType();
    }
}