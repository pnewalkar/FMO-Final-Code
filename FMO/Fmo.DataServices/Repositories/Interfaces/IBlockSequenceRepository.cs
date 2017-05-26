using System;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IBlockSequenceRepository
    {
        /// <summary>
        /// Save block sequence in database
        /// </summary>
        /// <param name="blockSequenceDTO">blockSequenceDTO</param>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        void AddBlockSequence(BlockSequenceDTO blockSequenceDTO, Guid deliveryRouteId);
    }
}