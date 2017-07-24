using System;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    public interface IBlockSequenceDataService
    {
        /// <summary>
        /// method to save delivery point and selected route mapping in block sequence table
        /// </summary>
        /// <param name="routeId">selected route id</param>
        /// <param name="deliveryPointId">Delivery point unique id</param>
        void SaveDeliveryPointRouteMapping(Guid routeId, Guid deliveryPointId);
    }
}