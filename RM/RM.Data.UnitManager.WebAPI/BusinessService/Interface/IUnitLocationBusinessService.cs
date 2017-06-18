using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DTO;

namespace RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface
{
    /// <summary>
    /// This interface contains declaration of methods for fetching Delivery Unit data.
    /// </summary>
    public interface IUnitLocationBusinessService
    {
        /// <summary>
        /// Fetch the Delivery units for an user.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>
        /// The list of <see cref="UnitLocationDTO"/>.
        /// </returns>
        List<UnitLocationDTO> FetchDeliveryUnitsForUser(Guid userId);

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        Task<PostCodeSectorDTO> GetPostCodeSectorByUDPRN(int uDPRN);

        /// <summary>
        /// Fetch the post code unit for basic search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        Task<List<PostCodeDTO>> FetchPostCodeUnitForBasicSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Get the post code unit count
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        Task<int> GetPostCodeUnitCount(string searchText, Guid userUnit);

        /// <summary>
        /// Fetch the post code unit for advance search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit"></param>
        /// <returns></returns>
        Task<List<PostCodeDTO>> FetchPostCodeUnitForAdvanceSearch(string searchText, Guid userUnit);

        /// <summary>
        /// Fetch the Delivery Scenario.
        /// </summary>
        /// <param name="operationStateID"></param>
        /// <param name="deliveryScenarioID"></param>
        /// <returns></returns>
        List<ScenarioDTO> FetchDeliveryScenario(Guid operationStateID, Guid deliveryScenarioID);

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        Task<Guid> GetPostCodeID(string postCode);

        Task<List<PostCodeDTO>> GetPostCodes(Guid unitGuid, List<Guid> postcodeGuids);

        Task<PostCodeDTO> GetSelectedPostCode(Guid unitGuid, Guid postcodeGuid);
    }
}