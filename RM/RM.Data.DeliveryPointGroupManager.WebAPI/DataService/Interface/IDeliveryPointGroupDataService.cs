using RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO;
using System;
using System.Collections.Generic;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService
{
    public interface IDeliveryPointGroupDataService
    {

        /// <summary>
        /// This method is used to fetch delivery  data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates.</param>
        /// <param name="unitGuid">The unit unique identifier.</param>
        /// <returns>List of Access Link dto</returns>
        List<DeliveryPointGroupDataDTO> GetDeliveryGroups(string boundingBoxCoordinates, Guid unitGuid);
    }
}