using RM.CommonLibrary.EntityFramework.DTO;
using System;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DataService.Interfaces
{
    public interface IBlockSequenceDataService
    {
        /// <summary>
        /// Save block sequence in database
        /// </summary>
        /// <param name="blockSequenceDTO">blockSequenceDTO</param>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <returns>bool</returns>
        Task<bool> AddBlockSequence(BlockSequenceDTO blockSequenceDTO, Guid deliveryRouteId);
    }
}