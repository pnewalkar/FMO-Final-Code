using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IReferenceDataRepository
    {
        ReferenceDataDTO GetReferenceDataId(string strDataDesc, string strDisplayText);
    }
}