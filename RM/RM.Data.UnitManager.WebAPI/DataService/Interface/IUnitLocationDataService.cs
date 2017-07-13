using RM.CommonLibrary.EntityFramework.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    public interface IUnitLocationDataService
    {
        /// <summary>
        /// Gets the all delivery units for an user.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>
        /// The list of <see cref="UnitLocationDTO"/>.
        /// </returns>
        List<UnitLocationDTO> GetDeliveryUnitsForUser(Guid userId);

        /// <summary>
        /// Gets postcodes details by postcodeGuids
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <param name="postcodeSectorGUID"></param>
        /// <returns></returns>
        Task<List<PostCodeDTO>> GetPostCodeDetails(List<Guid> postcodeGuids, Guid postcodeSectorGUID);
    }
}