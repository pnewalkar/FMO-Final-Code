using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.Data.UnitManager.WebAPI.DTO;

namespace RM.DataManagement.UnitManager.WebAPI.BusinessService.Integration.Interface
{
    /// <summary>
    /// Interface for Integration service
    /// </summary>
    public interface IUnitManagerIntegrationService
    {
        /// <summary>
        ///  Retreive reference data details from ReferenceData Web API
        /// </summary>
        /// <param name="categoryName">categoryname</param>
        /// <param name="itemName">Reference data Name</param>
        /// <returns>GUID</returns>
        Task<Guid> GetReferenceDataGuId(string categoryName, string itemName);

        /// <summary>
        /// Gets the name of the reference data categories by category.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns>ReferenceDataCategoryDTO</returns>
        Task<ReferenceDataCategoryDTO> GetReferenceDataSimpleLists(string listName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="postcode"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<List<DeliveryRouteDTO>> GetRouteData(string postcode, string fields);
    }
}