using System;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IBlockSequenceBusinessService
    {
        /// <summary>
        /// Method to create block sequence for delivery point
        /// </summary>
        /// <param name="deliveryRouteId">Route ID</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        void CreateBlockSequenceForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId);
    }
}