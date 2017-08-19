using System.Threading.Tasks;

using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;

using RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO;
using System;
using System.Collections.Generic;

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
        Task CreateDeliveryGroup();

        /// <summary>
        /// This method is used to fetch delivery  data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List of Access Link dto</returns>
        List<DeliveryPointGroupDataDTO> GetDeliveryGroups(string boundingBoxCoordinates, Guid unitGuid);

        DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto);
    }
}