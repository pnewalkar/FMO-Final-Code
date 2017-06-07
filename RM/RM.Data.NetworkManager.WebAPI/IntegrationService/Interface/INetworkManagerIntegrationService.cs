using System.Collections.Generic;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.DataManagement.NetworkManager.WebAPI.IntegrationService
{
    public interface INetworkManagerIntegrationService
    {
        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames);

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        Task<List<ReferenceDataCategoryDTO>> GetReferenceDataNameValuePairs(List<string> categoryNames);
    }
}