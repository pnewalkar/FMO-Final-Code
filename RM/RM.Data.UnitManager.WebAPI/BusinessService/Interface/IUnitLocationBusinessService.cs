﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using RM.DataManagement.UnitManager.WebAPI.DTO;

namespace RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface
{
    /// <summary>
    /// This interface contains declaration of methods for fetching Delivery Unit data.
    /// </summary>
    public interface IUnitLocationBusinessService
    {
        /// <summary>
        /// Gets all the associated Delivery units for an user.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>
        /// The list of <see cref="UnitLocationDTO"/>.
        /// </returns>
        Task<IEnumerable<UnitLocationDTO>> GetDeliveryUnitsForUser(Guid userId);

        /// <summary>
        /// Get the postcode sector by the UDPRN id
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>PostCodeSectorDTO object</returns>
        Task<PostCodeSectorDTO> GetPostcodeSectorByUdprn(int udprn);

        /// <summary>
        /// Gets first five postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitlocationId"></param>
        /// <param name="postcodeTypeGUID"></param>
        /// <returns>list of PostCodeDTO</returns>
        Task<IEnumerable<PostCodeDTO>> GetPostcodeUnitForBasicSearch(string searchText, Guid unitlocationId);

        /// <summary>
        /// Gets count of postcodeunits for an unit for a given search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="unitlocationId"></param>
        /// <param name="postcodeTypeGUID"></param>
        /// <returns>count of postcodeunits</returns>
        Task<int> GetPostcodeUnitCount(string searchText, Guid unitlocationId);

        /// <summary>
        /// Fetch the post code unit for advance search.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="userUnit Guid Id"></param>
        /// <returns></returns>
        Task<IEnumerable<PostCodeDTO>> GetPostcodeUnitForAdvanceSearch(string searchText, Guid unitlocationId);

        /// <summary>
        /// Get the list of route scenarios by the operationstateID and locationID.
        /// </summary>
        /// <param name="operationStateID"></param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        Task<IEnumerable<ScenarioDTO>> GetRouteScenarios(Guid operationStateID, Guid locationID);

        /// <summary>
        /// Get post code ID by passing post code.
        /// </summary>
        /// <param name="postCode"> Post Code</param>
        /// <returns>Post code ID</returns>
        Task<Guid> GetPostcodeID(string postCode);

        /// <summary>
        /// Get post code details by post code guid id.
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <returns></returns>
        Task<IEnumerable<PostCodeDTO>> GetPostcodes(List<Guid> postcodeGuids);

        /// <summary>
        /// Gets approx location based on the postal code.
        /// </summary>
        /// <param name="postcode">Postal code</param>
        /// <param name="unitId">Unique identifier for unit.</param>
        /// <returns>The approx location for the given postal code.</returns>
        Task<DbGeometry> GetApproxLocation(string postcode, Guid unitId);
    }
}