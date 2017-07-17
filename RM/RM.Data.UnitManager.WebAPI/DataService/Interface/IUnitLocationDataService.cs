using RM.DataManagement.UnitManager.WebAPI.DataDTO;
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
        /// <param name="postcodeAreaGUID">The post code area unique identifier.</param>
        /// <returns>
        /// The list of <see cref="UnitLocationDataDTO"/>.
        /// </returns>
        Task<IEnumerable<UnitLocationDataDTO>> GetDeliveryUnitsByUser(Guid userId, Guid postcodeAreaGUID);

        /// <summary>
        /// Gets postcodes details by postcodeGuids
        /// </summary>
        /// <param name="postcodeGuids"></param>
        /// <param name="postcodeSectorGUID"></param>
        /// <returns></returns>
        Task<List<PostcodeDataDTO>> GetPostcodes(List<Guid> postcodeGuids, Guid postcodeSectorGUID);
    }
}