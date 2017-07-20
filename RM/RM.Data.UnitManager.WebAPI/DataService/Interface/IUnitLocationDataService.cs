using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    public interface IUnitLocationDataService
    {
        /// <summary>
        /// Gets the all units for an user.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <param name="postcodeAreaGUID">The post code area unique identifier.</param>
        /// <returns>
        /// The list of <see cref="UnitLocationDataDTO"/>.
        /// </returns>
        Task<IEnumerable<UnitLocationDataDTO>> GetUnitsByUser(Guid userId, Guid postcodeAreaGUID);

        /// <summary>
        /// Gets the all units for an user whose access level is above mail centre.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postcodeAreaGUID"></param>
        /// <param name="currentUserUnitType"></param>
        /// The collection of <see cref="UnitLocationDataDTO"/>.
        Task<IEnumerable<UnitLocationDataDTO>> GetUnitsByUserForNational(Guid userId, Guid postcodeAreaGUID);

        /// <summary>
        /// Gets postcodes details by postcodeGuids
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <param name="postcodeSectorGUID"></param>
        /// <returns></returns>
        Task<List<PostcodeDataDTO>> GetPostcodes(List<Guid> postcodeGuids, Guid postcodeSectorGUID);
    }
}