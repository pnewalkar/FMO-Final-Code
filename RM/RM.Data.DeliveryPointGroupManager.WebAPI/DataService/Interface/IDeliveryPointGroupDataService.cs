using System;
using System.Collections.Generic;
using RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService
{
    public interface IDeliveryPointGroupDataService
    {
        List<DeliveryPointGroupDataDTO> GetDeliveryGroups(string boundingBoxCoordinates, Guid unitGuid);

        DeliveryPointGroupDataDTO UpdateDeliveryGroup(DeliveryPointGroupDataDTO deliveryPointGroupDto);

        DeliveryPointGroupDataDTO GetDeliveryGroup(Guid deliveryGroupId);
    }
}