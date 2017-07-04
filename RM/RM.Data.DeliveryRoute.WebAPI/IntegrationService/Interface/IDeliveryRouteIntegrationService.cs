using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService
{
    public interface IDeliveryRouteIntegrationService
    {
        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> categoryNames);

        /// <summary>
        ///  Retreive reference data details from ReferenceData Web API
        /// </summary>
        /// <param name="categoryName">categoryname</param>
        /// <param name="itemName">Reference data Name</param>
        /// <returns>GUID</returns>
        Task<Guid> GetReferenceDataGuId(string categoryName, string itemName);

        /// <summary>
        /// Fetches unit Location type id for current user
        /// </summary>
        /// <returns>Guid</returns>
        Task<Guid> GetUnitLocationTypeId(Guid unitId);
    }
}