using System;
using System.Collections.Generic;
using RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService
{
    /// <summary>
    /// This interface contains declarations of methods for fetching, Insertnig Delivery Group data.
    /// </summary>
    public interface IDeliveryPointGroupDataService
    {
        /// <summary>
        /// Add delivery point group
        /// </summary>
        /// <returns></returns>
        bool CreateDeliveryGroup(DeliveryPointGroupDataDTO deliveryPointGroup);

        /// <summary>
        /// This method is used to fetch delivery  data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List of Access Link dto</returns>
        List<DeliveryPointGroupDataDTO> GetDeliveryGroups(string boundingBoxCoordinates, Guid unitGuid);

        DeliveryPointGroupDataDTO UpdateDeliveryGroup(DeliveryPointGroupDataDTO deliveryPointGroupDto);

        DeliveryPointGroupDataDTO GetDeliveryGroup(Guid deliveryGroupId);
    }
}