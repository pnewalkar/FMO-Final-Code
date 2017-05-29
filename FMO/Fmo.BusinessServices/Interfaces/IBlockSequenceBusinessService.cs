using System;

namespace Fmo.BusinessServices.Interfaces
{
    public interface IBlockSequenceBusinessService
    {
        /// <summary>
        /// Method to create block sequence for delivery point
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        /// <returns>bool</returns>
        bool CreateBlockSequenceForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId);
    }
}