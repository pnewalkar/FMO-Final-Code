using RM.CommonLibrary.EntityFramework.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    public interface IUnitLocationDataService
    {
        /// <summary>
        /// Fetch the Delivery unit.
        /// </summary>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>
        /// The <see cref="UnitLocationDTO"/>.
        /// </returns>
        UnitLocationDTO FetchDeliveryUnit(Guid unitGuid);

        /// <summary>
        /// Fetch the Delivery units for an user.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>
        /// The list of <see cref="UnitLocationDTO"/>.
        /// </returns>
        List<UnitLocationDTO> FetchDeliveryUnitsForUser(Guid userId);

        Task<List<PostCodeDTO>> GetPostCodes(List<Guid> postcodeGuids, Guid unitGuid);

        Task<PostCodeDTO> GetSelectedPostcode(Guid postcodeGuid, Guid unitGuid);
    }
}