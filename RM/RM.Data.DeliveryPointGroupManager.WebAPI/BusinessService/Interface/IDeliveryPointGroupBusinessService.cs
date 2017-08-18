using System;
using System.Threading.Tasks;
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.BusinessService
{
    public interface IDeliveryPointGroupBusinessService
    {
        string GetDeliveryPointGroups(string boundaryBox, Guid unitGuid);

        DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto);
    }
}