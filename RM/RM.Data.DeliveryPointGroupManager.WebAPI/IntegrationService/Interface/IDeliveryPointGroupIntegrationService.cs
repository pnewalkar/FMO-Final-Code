using System.Collections.Generic;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Integration
{
    public interface IDeliveryPointGroupIntegrationService
    {
        Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames);
    }
}