using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService
{
    public interface IDeliveryPointGroupDataService
    {
        DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto);
    }
}